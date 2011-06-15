using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Drawing;
using WinMovieRack.Controller.ThreadManagement;

namespace WinMovieRack.Controller.Parser
{
	public delegate void WorkWithoutParam();
	public delegate void WorkWithInt(int i);
	public delegate void WorkWithObject(object obj);
    class FunctionCall : ThreadJob
    {
		private WorkWithoutParam work;
		private WorkWithInt withInt;
		private WorkWithObject withObj;

        public void run()
        {
			work();

        }
		public void addFunction(WorkWithoutParam w) {
			work += w;
		}
		public void addFunction(WorkWithInt w, int i) {
			work += () => {
				w(i);
			};
		}
		public void addFunction(WorkWithObject w, object o) {
			work += () => {
				w(o);
			};
		}
    }
}
