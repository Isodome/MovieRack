using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Controller.ThreadManagement
{
    public interface ThreadJob
    {
        void run();
    }
}
