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
    public partial class MovieRackListBoxItem : UserControl
    {
        public MovieRackListBoxItem()
        {
            InitializeComponent();
        }

        public void setListBoxTitle(String title) 
        {
            labelMovieTitle.Content = title;
        }
        public void setYearCharakter(String yearCharakter)
        {
            labelMovieYear.Content = yearCharakter;
        }
        public void setEditableAge(String editableAge)
        {
            labelMovieEditable.Content = editableAge;
        }
        public void setPicture()
        {
        }
    }
}
