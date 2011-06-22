using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model
{
    public class MRSeenData
    {
        public string date;
        public string notes;
        public MRSeenData(DateTime date, string notes)
        {
            this.date = date.ToShortDateString();
            this.notes = notes;
        }
    }
}
