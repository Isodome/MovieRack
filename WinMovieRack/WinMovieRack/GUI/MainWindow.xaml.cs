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
using WinMovieRack.Resources.Localization.MainWindow;
using WinMovieRack.Model;
using WinMovieRack.Controller.Parser;
using System.Threading;
namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UIElement current;

        public MainWindow()
        {
            InitializeComponent();         
        }

        void setLocalization()
        {
            fileMenuEntry.Header = MainWindowStrings.FileKey;
        }

        private void moviesMenuEntry_Clicked(object sender, RoutedEventArgs e)
        {
            DetailsView d = new DetailsView();
            changeView(d);

			imdbMovieParser p = new imdbMovieParser(477348);
			Console.WriteLine("http request");
			p.doRequest();
			//imdbMovieParser p11 = new imdbMovieParser(0120737);
			//p11.doRequest();

        }
        private void changeView(UIElement newView)
        {
            mainGrid.Children.Remove(current);
            Grid.SetColumn(newView, 0);
            Grid.SetRow(newView, 2);
            Grid.SetColumnSpan(newView, 3);
            mainGrid.Children.Add(newView);
            current = newView;
        }
    }
}