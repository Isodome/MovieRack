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
			try {
				WebRequest req = WebRequest.Create(url);
				WebResponse resp = req.GetResponse();
				result = new Bitmap(resp.GetResponseStream());
				if (savePath != null) {
					result.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
				}
				resp.Close();
			} catch (Exception) {
				result = null;
			}

			
			
		}
		public Bitmap getResult()
		{
			return this.result;
		}
    }
}
