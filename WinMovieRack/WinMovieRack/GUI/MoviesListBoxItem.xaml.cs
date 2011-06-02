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

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaktionslogik für MoviesListBoxItem.xaml
    /// </summary>
    public partial class MoviesListBoxItem : UserControl
    {
        public MoviesListBoxItem()
        {
            InitializeComponent();
        }

        public void setMovieTitle(String title) 
        {
            labelMovieTitle.Content = title;
        }
        public void setMovieYear(String year)
        {
            labelMovieYear.Content = year;
        }
        public void setMovieEditable(String editable)
        {
            labelMovieEditable.Content = editable;
        }
    }
}
