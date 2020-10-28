using System;
using System;
using System.IO;
using Android.App;
using Android.Content;
using Com.Neurosky.Connection;


namespace gnf
{
	class SignalLogger
	{
		Activity refActivity;
		string currentFileName = "";
		EEGSignals signals;
		AlphaDetector detector;
        public int TotalNumSignals = 0;


        public SignalLogger(Activity refActivity, EEGSignals signals, AlphaDetector detector)
		{
			this.refActivity = refActivity;
			this.signals = signals; 
			this.detector = detector;
		}

		public void Close()
		{
			streamWriter.Dispose();
		}

		public void Start()
		{
			var istr = GetString("lognum");
			istr = Convert.ToString(Convert.ToInt32(istr) + 1);
			SaveString("lognum", istr);

            // TODO log into folder, not "root"
			string path = Android.OS.Environment.ExternalStorageDirectory.Path;
			//currentFileName = Path.Combine(path, "mindset_eeg_log_" + istr + ".csv");
			currentFileName = Path.Combine(path, "mindset_eeg_log" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm")+ "_" + istr + ".csv");
            streamWriter = new StreamWriter(currentFileName, true);

			//write header
			//var timStr = DateTime.Now.ToString("HH:mm:ss");
			var timStr = "Start of recording: " + DateTime.Now.ToString("u");

			var str = "Time,Poor,Event,Alpha,Medit,Attent,Delta,HiAlpha,HiBeta,LowAlpha,LowBeta,LowGamma,MidGamma,Theta,PAlpha,PBeta,PDelta,PGamma,PTheta,TotalpowNoDeltaD10K";
			try
			{
				streamWriter.WriteLine(timStr);
				streamWriter.WriteLine(str);
			}
			catch (Exception e)
			{

			}

		}

		StreamWriter streamWriter;
		public void LogSignals()
		{
			var timStr = new TimeSpan(0, 0, signals.elapsedSeconds).ToString("c");

            var str = timStr + "," +
                (signals.IsPoorSignal ? "100" : "0") + "," +
                (signals.IsEvent * 50) + "," +
                (detector.GetDetectorSignalForLog()) + "," +
                signals.meditSignals.GetStringValueForLog() + "," +
                signals.attentSignals.GetStringValueForLog() + "," +
                signals.powerSignals.GetStringValueForLog() + "," +
                       signals.percentAlpha.GetStringValueForLog() + "," +
                       signals.percentBeta.GetStringValueForLog() + "," +
                       signals.percentDelta.GetStringValueForLog() + "," +
                       signals.percentGamma.GetStringValueForLog() + "," +
                       signals.percentTheta.GetStringValueForLog() + "," +
                       signals.totalPowerNoDelta.GetStringValueForLog();// + "," +
                       //TotalNumSignals;

            // replace for excel/openoffice
            //str = str.Replace (".", ",");

            try
			{
				streamWriter.WriteLine(str);
			}
			catch (Exception )
			{

			}

			//event was logged, now we can turn it off again
		}



		// --------------------------------------

		public string GetString(string key)
		{
			var prefs = refActivity.GetSharedPreferences(refActivity.PackageName, FileCreationMode.Private);
			return prefs.GetString(key, "1");
		}

		public void SaveString(string key, string value)
		{
			var prefs = refActivity.GetSharedPreferences(refActivity.PackageName, FileCreationMode.Private);
			var prefEditor = prefs.Edit();
			prefEditor.PutString(key, value);
			prefEditor.Commit();
		}
	}
}

