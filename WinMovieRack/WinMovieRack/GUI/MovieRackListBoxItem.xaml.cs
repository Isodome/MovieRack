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
using System.IO;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaktionslogik für MoviesListBoxItem.xaml
    /// </summary>
    public partial class MovieRackListBoxItem : UserControl
    {
        private MRListData dbItem;
        public int itemID;
        public string title;
        public MovieRackListBoxItem(MRListData dbItem, bool isMovie)
        {
            InitializeComponent();
            this.dbItem = dbItem;
            this.itemID = dbItem.dbItemID;
            this.title = dbItem.titleName;
            labelTitleName.Content = dbItem.titleName;
            labelYearAge.Content = dbItem.yearAge;
            labelEditableCharacter.Content = dbItem.editableCharakter;
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            if (isMovie)
            {
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(dbItem.dbItemID, PosterSize.LIST));
            }
            else
            {
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(dbItem.dbItemID, PosterSize.LIST));
            }
			posterBitmap.CacheOption = BitmapCacheOption.None;
			posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
            posterBitmap.EndInit();
            poster.Source = posterBitmap;
		}
    }
}
