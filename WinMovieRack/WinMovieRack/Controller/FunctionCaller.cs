﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Controller.Parser;

namespace WinMovieRack.Controller {
	public class FunctionCaller : ConcThreadJobMaster {

		private FunctionCall fc = new FunctionCall(); 

		public FunctionCaller() {
			this.addJob(fc);
		}

		public override bool hasFinished(ThreadJob job) {
			return true;
		}
		public override bool isWaitingTask() {
			return true;
		}

		public void addFunction(WorkWithoutParam w) {
			fc.addFunction(w);
		}
		public void addFunction(WorkWithInt w, int i) {
			fc.addFunction(w,i);
		}
		public void addFunction(WorkWithObject w, object o) {
			fc.addFunction(w,o);
		}
	}
}
