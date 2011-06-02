using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WinMovieRack.Controller.Parser
{
	class JobWebPageDownload : ThreadJob
	{
		private string url;
		private string result;
        public JobWebPageDownload(string url)
        {
            this.url = url;
        }

        public void run()
        {
            getMainPage();
        }

        private void getMainPage()
        {
			WebRequest req = WebRequest.Create(url);
            WebResponse resp =req.GetResponse();
            StreamReader r = new StreamReader(resp.GetResponseStream());
            result = r.ReadToEnd();
        }

		public string getResult()
		{
			return (this.result);
		}
	}
}
