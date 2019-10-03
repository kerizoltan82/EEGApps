

using System;
using Android.Views;
using Android.App;
using Android.Widget;
using Com.Neurosky.Connection;
using Com.Neurosky.Connection.DataType;
using Android.OS;
using Java.IO;
using Android.Util;

/**
 * This activity demonstrates how to use the constructor:
 * TgStreamReader(InputStream is, TgStreamHandler tgStreamHandler)
 * and related functions:
 * (1) setReadFileBlockSize
 * (2) setReadFileDelay
 * (3) How to destroy a TgStreamReader object
 * (4) ConnectionStates.STATE_COMPLETE is state that indicates read file to the end
 *
 */

namespace gnf
{
	[Activity(Label = "FileDemoActivity", Icon = "@mipmap/icon")]
	//Register[""]
	public class FileDemoActivity : Activity
	{

		private static String TAG = "FileDemoActivity";//.class.getSimpleName();

		private static int MSG_UPDATE_BAD_PACKET = 1001;
		private static int MSG_UPDATE_STATE = 1002;


		private TextView tv_ps = null;
		private TextView tv_attention = null;
		private TextView tv_meditation = null;
		private TextView tv_delta = null;
		private TextView tv_theta = null;
		private TextView tv_lowalpha = null;

		private TextView tv_highalpha = null;
		private TextView tv_lowbeta = null;
		private TextView tv_highbeta = null;

		private TextView tv_lowgamma = null;
		private TextView tv_middlegamma = null;
		private TextView tv_badpacket = null;

		private Button btn_start = null;
		private Button btn_stop = null;
		private LinearLayout wave_layout;

		private int badPacketCount = 0;

		private TgStreamReader tgStreamReader;
		LinkDetectedHandler linkHandler = new LinkDetectedHandler();
		static FileDemoActivity Instance;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			LinkDetectedHandler.activ = this;
			Instance = this;

			//requestWindowFeature(Window.FEATURE_NO_TITLE);
			//getWindow().addFlags(WindowManageResources.LayoutParams.FLAG_KEEP_SCREEN_ON);
			SetContentView(Resource.Layout.first_view);

			initView();
			setUpDrawWaveView();
		}

		private void initView()
		{
			tv_ps = (TextView)FindViewById(Resource.Id.tv_ps);
			tv_attention = (TextView)FindViewById(Resource.Id.tv_attention);
			tv_meditation = (TextView)FindViewById(Resource.Id.tv_meditation);
			tv_delta = (TextView)FindViewById(Resource.Id.tv_delta);
			tv_theta = (TextView)FindViewById(Resource.Id.tv_theta);
			tv_lowalpha = (TextView)FindViewById(Resource.Id.tv_lowalpha);

			tv_highalpha = (TextView)FindViewById(Resource.Id.tv_highalpha);
			tv_lowbeta = (TextView)FindViewById(Resource.Id.tv_lowbeta);
			tv_highbeta = (TextView)FindViewById(Resource.Id.tv_highbeta);

			tv_lowgamma = (TextView)FindViewById(Resource.Id.tv_lowgamma);
			tv_middlegamma = (TextView)FindViewById(Resource.Id.tv_middlegamma);
			tv_badpacket = (TextView)FindViewById(Resource.Id.tv_badpacket);


			btn_start = (Button)FindViewById(Resource.Id.btn_start);
			btn_stop = (Button)FindViewById(Resource.Id.btn_stop);
			wave_layout = (LinearLayout)FindViewById(Resource.Id.wave_layout);

			btn_start.Click += (sender, e) =>
			{
				badPacketCount = 0;

				// (3) How to destroy a TgStreamReader object
				if (tgStreamReader != null)
				{
					tgStreamReader.Stop();
					tgStreamReader.Close();
					tgStreamReader = null;
				}
				var iss = ApplicationContext.Resources.OpenRawResource(Resource.Raw.tgam_capture);
				// Example of TgStreamReader(InputStream is, TgStreamHandler tgStreamHandler)
				var callback = new TgStreamHandlerImplementation();
				tgStreamReader = new TgStreamReader(iss, callback);

				// (1) Example of setReadFileBlockSize(int), the default block size is 8, call it before connectAndStart() or connect()
				tgStreamReader.SetReadFileBlockSize(16);
				// (2) Example of setReadFileDelay(int), the default delay time is 2ms, call it before connectAndStart() or connect()
				tgStreamReader.SetReadFileDelay(2);

				tgStreamReader.ConnectAndStart();

			};

			btn_stop.Click += (sender, e) =>
			{

				// TODO Auto-generated method stub
				stop();

			};

		}

		public void stop()
		{
			if (tgStreamReader != null)
			{
				tgStreamReader.Stop();
				tgStreamReader.Close();
			}
		}

		protected override void OnDestroy()
		{
			// TODO Auto-generated method stub
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
			// TODO Auto-generated method stub
			base.OnStop();
			stop();
		}

		// TODO view
		//DrawWaveView waveView = null;
		View waveView = null;

		public void setUpDrawWaveView()
		{
			// TODO use self view to drawing ECG

			/*
			waveView = new DrawWaveView(this.ApplicationContext);
			wave_layout.AddView(waveView, new ViewGroup.LayoutParams(
					ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
			waveView.setValue(2048, 2048, -2048);
			*/
		}

		public void updateWaveView(int data)
		{
			if (waveView != null)
			{
				//waveView.updateData(data);
			}

		}

		//ITgStreamHandler callback;

		class TgStreamHandlerImplementation : Java.Lang.Object, ITgStreamHandler
		{

			public  void OnStatesChanged(int connectionStates)
			{
				// TODO Auto-generated method stub
				Log.Debug(TAG, "connectionStates change to: " + connectionStates);
				switch (connectionStates)
				{
					case ConnectionStates.StateConnected:
						//sensoResources.start();
						Instance.showToast("Connected", ToastLength.Short);
						break;
					case ConnectionStates.StateWorking:

						break;
					case ConnectionStates.StateGetDataTimeOut:
						//  get data time out
						break;
					case ConnectionStates.StateComplete:
						//read file complete
						Instance.showToast("STATE_COMPLETE", ToastLength.Short);
						break;
					case ConnectionStates.StateStopped:
						break;
					case ConnectionStates.StateDisconnected:
						break;
					case ConnectionStates.StateError:
						break;
				}
				Message msg = Instance.linkHandler.ObtainMessage();
				msg.What = MSG_UPDATE_STATE;
				msg.Arg1 = connectionStates;
				Instance.linkHandler.SendMessage(msg);

			}

			public  void OnRecordFail(int a)
			{
				// TODO Auto-generated method stub
				Log.Error(TAG,"onRecordFail: " +a);

			}

			public void OnChecksumFail(byte[] payload, int length, int checksum)
			{
				// TODO Auto-generated method stub

				Instance.badPacketCount++;
				Message msg = Instance.linkHandler.ObtainMessage();
				msg.What = MSG_UPDATE_BAD_PACKET;
				msg.Arg1 = Instance.badPacketCount;
				Instance.linkHandler.SendMessage(msg);

			}

			public void OnDataReceived(int datatype, int data, Java.Lang.Object obj)
			{
				// TODO Auto-generated method stub
				Message msg = Instance.linkHandler.ObtainMessage();
				msg.What = datatype;
				msg.Arg1 = data;
				msg.Obj = obj;
				Instance.linkHandler.SendMessage(msg);
				Log.Info(TAG,"onDataReceived");
			}

		}

        
		class LinkDetectedHandler : Handler //LinkDetectedHandler = new Handler()
		{
			public static FileDemoActivity activ;

		public override void HandleMessage(Message msg)
		{

			switch (msg.What)
			{
				case MindDataType.CodeRaw:
					activ.updateWaveView(msg.Arg1);
					//You can put the raw data into Algo SDKs here
					break;
				case MindDataType.CodeMeditation:
					Log.Debug(TAG, "HeadDataType.CODE_MEDITATION " + msg.Arg1);
					activ.tv_meditation.Text = ("" + msg.Arg1);
					break;
				case MindDataType.CodeAttention:
					Log.Debug(TAG, "CODE_ATTENTION " + msg.Arg1);
					activ.tv_attention.Text = ("" + msg.Arg1);
					break;
				case MindDataType.CodeEegpower:
					EEGPower power = (EEGPower)msg.Obj;
					if (power.IsValidate)
					{
						activ.tv_delta.Text = ("" + power.Delta);
						activ.tv_theta.Text = ("" + power.Theta);
						activ.tv_lowalpha.Text = ("" + power.LowAlpha);
						activ.tv_highalpha.Text = ("" + power.HighAlpha);
						activ.tv_lowbeta.Text = ("" + power.LowBeta);
						activ.tv_highbeta.Text = ("" + power.HighBeta);
						activ.tv_lowgamma.Text = ("" + power.LowGamma);
						activ.tv_middlegamma.Text = ("" + power.MiddleGamma);
					}
					break;
				case MindDataType.CodePoorSignal://
					int poorSignal = msg.Arg1;
					Log.Debug(TAG, "poorSignal:" + poorSignal);
					activ.tv_ps.Text = ("" + msg.Arg1);

					break;
				case 1001: // MSG_UPDATE_BAD_PACKET
					activ.tv_badpacket.Text = ("" + msg.Arg1);

					break;
				default:
					break;
			}
			base.HandleMessage(msg);
		}
	}


	public void showToast(String msg, ToastLength timeStyle)
	{
			Toast.MakeText(ApplicationContext, msg, timeStyle).Show();
			/*
		FileDemoActivity.this.runOnUiThread(new Runnable()
		{

			public void run()    
            {    
            	Toast.MakeText(ApplicationContext, msg, timeStyle).show();
            }    
    
        }); 
        */
	}
}

}