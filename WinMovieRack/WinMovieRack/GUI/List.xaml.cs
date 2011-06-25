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

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for List.xaml
    /// </summary>
    public partial class List : Window
    {
        GUIController controller;

        public List(GUIController controller)
        {
            InitializeComponent();
            this.controller = controller;
            this.Left = System.Windows.Forms.Control.MousePosition.X;
            this.Top = System.Windows.Forms.Control.MousePosition.Y;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            controller.addList(listTextBox.Text);
            this.Close();
        }
    }
}
