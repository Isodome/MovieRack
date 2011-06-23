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
using WinMovieRack.Model;

namespace WinMovieRack.GUI {
    /// <summary>
    /// Interaktionslogik für TodoList.xaml
    /// </summary>
    public partial class TodoList : UserControl {
        private TodoListController controller;

        public TodoList(TodoListController contr) {
            InitializeComponent();
            this.controller = contr;
        }

    }
}
