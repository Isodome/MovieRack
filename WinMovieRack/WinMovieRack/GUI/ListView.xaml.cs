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
using System.Runtime.InteropServices;
using WinMovieRack.Model;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : System.Windows.Controls.UserControl
    {

        ListViewController controller;
        public ListView(ListViewController lvc)
        {
            InitializeComponent();
            this.controller = lvc;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Test");
        }

        private void movieList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GUIMovie movie = (GUIMovie)movieList.SelectedItem;
            movieTitleBox.Text = movie.title;
            plot.Text = movie.plot;
        }
    }

}

