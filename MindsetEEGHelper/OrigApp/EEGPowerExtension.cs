using System;
using Com.Neurosky.Connection;

namespace gnf
{
	public static class EEGPowerExtension
	{
		public static string ToCommaString(this EEGPower power)
		{
			return power.Delta.ToString("F2") + "," +
				power.HighAlpha.ToString("F2") + "," +
				power.HighBeta.ToString("F2") + "," +
				power.LowAlpha.ToString("F2") + "," +
				power.LowBeta.ToString("F2") + "," +
				power.LowGamma.ToString("F2") + "," +
				power.MiddleGamma.ToString("F2") + "," +
				power.Theta.ToString("F2");
		}

		public static int GetTotalPower(this EEGPower power)
		{
			return power.Delta +
				power.HighAlpha +
				power.HighBeta + 
				power.LowAlpha + 
				 power.LowBeta + 
				power.LowGamma + 
	     	power.MiddleGamma + 
				power.Theta;
		}

		public static int GetTotalPowerWithoutDelta(this EEGPower power)
		{
			return 
				power.HighAlpha +
				power.HighBeta +
				power.LowAlpha +
				 power.LowBeta +
				power.LowGamma +
			 power.MiddleGamma +
				power.Theta;
		}


	}
}

