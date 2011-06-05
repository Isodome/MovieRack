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

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for BigPicture.xaml
    /// </summary>
    public partial class BigPicture : Window
    {
        public BigPicture()
        {
            InitializeComponent();
            this.MaxHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        }

        private void bigPicture_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bigPicture_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0.5;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void buttonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 1;

        }

        private void buttonGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0;
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void quitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 1;
        }

        private void quitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0;
        }

        private void saveButton_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 1;
        }

        private void saveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0;
        }
    }
}
