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
		private const int maxAttempts = 5;
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
			int attempts = maxAttempts;
			do {
				try {
					WebRequest req = HttpWebRequest.Create(url);
					req.Proxy = null;
					WebResponse resp = req.GetResponse();
					StreamReader r = new StreamReader(resp.GetResponseStream());
					result = WebUtility.HtmlDecode(r.ReadToEnd());
					r.Close();
					resp.Close();
					break;
				} catch (UriFormatException e) {
					Console.WriteLine("Unvalid URL: " + this.url);
					attempts--;
					Console.WriteLine("Error downloading website from {0} in attempt {1}. There are {2} attemps remaining", this.url, maxAttempts - attempts, attempts);
				} catch (Exception f) {
					attempts--;
					Console.WriteLine("Webexception: " + this.url + f.Message);
					Console.WriteLine("Error downloading website from {0} in attempt {1}. There are {2} attemps remaining", this.url, maxAttempts - attempts, attempts);

				} finally {

				}
			} while (attempts > 0);

			if (attempts == 0) {
				result = null;
				Console.WriteLine("Download of website from {0} failed after {1} unsuccessful attemps.", this.url, maxAttempts);
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
