using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model
{
    public class MRListData
    {
        public int dbItemID;
        public string titleName;
        public int yearAge;
        public string editableCharakter;

        public MRListData(int dbItemID, string titleName, int yearAge, string editableCharakter)
        {
            this.dbItemID = dbItemID;
            this.titleName = titleName;
            this.yearAge = yearAge;
            this.editableCharakter = editableCharakter;
        }
    }
}
