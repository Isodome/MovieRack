using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace WinMovieRack.Controller.Parser
{
    class JobLoadMainPage : ThreadJob
    {
        private imdbMovieParserMaster parent;
        public JobLoadMainPage(imdbMovieParserMaster parent)
        {
            this.parent = parent;
        }

        public void run()
        {
            getMainPage();
            parent.hasFinished(this);
        }

        private void getMainPage()
        {
            WebResponse resp = parent.mainPageRequest.GetResponse();
            StreamReader r = new StreamReader(resp.GetResponseStream());
            parent.mainPage = r.ReadToEnd();
        }
    }
}
