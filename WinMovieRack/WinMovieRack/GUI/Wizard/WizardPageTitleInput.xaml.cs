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
    /// Interaction logic for WizardPageTitleInput.xaml
    /// </summary>
    public partial class WizardPageTitleInput : PageFunction<WizardResult>
    {
        WizardData wizardData;
        public WizardPageTitleInput(WizardData wizardData)
        {
            InitializeComponent();
            this.DataContext = wizardData;
            this.wizardData = wizardData;
        }


        void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (wizardData.imdb)
            {
                //Go to next wizard page
                WizardPageIMDBResult wizardPageIMDBResult = new WizardPageIMDBResult((WizardData)this.DataContext);
                wizardPageIMDBResult.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
                this.NavigationService.Navigate(wizardPageIMDBResult);
            }
            else if (wizardData.boxoffice)
            {
                //Go to next wizard page
                WizardPageBoxofficeResult wizardPageBoxofficeResult = new WizardPageBoxofficeResult((WizardData)this.DataContext);
                wizardPageBoxofficeResult.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
                this.NavigationService.Navigate(wizardPageBoxofficeResult);
            }


            // Go to next wizard page
            //   WizardPage2 wizardPage2 = new WizardPage2((WizardData)this.DataContext);
            //    wizardPage2.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
            //  this.NavigationService.Navigate(wizardPage2);
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
    }
}
