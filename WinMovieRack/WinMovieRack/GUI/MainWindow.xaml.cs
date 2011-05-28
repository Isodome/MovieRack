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
using WinMovieRack.Resources.Localization.MainWindow;
using WinMovieRack.Model;

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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DetailsView d = new DetailsView();
            changeView(d);
            DBInterface i = new DBInterface();
            i.initDb();
            i.checkTables();
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

        private void fileMenuEntry_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
