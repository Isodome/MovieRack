using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using WinMovieRack.Controller.ThreadManagement;
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
			try {
				WebRequest req = WebRequest.Create(url);
				WebResponse resp = req.GetResponse();
				StreamReader r = new StreamReader(resp.GetResponseStream());
				result = r.ReadToEnd();
				resp.Close();
				r.Close();
			} catch (UriFormatException e) {
				Console.WriteLine("Unvalid URL: " + this.url);
			} catch (WebException f) {
				Console.WriteLine("Webexception: " + this.url + f.Message);
			}
        }

		/// <summary>
		/// Get the result of the WebRequest as a string
		/// </summary>
		/// <returns>The result of the web request as string</returns>
		public string getResult()
		{
			return (this.result);
		}
	}
}
