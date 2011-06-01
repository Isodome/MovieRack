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
    class JobLoadCreditsPage : ThreadJob
    {
        private imdbMovieParserMaster parent;
        public JobLoadCreditsPage(imdbMovieParserMaster parent)
        {
            this.parent = parent;
        }

        public void run()
        {
            getCreditsPage();
            parent.hasFinished(this);
        }

        private void getCreditsPage()
        {
            WebResponse resp = parent.creditsPageRequest.GetResponse();
            StreamReader r = new StreamReader(resp.GetResponseStream());
            parent.creditsPage = r.ReadToEnd();
        }
    }
}
