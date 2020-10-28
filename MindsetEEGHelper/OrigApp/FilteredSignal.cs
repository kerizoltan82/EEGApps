using System;
using System.Collections.Generic;
using System.Linq;
using Com.Neurosky.Connection;

namespace gnf
{
	
	

	class FilteredPowerSignal : FilteredSignal<EEGPower>
	{
		
		//using last sample here
		public string GetStringValueForLog()
		{
			if (this.Count == 0 || Average == null)
			{
				return "0,0,0,0,0,0,0,0";
			}
			else {
				return LastSample.ToCommaString();
			}
		}

		public string GetDisplayValue()
		{
			if (this.Count == 0 || Average == null)
			{
				return "no samples";
			}
			else {
				return LastSample.ToCommaString() ;//+ " (" + this.Count + ")";
			}
		}

		// to avoid oncucrrent exception ni list access
		public EEGPower Average
		{
			get;
			private set;
		}

		public override void AddSignal(EEGPower rawValue)
		{
			// do scaling before anything else
			// disabled when all percentages are there
			//ScalePower(rawValue);

			base.AddSignal(rawValue);
			Average = GetAverage();
		}


		EEGPower GetAverage()
		{
			var ret = new EEGPower(new byte[] { 0 } ,0 ,0);
			ret.Delta = (int) this.Average(ep => ep.Delta);
			ret.HighAlpha = (int)this.Average(ep => ep.HighAlpha);
			ret.HighBeta = (int)this.Average(ep => ep.HighBeta);
			ret.LowAlpha = (int)this.Average(ep => ep.LowAlpha);
			ret.LowBeta = (int)this.Average(ep => ep.LowBeta);
			ret.LowGamma = (int)this.Average(ep => ep.LowGamma);
			ret.MiddleGamma = (int)this.Average(ep => ep.MiddleGamma );
			ret.Theta = (int)this.Average(ep => ep.Theta);
			return ret;
		}
        /*
		public static void ScalePower(EEGPower power)
		{
			power.Delta = (int) power.Delta / 10;
			power.MiddleGamma = (int)power.MiddleGamma * 4;
			power.Theta = (int)power.Theta / 3;
		}
        */
	}

	class FilteredDoubleSignal : FilteredSignal<double>
	{

		//using average here
		public string GetStringValueForLog ()
		{
			if (this.Count == 0) {
				return "0";
			} else {
				return Average.ToString ("F2");
			}
		}

		public string GetDisplayValue ()
		{
			if (this.Count == 0) {
				return "no samples";
			} else {
				return Average.ToString ("F2");// + " (" + this.Count + ")";
			}
		}

		// to avoid oncucrrent exception ni list access
		public override void AddSignal (double rawValue)
		{
			base.AddSignal (rawValue);
			Average = this.Average ();
		}

		public double Average { get; private set; } = 0f;
	}


	class FilteredIntSignal : FilteredSignal<int>
	{

		//using average here
		public string GetStringValueForLog()
		{
			if (this.Count == 0)
			{
				return "0";
			}
			else {
				return Average.ToString("F2");
			}
		}

		public string GetDisplayValue()
		{
			if (this.Count == 0)
			{
				return "no samples";
			}
			else {
				return Average.ToString ("F2");// + " (" + this.Count + ")";
			}
		}

		// to avoid oncucrrent exception ni list access
		public override void AddSignal(int rawValue)
		{
			base.AddSignal(rawValue);
			Average = this.Average();
		}

		public double Average { get; private set; } = 0f;
	}


	class FilteredSignal<T> : List<T>
	{

		public static int filterSamplesCount = 60;

		public virtual void AddSignal(T rawValue)
		{
			Add(rawValue);
			LastSample = rawValue;
			if (Count > filterSamplesCount)
			{
				RemoveAt(0);
				IsFull = true;
			}
		}

		public void ClearAll()
		{
			IsFull = false;
			this.Clear();
		}

		public bool IsFull { get; private set; } = false;

		public T LastSample { get; private set; }
	}

}

