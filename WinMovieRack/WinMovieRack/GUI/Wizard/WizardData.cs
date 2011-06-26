using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.GUI.Wizard
{
    public class WizardData
    {
        bool imdbBool;
        bool boxofficeBool;
        bool metacritcisBool;
        bool rottentomatoeBool;
        int imdbIDInt;
        string boxofficeIDString;
        public bool imdb
        {
            get { return this.imdbBool; }
            set { this.imdbBool = value; }
        }

        public bool boxoffice
        {
            get { return this.boxofficeBool; }
            set { this.boxofficeBool = value; }
        }
        public bool metacritics
        {
            get { return this.metacritcisBool; }
            set { this.metacritcisBool = value; }
        }
        public bool rottentomatoe
        {
            get { return this.rottentomatoeBool; }
            set { this.rottentomatoeBool = value; }
        }

        public int imdbID
        {
            get { return this.imdbIDInt; }
            set { this.imdbIDInt = value; }
        }

        public string boxofficeID
        {
            get { return this.boxofficeIDString; }
            set { this.boxofficeIDString = value; }
        }
    }
}
