
using System;
using Android.Views;
using Android.App;
using Android.Widget;
using Com.Neurosky.Connection;
using Com.Neurosky.Connection.DataType;
using Android.OS;
using Java.IO;
using Android.Util;
using Android.Bluetooth;
using Android.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using Android.Media;

namespace gnf
{
	[Activity(Label = "MindSet Helper", Icon = "@mipmap/icon")]
	public class BluetoothDeviceDemoActivity : Activity
	{
		private static String TAG = "MindSetHelper";


		private BluetoothAdapter mBluetoothAdapter;
		private BluetoothDevice mBluetoothDevice;
		private String address = null;


		private TgStreamReader tgStreamReader;
		LinkDetectedHandler linkHandler = new LinkDetectedHandler();
		static BluetoothDeviceDemoActivity Instance;
		TgStreamHandlerImplementation callback = new TgStreamHandlerImplementation();
		bool isMonitorMode = false;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Instance = this;

			Window.AddFlags(WindowManagerFlags.KeepScreenOn);

			SetContentView(Resource.Layout.bluetoothdevice_view);

			initView();

			CheckBluetooth();

			SetupSounds();

			//Debug code
			//PlaySound(1);
			//StartSignalTimer();
		}

		void CheckBluetooth()
		{
			try
			{
				mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
				if (mBluetoothAdapter == null || !mBluetoothAdapter.IsEnabled)
				{
					Toast.MakeText(
							this,
							"Please enable your Bluetooth and re-run this program !",
							ToastLength.Long).Show();
					//Finish();
					//				return;
				}
			}
			catch (Exception e)
			{
				//e.printStackTrace();
				Log.Info(TAG, "error:" + e.Message);
				return;
			}
		}

		private TextView tv_ps = null;
		private TextView tv_attention = null;
		private TextView tv_meditation = null;
		//private TextView tv_delta = null;
		private TextView tv_theta = null;
		private TextView tv_lowalpha = null;

		private TextView tv_highalpha = null;
		private TextView tv_lowbeta = null;
		private TextView tv_highbeta = null;

        TextView tv_log = null;

		private TextView tv_lowgamma = null;
		//private TextView tv_middlegamma = null;
		//private TextView tv_badpacket = null;

		private TextView tv_totalPower = null;

		private Button btn_finish = null;
		private Button btn_stop = null;
		private Button btn_selectdevice = null;
		private Button btn_monitor = null;
		private GridLayout grid_layout;

        SimpleChartView chartView;

		private int badPacketCount = 0;

		private void initView()
		{
			tv_ps = (TextView)FindViewById(Resource.Id.tv_ps);
			tv_attention = (TextView)FindViewById(Resource.Id.tv_attention);
			tv_meditation = (TextView)FindViewById(Resource.Id.tv_meditation);
			//tv_delta = (TextView)FindViewById(Resource.Id.tv_delta);
			tv_theta = (TextView)FindViewById(Resource.Id.tv_theta);
			tv_lowalpha = (TextView)FindViewById(Resource.Id.tv_lowalpha);

			tv_highalpha = (TextView)FindViewById(Resource.Id.tv_highalpha);
			tv_lowbeta = (TextView)FindViewById(Resource.Id.tv_lowbeta);
			tv_highbeta = (TextView)FindViewById(Resource.Id.tv_highbeta);

			tv_lowgamma = (TextView)FindViewById(Resource.Id.tv_lowgamma);
			//tv_middlegamma = (TextView)FindViewById(Resource.Id.tv_middlegamma);
			//%%tv_badpacket = (TextView)FindViewById(Resource.Id.tv_badpacket);
			tv_totalPower = (TextView)FindViewById(Resource.Id.tv_power_total);

			btn_finish = (Button)FindViewById(Resource.Id.btn_finish);
			btn_stop = (Button)FindViewById(Resource.Id.btn_stop);
			btn_monitor = (Button)FindViewById(Resource.Id.btn_monitormode);
            //	wave_layout = (LinearLayout)FindViewById(Resource.Id.wave_layout);

            chartView = FindViewById<SimpleChartView>(Resource.Id.chartview);
            tv_log = FindViewById<TextView>(Resource.Id.textview_log);

            grid_layout = (GridLayout)FindViewById(Resource.Id.layout_grid);
			grid_layout.Click += (sender, e) => { 
				eegSignals.AddEvent(); 
				//disable sound, interrupts meditation
				//PlaySound(0); 
			};

			btn_finish.Click += (sender, e) =>
			{
				Finish();
			};

			btn_stop.LongClick += (sender, e) => 
			//btn_stop.Click += (sender, e) =>
			{
				if (tgStreamReader != null)
				{
					tgStreamReader.Stop();
					showToast("stopping ...", ToastLength.Short);
					StopSignalTimer();
				}

			};

			btn_selectdevice = FindViewById<Button>(Resource.Id.btn_selectdevice);
			btn_selectdevice.Click += (sender, e) => ScanDeviceAndStartMeasurement(false);
			btn_monitor.Click += (sender, e) => ScanDeviceAndStartMeasurement(true);
					
			btn_finish.Visibility = ViewStates.Gone;
			btn_stop.Visibility = ViewStates.Gone;

		}

        void AddLogMessage(string str) {
            tv_log.Text = str + "\n" + tv_log.Text;
        }

        void AddLogInUiThread(string str) {
            RunOnUiThread(() => {
                AddLogMessage(str);
            });
        }

        public void stop()
		{
			if (tgStreamReader != null)
			{
				tgStreamReader.Stop();
				tgStreamReader.Close();
			}

			StopSignalTimer();

            // do not show again, program may start only once!
            //after this, only closing is possible.
            AddLogMessage("stopped.");

        }


		protected override void OnDestroy()
		{
			if (tgStreamReader != null)
			{
				tgStreamReader.Close();
				tgStreamReader = null;
			}

			base.OnDestroy();
		}

		protected override void OnStart()
		{
			base.OnStart();
		}

		protected override void OnStop()
		{
			base.OnStop();
			stop();
		}

		public override void OnBackPressed()
		{
			//do nothing (important: do not quit app accidentally by touching back button)
			//base.OnBackPressed();
		}
        
        


		// --------------------------------------


		Timer timer;
		SignalLogger signalLogger;

		void StartSignalTimer()
		{

			eegSignals = new EEGSignals();

            eegSignals.ProcessAlsoIfSignalIsPoor = true;

            alphaDetector = new AlphaDetector(eegSignals);
			signalLogger = new SignalLogger(this, eegSignals, alphaDetector);

			eegSignals.Start();
			signalLogger.Start();

			timer = new Timer(MainTick, null, 0, 1000);
		}

		void MainTick( Object state)
		{
			eegSignals.Tick();
			alphaDetector.Tick();
			signalLogger.LogSignals();

			// meditation program like pings every x seconds
			TickForProgram();


			RunOnUiThread(() => { 
				DisplayAlpha(); 
				DisplaySignals();
				if (isMonitorMode)
				{
					MonitorAndDisplaySignals();
				}
			});
		}


		void StopSignalTimer()
		{
			if (timer != null)
			{
				timer.Dispose();
				eegSignals.Close();
				signalLogger.Close();
			}

			btn_stop.Visibility = ViewStates.Gone;
			btn_finish.Visibility = ViewStates.Visible;
		}

		// --------------------------------------

		EEGSignals eegSignals;
		AlphaDetector alphaDetector;

		bool alphaEntered = false;

		void DisplayAlpha()
		{
			if (eegSignals.IsPoorSignal)
			{
				grid_layout.SetBackgroundColor(Android.Graphics.Color.DarkRed);
			}

			else {
				if (alphaDetector.IsAlpha)
				{
					if (!alphaEntered)
					{
						// signal meditation ON state
						//PlaySound(1);
						alphaEntered = true;
					}

					//TODO: sounds and improve detection
					if (alphaDetector.IsActiveMeditation)
					{
						grid_layout.SetBackgroundColor(Android.Graphics.Color.Green);
					}
					else {
						grid_layout.SetBackgroundColor(Android.Graphics.Color.Blue);
					}

				}
				else {
					grid_layout.SetBackgroundColor(Android.Graphics.Color.Black);

					if (alphaEntered)
					{
						// signal meditation OFF state
						//PlaySound(2);
						alphaEntered = false;
					}

				}
			}
		}

		void DisplaySignals()
		{
			var tistr = new TimeSpan (0, 0, eegSignals.elapsedSeconds).ToString ("g");
			if (eegSignals.powerSignals.Count < 60) {
				tistr += "(" + eegSignals.powerSignals.Count + ")";
			}
			tv_ps.Text = tistr;


			tv_meditation.Text = eegSignals.meditSignals.GetDisplayValue();
			tv_attention.Text = eegSignals.attentSignals.GetDisplayValue();
			//tv_totalPower.Text = eegSignals.powerSignals.GetDisplayValue();
			tv_totalPower.Text = eegSignals.totalPowerNoDelta.GetDisplayValue();


			//tv_delta.Text =""+ eegSignals.powerSignals.Count;

			tv_theta.Text = eegSignals.percentAlpha.GetDisplayValue();
			tv_lowalpha.Text = eegSignals.percentBeta.GetDisplayValue();
			tv_highalpha.Text = eegSignals.percentDelta.GetDisplayValue();
			tv_lowbeta.Text = eegSignals.percentTheta.GetDisplayValue ();
			tv_highbeta.Text = eegSignals.percentGamma.GetDisplayValue ();

            //tv_lowgamma.Text = eegSignals.totalPower.GetDisplayValue();

            UpdateChart();
		}

        Queue<double> signal1List = new Queue<double>();
        Queue<double> signal2List = new Queue<double>();

        private void UpdateChart() {
            if(chartView != null) {
                var sg = eegSignals.percentAlpha.Average / 50.0f;
                if(sg > 1) sg = 1;
                signal1List.Enqueue(sg);
                signal2List.Enqueue(eegSignals.percentDelta.Average / 100.0f);
                if(signal1List.Count > 30) {
                    signal1List.Dequeue();
                    signal2List.Dequeue();
                }

                if(signal1List.Count >3) {
                    List<double[]> newList = new List<double[]>();
                    List<double[]> newList2 = new List<double[]>();
                    var sl = signal1List.ToList();
                    var sl2 = signal2List.ToList();
                    var max = (double)signal1List.Count - 1.0f;
                    for(int i=0;i< signal1List.Count;i++) {
                        newList.Add(new double[]  { i / max, 1 - (sl[i] ) } );
                        newList2.Add(new double[] { i / max, 1 - (sl2[i] ) } );
                    }

                    chartView.Values = newList;
                    chartView.Values2 = newList2;
                    chartView.Invalidate();
                }

            }
        }


        // --------------------------------------

        void TickForProgram()
		{
			bool t = false;
			if ( (eegSignals.elapsedSeconds %  20) == 1)
			{
				t = true;
			}

			// also play at startup, to set the volume right
			if (t )
			{
				//PlaySound(0);
			}

		}

		void MonitorAndDisplaySignals()
		{
			// monitor bursts and high delta, theta and high/low power
			string monitorOutput = "";
			if (eegSignals.IsFull)
			{
				// TODO: play sound if alert changes, without flipflop
				if (eegSignals.totalPowerNoDelta.Average > 120)
				{
					monitorOutput += "hightotpow ";
				}
				if (eegSignals.totalPowerNoDelta.Average < 10)
				{
					monitorOutput += "lowtotpow ";
				}
				if (eegSignals.percentTheta.Average > 20)
				{
					monitorOutput += "hightheta ";
				}
				tv_lowgamma.Text = monitorOutput;
				if(monitorOutput != "") {
					tv_lowgamma.SetBackgroundColor(Android.Graphics.Color.DarkOrchid);
				}
				else {
					tv_lowgamma.SetBackgroundColor(Android.Graphics.Color.Black);
				}
			}
		}

		// --------------------------------------
		SoundPool soundPool;
		List<int> sounds = new 	List<int>();

		void SetupSounds()
		{

			// Set the hardware buttons to control the music
			this.VolumeControlStream = Android.Media.Stream.Music;

			// Load the sound
			soundPool = new SoundPool(10, Android.Media.Stream.Music, 0);
			sounds.Add(soundPool.Load(this, Resource.Raw.atype, 1));
			sounds.Add(soundPool.Load(this, Resource.Raw.bonline, 1));
			sounds.Add(soundPool.Load(this, Resource.Raw.boffline, 1));
		}

		private void PlaySound(int index)
		{
			//disabled now

			var audioManager = (AudioManager)GetSystemService(AudioService);
			var actualVolume = (float)audioManager.GetStreamVolume(Android.Media.Stream.Music);
			var maxVolume = (float)audioManager.GetStreamMaxVolume(Android.Media.Stream.Music);
			var volume = actualVolume / maxVolume;

			soundPool.Play(sounds[index], volume, volume, 1, 0, 1f);

		}


		// --------------------------------------

		class TgStreamHandlerImplementation : Java.Lang.Object, ITgStreamHandler
		{

			public void OnStatesChanged(int connectionStates)
			{
				Log.Debug(TAG, "connectionStates change to: " + connectionStates);

                switch (connectionStates)
				{
					case ConnectionStates.StateConnected:
						//sensoResources.start();
                        Instance.AddLogInUiThread("connectionStates change to: StateConnected");
						Instance.showToast("Connected", ToastLength.Short);
                        break;

					case ConnectionStates.StateComplete:
                        Instance.AddLogInUiThread("connectionStates change to: StateComplete");
                        //read file complete
                        break;
					case ConnectionStates.StateWorking:
						//byte[] cmd = new byte[1];
						//cmd[0] = 's';
						//tgStreamReader.sendCommandtoDevice(cmd);
						Instance.linkHandler.SendEmptyMessageDelayed(1234, 5000);
						break;
					case ConnectionStates.StateGetDataTimeOut:
						Log.Debug(TAG, "Timeout, reconnecting");
                        Instance.AddLogInUiThread("connectionStates change to: StateGetDataTimeOut");
                        //Instance.showToast("Timeout", ToastLength.Short);
                        Instance.Reconnect();
                        //  get data time out
                        break;
					case ConnectionStates.StateError:
						Log.Debug(TAG, "Connect error, Please try again!");
                        Instance.AddLogInUiThread("connectionStates change to: StateError");
						break;
                    case ConnectionStates.StateFailed:
						Log.Debug(TAG, "Connect failed, Please try again!");
                        Instance.AddLogInUiThread("connectionStates change to: StateFailed");
                        Instance.Reconnect();
                        break;

                    case ConnectionStates.StateStopped:
                        Instance.AddLogInUiThread("connectionStates change to: StateStopped");
						break;
                    case ConnectionStates.StateDisconnected:
                        Instance.AddLogInUiThread("connectionStates change to: StateDisconnected");
						break;

                }
				Message msg = Instance.linkHandler.ObtainMessage();
				msg.What = MSG_UPDATE_STATE;
				msg.Arg1 = connectionStates;
				Instance.linkHandler.SendMessage(msg);

			}

			public void OnRecordFail(int a)
			{
				Log.Error(TAG, "onRecordFail: " + a);

			}

			public void OnChecksumFail(byte[] payload, int length, int checksum)
			{
				Instance.badPacketCount++;
				Message msg = Instance.linkHandler.ObtainMessage();
				msg.What = MSG_UPDATE_BAD_PACKET;
				msg.Arg1 = Instance.badPacketCount;
				Instance.linkHandler.SendMessage(msg);
			}

			public void OnDataReceived(int datatype, int data, Java.Lang.Object obj)
			{
				Message msg = Instance.linkHandler.ObtainMessage();
				msg.What = datatype;
				msg.Arg1 = data;
				msg.Obj = obj;
				Instance.linkHandler.SendMessage(msg);
				//Log.Info(TAG, "onDataReceived");
			}

		}


		private static int MSG_UPDATE_BAD_PACKET = 1001;
		private static int MSG_UPDATE_STATE = 1002;
		private static int MSG_CONNECT = 1003;
		private bool isReadFilter = false;

		class LinkDetectedHandler : Handler
		{

			public override void HandleMessage(Message msg)
			{

				switch (msg.What)
				{
					case 1234:
						Instance.tgStreamReader.MWM15_getFilterType();
						Instance.isReadFilter = true;
						Log.Debug(TAG, "MWM15_getFilterType ");

						break;
					case 1235:
						Instance.tgStreamReader.MWM15_setFilterType(MindDataType.FilterType.Filter60hz);
						Log.Debug(TAG, "MWM15_setFilter  60HZ");
						Instance.linkHandler.SendEmptyMessageDelayed(1237, 1000);
						break;
					case 1236:
						Instance.tgStreamReader.MWM15_setFilterType(MindDataType.FilterType.Filter50hz);
						Log.Debug(TAG, "MWM15_SetFilter 50HZ ");
						Instance.linkHandler.SendEmptyMessageDelayed(1237, 1000);
						break;

					case 1237:
						Instance.tgStreamReader.MWM15_getFilterType();
						Log.Debug(TAG, "MWM15_getFilterType ");

						break;

					case MindDataType.CodeFilterType:
						Log.Debug(TAG, "CODE_FILTER_TYPE: " + msg.Arg1 + "  isReadFilter: " + Instance.isReadFilter);
						if (Instance.isReadFilter)
						{
							Instance.isReadFilter = false;
							if (msg.Arg1 == (int) MindDataType.FilterType.Filter50hz)
							{
								Instance.linkHandler.SendEmptyMessageDelayed(1235, 1000);
							}
							else if (msg.Arg1 == (int)MindDataType.FilterType.Filter60hz)
							{
								Instance.linkHandler.SendEmptyMessageDelayed(1236, 1000);
							}
							else {
								Log.Error(TAG, "Error filter type");
							}
						}

						break;


					case MindDataType.CodeRaw:
						//Instance.updateWaveView(msg.Arg1);
						//You can put the raw data into Algo SDKs here
						break;
					case MindDataType.CodeMeditation:
						//Log.Debug(TAG, "CODE_MEDITATION " + msg.Arg1);
						Instance.eegSignals.AddSignal(0, msg.Arg1, null);
						break;
					case MindDataType.CodeAttention:
						//Log.Debug(TAG, "CODE_ATTENTION " + msg.Arg1);
						Instance.eegSignals.AddSignal(1, msg.Arg1, null);
						break;
					case MindDataType.CodeEegpower:
						EEGPower power = (EEGPower)msg.Obj;

						if (power.IsValidate)
						{
							Instance.eegSignals.AddSignal(2, 0, power);
						}
						break;
					case MindDataType.CodePoorSignal://
						int poorSignal = msg.Arg1;
						//Log.Debug(TAG, "poorSignal:" + poorSignal);
						Instance.eegSignals.SetPoorSignal(msg.Arg1);

						break;
					case 1001: // MSG_UPDATE_BAD_PACKET
						//Instance.tv_badpacket.Text = ("" + msg.Arg1);

						break;
					default:
						break;
				}
				base.HandleMessage(msg);
			}
		}


		public void showToast(String msg, ToastLength timeStyle)
		{
			RunOnUiThread(() => {
				Toast.MakeText(ApplicationContext, msg, timeStyle).Show();
			});

		}


		//show device list while scanning
		//private ListView list_select;
	//	private BTDeviceListAdapter deviceListApapter = null;
		//private Dialog selectDialog;

		// (3) Demo of getting Bluetooth device dynamically
		public void ScanDeviceAndStartMeasurement(bool monitormode)
		{
			isMonitorMode = monitormode;

			//decide

			if (false)
			{

				if (mBluetoothAdapter.IsDiscovering)
				{
					mBluetoothAdapter.CancelDiscovery();
				}

				//setUpDeviceListView();

				//register the receiver for scanning
				//IntentFilter filter = new IntentFilter(BluetoothDevice.ActionFound);
				//this.RegisterReceiver(mReceiver, filter);

				//mBluetoothAdapter.StartDiscovery();

				// kz
				showToast("getting bonded devices...", ToastLength.Short);
				var pairedDevices = mBluetoothAdapter.BondedDevices;

				if (pairedDevices == null && pairedDevices.Count == 0)
				{
					showToast("Sorry, no paired device found.", ToastLength.Short);
				}
				else {
					// kz exta
					showToast("Using: " + pairedDevices.First().Name, ToastLength.Short);
					StartDataStream(pairedDevices.First());
				}

			}
			else {
				//connected and paired device
				showToast("Using connected and paired device ", ToastLength.Short);
				StartDataStream(null);
			}

		}

		public void StartDataStream(BluetoothDevice device)
		{
			BluetoothDevice remoteDevice = null;
			if (device != null)
			{
				// TODO Auto-generated method stub
				//Log.Debug(TAG, "Rico ####  list_select onItemClick     ");
				if (mBluetoothAdapter.IsDiscovering)
				{
					mBluetoothAdapter.CancelDiscovery();
				}
				//unregister receiver
				//Instance.UnregisterReceiver(mReceiver);

				mBluetoothDevice = device; //deviceListApapter.getDevice(deviceIndex);//arg2
										   //selectDialog.Dismiss();
										   //selectDialog = null;

				Log.Debug(TAG, "onItemClick name: " + mBluetoothDevice.Name + " , address: " + mBluetoothDevice.Address);
				address = mBluetoothDevice.Address;

				//ger remote device
				remoteDevice = mBluetoothAdapter.GetRemoteDevice(mBluetoothDevice.Address);

				//bind and connect
				//bindToDevice(remoteDevice); // create bond works unstable on Samsung S5
				//showToast("pairing ...",ToastLength.SHORT);

			}

			showToast("connecting to streamreader now!", ToastLength.Short);

			tgStreamReader = createStreamReaderWithNonConnectedDevice(remoteDevice);
			if (tgStreamReader != null)
			{
				btn_stop.Visibility = ViewStates.Visible;
				btn_selectdevice.Visibility = ViewStates.Gone;
				btn_monitor.Visibility = ViewStates.Gone;

				StartSignalTimer();

				tgStreamReader.ConnectAndStart();
			}
			else {
				showToast("Connecting to streamreader FAILED!", ToastLength.Long);
			}

		}

        public void Reconnect() {

            if(tgStreamReader != null) {
                // first close and stop old reader, then create new
                tgStreamReader.Stop();
                tgStreamReader.Close();

                tgStreamReader = createStreamReaderWithNonConnectedDevice(null);
                if(tgStreamReader != null) {
                    tgStreamReader.ConnectAndStart();
                }

                //tgStreamReader.Start();
            }
        }

        public TgStreamReader createStreamReaderWithNonConnectedDevice(BluetoothDevice bd)
		{
            TgStreamReader newStreamReader = null;

            if (bd != null)
			{
                newStreamReader = new TgStreamReader(bd, callback);
			}
			else {
                newStreamReader = new TgStreamReader(mBluetoothAdapter, callback);
			}
			if (newStreamReader != null)
			{
                newStreamReader.SetGetDataTimeOutTime(5); // seconds till timeout - was 10
				//tgStreamReader.StartLog(); // maybe log causes exceptionts in native c thread
			}
			return newStreamReader;
		}

		/*
		public void bindToDevice(BluetoothDevice bd)
		{
			int ispaired = 0;
			if (bd.BondState != Bond.Bonded)
			{
				//ispaired = remoteDevice.createBond();
				try
				{
					//Set pin
					if (Utils.autoBond(bd.getClass(), bd, "0000"))
					{
						ispaired += 1;
					}
					//bind to device
					if (Utils.createBond(bd.getClass(), bd))
					{
						ispaired += 2;
					}
					var createCancelMethod = BluetoothDevice.Class.getMethod("cancelBondProcess");
					var boolx = (bool)createCancelMethod.Invoke(bd);
					Log.Debug(TAG, "bool=" + boolx);

				}
				catch (Exception e)
				{
					// TODO Auto-generated catch block
					Log.Debug(TAG, " paire device Exception:    " + e.ToString());
				}
			}
			Log.Debug(TAG, " ispaired:    " + ispaired);

		}
		*/

		/*
		private MyBroadcastReceiver mReceiver = new MyBroadcastReceiver();
		class MyBroadcastReceiver : BroadcastReceiver
		{

			public override void OnReceive(Context context, Intent intent)
			{
				var action = intent.Action;
				Log.Debug(TAG, "mReceiver()");
				// When discovery finds a device
				if (BluetoothDevice.ActionFound == action)
				{
					// Get the BluetoothDevice object from the Intent
					BluetoothDevice device = intent.GetParcelableExtra(BluetoothDevice.ExtraDevice) as BluetoothDevice;
					Log.Debug(TAG, "mReceiver found device: " + device.Name);

					// update to UI
					//Instance.deviceListApapter.addDevice(device);
				//Instance.deviceListApapter.notifyDataSetChanged();

			} 
		}
	}
		*/
    
	}

}
