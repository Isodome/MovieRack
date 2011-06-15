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
    /// Interaction logic for ActorsView.xaml
    /// </summary>
    public partial class ActorsView : UserControl
    {
        ActorsViewController controller;
        GUIPerson personDetails;
        public ActorsView(ActorsViewController avc)
        {
            InitializeComponent();
            this.controller = avc;
        }

        public void addActorListBoxItem(MovieRackListBoxItem item)
        {
            listBoxActor.Items.Add(item);
            item.loadPicture();
        }

        private void listBoxActor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieRackListBoxItem selectetItem = (MovieRackListBoxItem)listBoxActor.SelectedItem;
            if (selectetItem != null)
            {
                getPersonDetails(selectetItem.itemID);
                setPersonDetails(personDetails);
            }
        }

        private void getPersonDetails(int itemID)
        {
            this.personDetails = controller.getGUIPerson(itemID);
        }

        private void setPersonDetails(GUIPerson personDetails)
        {
            this.name.Content = personDetails.Name;
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(personDetails.dbID, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            posterTitle.Source = posterBitmap;
            biography.Content = personDetails.Biography;
            orgialName.Content = personDetails.OriginalName;
        }
    }
}
