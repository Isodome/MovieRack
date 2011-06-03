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
using WinMovieRack.GUI;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for Movies.xaml
    /// </summary>
    public partial class DetailsView : UserControl
    {
        private UIElement current;
        DetailsViewActorPanel actorPanel;
        public DetailsView()
        {
            InitializeComponent();
        }

        public void addMoviesListBoxItem(MoviesListBoxItem item)
        {
            listBoxMovies.Items.Add(item);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void listBoxMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            actorPanel = new DetailsViewActorPanel();
            changeView(actorPanel);
        }

        private void changeView(UIElement newView)
        {
            tabControl.Children.Remove(current);
            Grid.SetColumn(newView, 2);
            Grid.SetRow(newView, 2);
            Grid.SetColumnSpan(newView, 1);
            tabControl.Children.Add(newView);
            current = newView;
        }

        private void director_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
