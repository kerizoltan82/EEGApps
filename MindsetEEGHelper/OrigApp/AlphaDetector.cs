using System;

namespace gnf
{

	class AlphaDetector
	{

		EEGSignals allSignals;
		public AlphaDetector(EEGSignals meditSignal)
		{
			this.allSignals = meditSignal;
		}

		public void Tick()
		{
			// old
			//IsAlpha = (meditSignal.meditSignals.IsFull && meditSignal.meditSignals.Average > 68);

			if (IsAlpha) {
				if( (allSignals.percentAlpha.Average / allSignals.percentBeta.Average) < 1.5f ) {
					// out from alpha state
					IsAlpha = false;
				}

				//no entry and exit levels yet
				//IsActiveMeditation = (allSignals.powerSignals.Average.Delta > 40000);

			} else {
				// go into alpha state
				IsAlpha = (allSignals.IsFull && allSignals.percentAlpha.Average > 10 && (allSignals.percentAlpha.Average / allSignals.percentBeta.Average) > 1.8f);

				IsActiveMeditation = false;
			}
		}

		public bool IsAlpha { get; private set; }

		public bool IsActiveMeditation { get; private set; }

		public int GetDetectorSignalForLog()
		{
			if (IsAlpha)
			{
				if (IsActiveMeditation)
				{
					return 100;
				}
				return 50;
			}
			return 0;
		}
	}


	// --------------------------------------

}

