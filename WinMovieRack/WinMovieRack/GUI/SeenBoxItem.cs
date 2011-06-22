using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.GUI
{
    class SeenBoxItem
    {
        private string date;
        private string notes;

        public SeenBoxItem(string date, string notes)
        {
            this.date = date;
            this.notes = notes;
            if (notes.Equals(""))
            {
                this.notes = "No Notes";
            }
            Console.WriteLine(notes);
        }

        public string labelOne
        {
            get { return date; }
        }

        public string labelTwo
        {
            get { return notes; }
        }
    }
}
