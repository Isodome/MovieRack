using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using WinMovieRack.Controller.ThreadManagement;
using System.Net;
namespace WinMovieRack.Controller.Parser
{
	class JobWebPageDownload : ThreadJob
	{
		private const int maxAttempts = 3;
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
			int attemps = maxAttempts;
			do {
				try {
					WebRequest req = WebRequest.Create(url);
					WebResponse resp = req.GetResponse();
					StreamReader r = new StreamReader(resp.GetResponseStream());
					result = WebUtility.HtmlDecode(r.ReadToEnd());
					resp.Close();
					r.Close();
					break;
				} catch (UriFormatException e) {
					Console.WriteLine("Unvalid URL: " + this.url);
					attemps--;
				} catch (Exception f) {
					Console.WriteLine("Webexception: " + this.url + f.Message);
					attemps--;
				}
			} while (attemps > 0);

			if (attemps == 0) {
				result = null;
				//ERRORHANDLING
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
