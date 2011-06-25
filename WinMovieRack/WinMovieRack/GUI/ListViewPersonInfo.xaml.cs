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
using WinMovieRack.Controller;
using WinMovieRack.Model;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for ListViewPersonInfo.xaml
    /// </summary>
    public partial class ListViewPersonInfo : UserControl
    {
        ListViewPersonController controller;
        GUIPerson selectedPerson;
        public ListViewPersonInfo(ListViewPersonController controller)
        {
            InitializeComponent();
            this.controller = controller;
        }

        public void setPersonInfo(int personID)
        {
            selectedPerson= controller.getPersonInfo(personID);
            textBoxName.Text = selectedPerson.Name;
            nameBox.Text = selectedPerson.Name;
            originalName.Text = selectedPerson.OriginalName;
            birthday.Text = selectedPerson.Birthday;
            gender.Text = selectedPerson.gender;
            countryofBirth.Content = selectedPerson.CountryofBirth;
            cityofBirth.Content = selectedPerson.CityofBirth;
            lifetimeGross.Content = selectedPerson.lifetimeGross;
            boxofficeAverage.Content = selectedPerson.boxofficeAverage;
            oscarNom.Content = selectedPerson.OscarNominations;
            oscarWins.Content = selectedPerson.OscarWins;
            otherNom.Content = selectedPerson.OtherNominations;
            otherWins.Content = selectedPerson.OtherWins;
            biogrpahy.Text = selectedPerson.Biography;
            controller.loadMovieListToPerson(personID);
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(selectedPerson.dbID, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            posterInfo.Source = posterBitmap;
        }

        private void posterInfo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BigPicture bigPicture = new WinMovieRack.GUI.BigPicture();
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(selectedPerson.dbID, PosterSize.FULL));
            posterBitmap.EndInit();
            bigPicture.bigPicture.Source = posterBitmap;
            Point origin = new Point(0, 0);
            Point screenOrigin = posterInfo.PointToScreen(origin);
            bigPicture.setOrigin(posterInfo.Source.Height, posterInfo.Source.Width, screenOrigin.X, screenOrigin.Y);
            bigPicture.ShowDialog();
        }

        private void movieListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedItem = (MRListBoxItem)movieListBox.SelectedItem;
            controller.movieClicked(selectedItem);
        }
    }
}
