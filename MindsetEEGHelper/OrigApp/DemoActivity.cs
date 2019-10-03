/*

using com.neurosky.connection.TgStreamReader;

using android.app.Activity;
using android.content.Intent;
using android.os.Bundle;
using android.util.Log;
using android.view.View;
using android.view.View.OnClickListener;
using android.view.Window;
using android.view.WindowManager;
using android.widget.Button;
using android.widget.TextView;
using android.widget.Toast;


public class DemoActivity extends Activity {
	private static String TAG = DemoActivity.class.getSimpleName();
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		supeResources.onCreate(savedInstanceState);
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		getWindow().addFlags(WindowManageResources.LayoutParams.FLAG_KEEP_SCREEN_ON);
		setContentView(Resources.layout.main_view);

		initView();
		// (1) Example of redirectConsoleLogToDocumentFolder()
		// Call redirectConsoleLogToDocumentFolder at the beginning of the app, it will record all the log.
		// Don't forget to call stopConsoleLog() in onDestroy() if it is the end point of this app.
		// If you can't find the end point of the app , you don't have to call stopConsoleLog()
		TgStreamReadeResources.redirectConsoleLogToDocumentFolder();
		// (3) demo of getVersion
		Log.d(TAG,"lib version: " + TgStreamReadeResources.getVersion());
	}

	private TextView tv_filedemo = null;
	private TextView  tv_adapter = null;
	private TextView  tv_device = null;
	//private TextView  tv_uart = null;
	
	private Button btn_filedemo = null;
	private Button btn_adapter = null;
	private Button btn_device = null;
	//private Button btn_uart = null;


	private void initView() {
		tv_filedemo = (TextView) FindViewById(Resources.Id.tv_filedemo);
		tv_adapter = (TextView) FindViewById(Resources.Id.tv_adapter);
		tv_device = (TextView) FindViewById(Resources.Id.tv_device);
		//tv_uart = (TextView) FindViewById(Resources.Id.tv_uart);

		btn_filedemo = (Button) FindViewById(Resources.Id.btn_filedemo);
		btn_adapter = (Button) FindViewById(Resources.Id.btn_adapter);
		btn_device = (Button) FindViewById(Resources.Id.btn_device);
		//btn_uart = (Button) FindViewById(Resources.Id.btn_uart);
		
		btn_filedemo.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				Intent intent = new Intent(DemoActivity.this,FileDemoActivity.class);
				Log.d(TAG,"Start the FileDemoActivity");
				startActivity(intent);
			}
		});

		
		btn_adapteResources.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				Intent intent = new Intent(DemoActivity.this,BluetoothAdapterDemoActivity.class);
				Log.d(TAG,"Start the BluetoothAdapterDemoActivity");
				startActivity(intent);
			}
		});
		btn_device.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View arg0) {
				Intent intent = new Intent(DemoActivity.this,BluetoothDeviceDemoActivity.class);
				Log.d(TAG,"Start the BluetoothDeviceDemoActivity");
				startActivity(intent);
			}
		});
//		btn_uart.setOnClickListener(new OnClickListener() {
//
//			@Override
//			public void onClick(View arg0) {
//				Intent intent = new Intent(DemoActivity.this,UARTDemoActivity.class);
//				startActivity(intent);
//			}
//		});
	}



	@Override
	protected void onDestroy() {
		
		// (2) Example of stopConsoleLog()
		TgStreamReadeResources.stopConsoleLog();
		supeResources.onDestroy();
	}

	@Override
	protected void onStart() {
		supeResources.onStart();
	}

	@Override
	protected void onStop() {
		// TODO Auto-generated method stub
		supeResources.onStop();
	}


}
*/