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
    class JobLoadAwardsPage : ThreadJob
    {
        private imdbMovieParserMaster parent;
        public JobLoadAwardsPage(imdbMovieParserMaster parent)
        {
            this.parent = parent;
        }

        public void run()
        {
            getAwardsPage();
            parent.hasFinished(this);
        }

        private void getAwardsPage()
        {
            WebResponse resp = parent.awardsPageRequest.GetResponse();
            StreamReader r = new StreamReader(resp.GetResponseStream());
            parent.awardsPage = r.ReadToEnd();
        }
    }
}
