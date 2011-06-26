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
using System.Collections.ObjectModel;

namespace WinMovieRack.GUI.Wizard
{
    /// <summary>
    /// Interaction logic for WizardPageIMDBResult.xaml
    /// </summary>
    public partial class WizardPageIMDBResult : PageFunction<WizardResult>
    {
        WizardData wizardData;
        public WizardPageIMDBResult(WizardData wizardData)
        {
            InitializeComponent();
            this.wizardData = wizardData;
            this.DataContext = wizardData;
        }

        void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the wizard and don't return any data
            OnReturn(new ReturnEventArgs<WizardResult>(WizardResult.Canceled));
        }

        public void wizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            // If returning, wizard was completed (finished or canceled),
            // so continue returning to calling page
            OnReturn(e);
        }


        private void approxMatches_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedTitle = (MRListBoxItem)sender;
            wizardData.imdbID = selectedTitle.getId;
            // Go to next wizard page
           //    WizardPage2 wizardPage2 = new WizardPage2((WizardData)this.DataContext);
            //    wizardPage2.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
            //  this.NavigationService.Navigate(wizardPage2);
        }

        private void popularTitles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedTitle = (MRListBoxItem)sender;
            wizardData.imdbID = selectedTitle.getId;
            // Go to next wizard page
            //   WizardPage2 wizardPage2 = new WizardPage2((WizardData)this.DataContext);
            //    wizardPage2.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
            //  this.NavigationService.Navigate(wizardPage2);
        }
    }
}
