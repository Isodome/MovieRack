using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using WinMovieRack.Model;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaktionslogik für MoviesListBoxItem.xaml
    /// </summary>
    public partial class MovieRackListBoxItem : UserControl
    {
        private MRListData dbItem;
        public int itemID;

        public MovieRackListBoxItem(MRListData dbItem)
        {
            InitializeComponent();
            this.dbItem = dbItem;
            this.itemID = dbItem.dbItemID;
            labelTitleName.Content = dbItem.titleName;
            labelYearAge.Content = dbItem.yearAge;
            labelEditableCharacter.Content = dbItem.editableCharakter;
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(dbItem.posterPath);
            posterBitmap.EndInit();
            poster.Source = posterBitmap;
        }
    }
}
