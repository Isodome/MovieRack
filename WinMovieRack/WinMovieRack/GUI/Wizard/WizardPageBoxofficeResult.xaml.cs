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

namespace WinMovieRack.GUI.Wizard
{
    /// <summary>
    /// Interaction logic for WizardPageBoxofficeResult.xaml
    /// </summary>
    public partial class WizardPageBoxofficeResult : PageFunction<WizardResult>
    {
        WizardData wizardData;
        public WizardPageBoxofficeResult(WizardData wizardData)
        {
            InitializeComponent();
            this.wizardData = wizardData;
            this.DataContext = wizardData;
        }

        private void approxMatches_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedTitle = (MRListBoxItem)sender;
           // wizardData.boxofficeID = selectedTitle.getId; //ToDo


            // Go to next wizard page
            //    WizardPage2 wizardPage2 = new WizardPage2((WizardData)this.DataContext);
            //    wizardPage2.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
            //  this.NavigationService.Navigate(wizardPage2);
        }

        private void popularTitles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedTitle = (MRListBoxItem)sender;
        //    wizardData.boxofficeID = selectedTitle.getId;


            // Go to next wizard page
            //   WizardPage2 wizardPage2 = new WizardPage2((WizardData)this.DataContext);
            //    wizardPage2.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
            //  this.NavigationService.Navigate(wizardPage2);
        }
    }
}
