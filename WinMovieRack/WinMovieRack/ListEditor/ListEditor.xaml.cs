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
using WinMovieRack.Resources.Localization.ListEditor;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for ListEditor.xaml
    /// </summary>
    public partial class ListEditor : Window
    {
        private string sqlTableName;
        private string sqlColumnName;
        private string propertyToDisplay;

        public ListEditor(string sqlTable, string sqlCol, string displayName)
        {
            InitializeComponent();
            this.sqlTableName = sqlTable;
            this.sqlColumnName = sqlCol;
            this.propertyToDisplay = displayName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setLocalization();
        }

        private void setLocalization()
        {
            addButton.Content = ListEditorStrings.AddButtonKey;
            editButton.Content = ListEditorStrings.EditButtonKey;
            deleteButton.Content = ListEditorStrings.DeleteButtonKey;
        }
    }
}
