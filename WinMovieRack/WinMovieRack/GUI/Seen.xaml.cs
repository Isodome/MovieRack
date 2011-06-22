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
    /// Interaction logic for Seen.xaml
    /// </summary>
    public partial class Seen : Window
    {
        private DetailsViewController detailsViewController;
        private int id;
        DateTime selectedDate;
        public Seen(DetailsViewController dvc, int id)
        {
            InitializeComponent();
            this.detailsViewController = dvc;
            this.id = id;
            calendar.DisplayDateEnd = DateTime.Today;
            this.Left = System.Windows.Forms.Control.MousePosition.X;
            this.Top = System.Windows.Forms.Control.MousePosition.Y;
            calendar.SelectedDate = DateTime.Today;
        }

        private void todayButton_Click(object sender, RoutedEventArgs e)
        {
            dateButton.IsChecked = false;
            selectedDate = DateTime.Today;
            date.Content = selectedDate.ToShortDateString();
        }

        private void yesterdayButton_Click(object sender, RoutedEventArgs e)
        {
            dateButton.IsChecked = false;
            selectedDate = DateTime.Today.Subtract(new TimeSpan(TimeSpan.TicksPerDay));
            date.Content = selectedDate.ToShortDateString();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDate.Year != 1)
            {
                detailsViewController.setSeenDate(selectedDate, id, notes.Text);
                this.Close();
            }
            else
            {
                new ErrorMessage("No Date");
            }
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = (DateTime)calendar.SelectedDate;
            date.Content = selectedDate.ToShortDateString();
        }

        private void dateButton_Checked(object sender, RoutedEventArgs e)
        {
            calendar.SelectedDatesChanged += calendar_SelectedDatesChanged;
            selectedDate = (DateTime)calendar.SelectedDate;
            date.Content = selectedDate.ToShortDateString();
        }

        private void dateButton_Unchecked(object sender, RoutedEventArgs e)
        {
            calendar.SelectedDatesChanged -= calendar_SelectedDatesChanged;
        }

    }
}
