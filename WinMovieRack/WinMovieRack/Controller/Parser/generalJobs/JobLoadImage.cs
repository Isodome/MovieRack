using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Drawing;

namespace WinMovieRack.Controller.Parser
{
    class JobLoadImage : ThreadJob
    {
		string url;
		Image result;
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
			WebRequest req = WebRequest.Create(url);
			WebResponse resp = req.GetResponse();
			result = Image.FromStream(resp.GetResponseStream(), true, true);
			if (savePath != null)
			{
				result.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			resp.Close();
		}
		public Image getResult()
		{
			return this.result;
		}
    }
}
