using System;
using System.IO;
using Android.App;
using Android.Content;
using Com.Neurosky.Connection;

namespace gnf
{
	class EEGSignals
	{
		public EEGSignals()
		{
		}

        public bool ProcessAlsoIfSignalIsPoor = false;

		public FilteredIntSignal percentAlpha = new FilteredIntSignal();
		public FilteredIntSignal percentBeta = new FilteredIntSignal();
		public FilteredIntSignal percentDelta = new FilteredIntSignal();
		public FilteredIntSignal percentGamma = new FilteredIntSignal ();
		public FilteredIntSignal percentTheta = new FilteredIntSignal ();
		public FilteredDoubleSignal totalPowerNoDelta = new FilteredDoubleSignal ();

		public FilteredIntSignal meditSignals = new FilteredIntSignal();
		public FilteredIntSignal attentSignals = new FilteredIntSignal();
		public FilteredPowerSignal powerSignals = new FilteredPowerSignal();

		public void Start()
		{
			elapsedSeconds = 0;
			ClearSignals();
		}

		public void Close()
		{
		}

		void ClearSignals()
		{
			meditSignals.ClearAll();
			attentSignals.ClearAll();
			powerSignals.ClearAll();
			percentAlpha.ClearAll();
			percentBeta.ClearAll();
			percentDelta.ClearAll();
			percentGamma.ClearAll ();
			percentTheta.ClearAll ();
			totalPowerNoDelta.ClearAll();
		}

		public bool IsPoorSignal = false;

		public void SetPoorSignal(int poor)
		{
			IsPoorSignal = poor != 0;
		}


		public void AddSignal(int signalType, int rawValue, EEGPower power)
		{
			// do not process if bad signal is there
			if (!IsPoorSignal || ProcessAlsoIfSignalIsPoor)
			{

			    if (signalType == 0)
			    {
				    meditSignals.AddSignal(rawValue);
			    }
			    else if (signalType == 1)
			    {
				    attentSignals.AddSignal(rawValue);
			    }
			    else if (signalType == 2)
			    {
				    powerSignals.AddSignal(power);
				    int totalPowerSample = power.GetTotalPower();
                    if(totalPowerSample > 0) {
                        percentAlpha.AddSignal((int)(((power.HighAlpha + power.LowAlpha) * 100) / totalPowerSample));
                        percentBeta.AddSignal((int)(((power.HighBeta + power.LowBeta) * 100) / totalPowerSample));
                        percentDelta.AddSignal((int)(((power.Delta) * 100) / totalPowerSample));
                        percentTheta.AddSignal((int)(((power.Theta) * 100) / totalPowerSample));
                        percentGamma.AddSignal((int)(((power.LowGamma + power.MiddleGamma) * 100) / totalPowerSample));

                        totalPowerNoDelta.AddSignal(power.GetTotalPowerWithoutDelta() / 10000f);
                    }
				   
			    }

            }

        }

        public int IsEvent { get; private set; } = 0;

		public bool IsFull { 
			get {
				//could use any other.
				return powerSignals.IsFull;
			} 
		}


		int rawClickEvent = 0;
		public void AddEvent()
		{
			if (rawClickEvent < 2)
			{
				rawClickEvent++;
			}
		}

		public void Tick()
		{
			elapsedSeconds++;
			IsEvent = rawClickEvent;
			rawClickEvent = 0;
		}

		public int elapsedSeconds = 0;


	}
}

