using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Controller.ThreadManagement {
	public delegate void FinalizeFunctionSerial(SerialThreadJobMaster master);
	public abstract class SerialThreadJobMaster {

		private FinalizeFunctionSerial finFunction = null;

		public abstract bool run();

		public void setFinalizeFunction(FinalizeFunctionSerial func) {
			this.finFunction = func;
		}

		public void finalize() {
			if (finFunction != null) {
				finFunction(this);
			}
		}
	}
}
