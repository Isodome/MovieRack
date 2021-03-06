﻿using System;
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
        public bool isMovie;
        public Label editableCharacter;
        public MovieRackListBoxItem(MRListData dbItem, bool isMovie)
        {
            InitializeComponent();
            this.dbItem = dbItem;
            this.itemID = dbItem.dbItemID;
            this.title = dbItem.titleName;
            this.isMovie = isMovie;

            editableCharacter = labelEditableCharacter;
            labelTitleName.Content = dbItem.titleName;
            if (dbItem.yearAge != -1)
            {
                labelYearAge.Content = dbItem.yearAge;
            }
            else
            {
                labelYearAge.Content = "No Age";
            }
            if (isMovie)
            {
                if (dbItem.editableCharakter.Equals("-1"))
                {
                    labelEditableCharacter.Content = "No Runtime";
                }
                else
                {
                    labelEditableCharacter.Content = dbItem.editableCharakter + " min";
                }
            }
            else
            {
                labelEditableCharacter.Content = dbItem.editableCharakter;
            }
            if (isMovie)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(dbItem.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                poster.Source = posterBitmap;
            }
        }
        public void loadPicture()
        {
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(dbItem.dbItemID, PosterSize.LIST));
            posterBitmap.EndInit();
            poster.Source = posterBitmap;
        }
    }
}
