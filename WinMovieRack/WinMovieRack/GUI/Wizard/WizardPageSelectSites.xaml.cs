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
    /// Interaction logic for WizardPageSelectSites.xaml
    /// </summary>
    public partial class WizardPageSelectSites : PageFunction<WizardResult>
    {
        public WizardPageSelectSites(WizardData wizardData)
        {
            InitializeComponent();
            this.DataContext = wizardData;
        }

        void nextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            WizardPageTitleInput wizardPageTitleInput = new WizardPageTitleInput((WizardData)this.DataContext);
            wizardPageTitleInput.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
            this.NavigationService.Navigate(wizardPageTitleInput);
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
