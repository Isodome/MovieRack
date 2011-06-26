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
using System.Windows.Navigation;

namespace WinMovieRack.GUI.Wizard
{
    /// <summary>
    /// Interaction logic for MovieAddWizard.xaml
    /// </summary>
    public partial class MovieAddWizard : NavigationWindow
    {
        WizardData wizardData;

        public MovieAddWizard()
        {
            InitializeComponent();

            // Launch the wizard
            WizardLauncher wizardLauncher = new WizardLauncher();
            wizardLauncher.WizardReturn += new WizardReturnEventHandler(wizardLauncher_WizardReturn);
            this.Navigate(wizardLauncher);
        }

        public WizardData WizardData
        {
            get { return this.wizardData; }
        }

        void wizardLauncher_WizardReturn(object sender, WizardReturnEventArgs e)
        {
            // Handle wizard return
            this.wizardData = e.Data as WizardData;
            if (this.DialogResult == null)
            {
                this.DialogResult = (e.Result == WizardResult.Finished);
            }
        }
    }
}
