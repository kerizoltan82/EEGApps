using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using System;
using Android.Content;
using Android.Util;
using Android;
using Android.Content.PM;

namespace gnf
{
	[Activity(Label = "MindSet EEG Helper", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += ButtonClicked;
			//Com.Neurosky.AlgoSdk >;
			//Com.Neurosky.Connection.TgStreamReader.RedirectConsoleLogToDocumentFolder();

			CheckBluetooth();

            HandleRequestPermission();

            /*
var nskAlgoSdk = new NskAlgoSdk();

try
{
	// (1) Make sure that the device supports Bluetooth and Bluetooth is on
	var mBluetoothAdapter = BluetoothAdapteResources.DefaultAdapter;
	if (mBluetoothAdapter == null || !mBluetoothAdapteResources.IsEnabled)
	{

		//finish();
	}
}
catch (Exception e)
{
	//e.printStackTrace();
	//Log.i(TAG, "error:" + e.getMessage());
	return;
}
*/


        }

        void HandleRequestPermission() {
            if(Build.VERSION.SdkInt >= BuildVersionCodes.M) {
                string[] allPermissionsLocation =
                {
                  Manifest.Permission.WriteExternalStorage,
                  Manifest.Permission.ReadExternalStorage,
                  Manifest.Permission.Bluetooth,
                  Manifest.Permission.AccessFineLocation,
                  Manifest.Permission.AccessCoarseLocation
                };

                if(this.CheckSelfPermission(allPermissionsLocation[3]) != (int)Permission.Granted) {
                    RequestPermissions(allPermissionsLocation, RequestLocationId);
                }
                else {
                }
            }
            else {
            }
        }


        const int RequestLocationId = 0;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults) {
            if(requestCode == RequestLocationId) {
               
            }
        }


        void ButtonClicked(Object sender, EventArgs e)
		{
			//Log.d(TAG, "lib version: " + TgStreamReadeResources.getVersion());
			//var bb = new AlertDialog();

			// Show filedemoactivity
			//Intent intent = new Intent(this, typeof(FileDemoActivity));

			Intent intent = new Intent(this, typeof(BluetoothDeviceDemoActivity));
			Log.Debug("EEGLib", "Start the BluetoothDeviceDemoActivity");
			this.StartActivity(intent);
		}

		void CheckBluetooth()
		{
			try
			{
				var mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
				if (mBluetoothAdapter == null || !mBluetoothAdapter.IsEnabled)
				{
					Toast.MakeText(
							this,
							"Please enable your Bluetooth and re-run this program !",
							ToastLength.Long).Show();
					//Finish();
				}
			}
			catch (Exception e)
			{
				//e.printStackTrace();
				Log.Info("EEGLibApp", "error:" + e.Message);
				return;
			}
		}

		/*
		private NskAlgoSdk nskAlgoSdk;

		private int bLastOutputInterval = 1;
		private NskAlgoType currentSelectedAlgo;

		// canned data variables
		private short[] raw_data = { 0 };
		private int raw_data_index = 0;
		private float[] output_data;
		private int output_data_count = 0;
		private int raw_data_sec_len = 85;
		// COMM SDK handles
		private TgStreamReader tgStreamReader;
		private BluetoothAdapter mBluetoothAdapter;


		void ButtonClicked(Object sender, EventArgs e)
		{
			output_data_count = 0;
			output_data = null;

			raw_data = new short[512];
			raw_data_index = 0;

			// Example of constructor public TgStreamReader(BluetoothAdapter ba, TgStreamHandler tgStreamHandler)
			tgStreamReader = new TgStreamReader(mBluetoothAdapter, callback);

			if (tgStreamReader != null && tgStreamReadeResources.IsBTConnected)
			{

				// Prepare for connecting
				tgStreamReadeResources.Stop();
				tgStreamReadeResources.Close();
			}

			// (4) Demo of  using connect() and start() to replace connectAndStart(),
			// please call start() when the state is changed to STATE_CONNECTED
			tgStreamReadeResources.Connect();


		}
		*/

		protected override void OnDestroy()
		{
			base.OnDestroy();
			//Com.Neurosky.Connection.TgStreamReader.StopConsoleLog();
		}


	}
}


