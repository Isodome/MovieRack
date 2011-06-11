using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Controller.Parser.imdbMovieParser;

namespace WinMovieRack.Controller
{
    class TestJob : ThreadJob
    {
        private uint nr;
        private ConcurrentImdbMovieParser parent;

        public TestJob(ConcurrentImdbMovieParser parent, uint nr)
        {
            this.nr = nr;
            this.parent = parent;
        }
        public void run()
        {
            System.Console.WriteLine("Jobnummer: " + nr);
            parent.hasFinished(this);
        }
    }
}
