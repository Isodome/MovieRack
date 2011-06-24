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
using System.Windows.Forms;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : System.Windows.Controls.UserControl
    {
        [DllImport("uxtheme", CharSet = CharSet.Unicode)]
        public extern static Int32 SetWindowTheme
                (IntPtr hWnd, String textSubAppName, String textSubIdList);

        ListViewController controller;
        public ListView(ListViewController lvc)
        {
            InitializeComponent();
            this.controller = lvc;
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            System.Windows.Forms.TreeView test = new System.Windows.Forms.TreeView();
            host.Child = test;
            test.Nodes.Add(new TreeNode("Test"));
            test.Nodes.Add(new TreeNode("Test1"));
            test.Nodes.Add(new TreeNode("Test2"));
            test.Nodes.Add(new TreeNode("Test3"));
            test.Nodes.Add(new TreeNode("Test4"));
           // gridFilter.Children.Add(host);
            System.Windows.Forms.Integration.WindowsFormsHost.EnableWindowsFormsInterop();
            System.Windows.Forms.Application.EnableVisualStyles();
            SetWindowTheme(test.Handle, "Explorer", null);
        }
    }

}

