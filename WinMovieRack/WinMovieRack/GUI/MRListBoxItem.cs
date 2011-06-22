using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WinMovieRack.GUI
{
    public class MRListBoxItem
    {
        public string title;
        private string year;
        private string editable;
        private int id;
        private BitmapImage pictureBitmap;

        public MRListBoxItem(int id, string title, string year, string editable, BitmapImage pictureBitmap)
        {
            this.title = title;
            this.year = year;
            this.editable = editable;
            this.pictureBitmap = pictureBitmap;
            this.id = id;
        }

        public string labelOne
        {
            get { return title; }
            set { title = value; }
        }
        public string labelTwo
        {
            get { return year; }
            set { year = value; }
        }
        public string labelThree
        {
            get { return editable; }
            set { editable = value; }
        }

        public int getId
        {
            get { return id; }
        }

        public BitmapImage picture
        {
            get { return pictureBitmap; }
            set { pictureBitmap = value; }
        }
    }
}
