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
    class JobLoadImage : ThreadJob
    {
		private const int maxAttempts = 3;
		string url;
		Bitmap result;
		string savePath;

        public JobLoadImage(string url, string savePath)
        {
			this.savePath = savePath;
			this.url = url;
        }

        public void run()
        {
            getPicture();
        }

		private void getPicture()
		{
			int attempts = maxAttempts;
			do {
				try {
					WebRequest req = WebRequest.Create(url);
					WebResponse resp = req.GetResponse();
					result = new Bitmap(resp.GetResponseStream());
					resp.Close();
					break;
				} catch (Exception) {
					result = null;
					attempts--;
					Console.WriteLine("Error downloading image from {0} in attempt {1}. There are {2} attemps remaining", this.url, maxAttempts - attempts, attempts);
				}

			} while (attempts > 0);

			if (attempts == 0) {
				result = null;
				Console.WriteLine("Download of image from {0} failed after {1} unsuccessful attemps.", this.url, maxAttempts);
				// ERRORHANDLING
			} else if (savePath != null) {
				result.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			
		}
		public Bitmap getResult()
		{
			return this.result;
		}
    }
}
