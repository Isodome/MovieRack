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
        private imdbMovieParserMaster parent;
        public JobLoadImage(imdbMovieParserMaster parent)
        {
            this.parent = parent;
        }

        public void run()
        {
            getPicture();
            parent.hasFinished(this);
        }

        private void getPicture()
        {
            Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.mediaURLRegex);
            string pictureURL = m.Groups["url"].Value + ".jpg";
            WebResponse resp = WebRequest.Create(pictureURL).GetResponse();
            parent.poster = Image.FromStream(resp.GetResponseStream(), true, true);
            parent.poster.Save(parent.imdbID.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
