using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WinMovieRack.Model;

namespace WinMovieRack.GUI
{
    class ListComparerTest : IComparer
    {
        public int Compare(object pers1, object pers2)
        {
            return ((MovieRackListBoxItem)pers1).title.CompareTo(((MovieRackListBoxItem)pers2).title);
        } 
    }
}
