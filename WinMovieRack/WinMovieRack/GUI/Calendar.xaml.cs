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
using System.Windows.Shapes;
using WinMovieRack.Controller;
namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Window
    {
        private DetailsViewController detailsViewController;
        private int id;
        public Calendar(DetailsViewController dvc, int id)
        {
            InitializeComponent();
            this.Left = System.Windows.Forms.Control.MousePosition.X;
            this.Top = System.Windows.Forms.Control.MousePosition.Y;
            this.detailsViewController = dvc;
            this.id = id;
            seenCalendar.DisplayDateEnd = DateTime.Today;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void seenCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            detailsViewController.setSeenDate((DateTime)seenCalendar.SelectedDate, id);
            this.Close();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
