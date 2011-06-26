using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace WinMovieRack.GUI.Wizard
{
    public class WizardLauncher : PageFunction<WizardResult>
    {
        WizardData wizardData = new WizardData();
        public event WizardReturnEventHandler WizardReturn;

        protected override void Start()
        {
            base.Start();

            // So we remember the WizardCompleted event registration
            this.KeepAlive = true;

            // Launch the wizard
            WizardPageSelectSites wizardPageSelectSites = new WizardPageSelectSites(this.wizardData);
            wizardPageSelectSites.Return += new ReturnEventHandler<WizardResult>(wizardPage_Return);
            this.NavigationService.Navigate(wizardPageSelectSites);
        }

        public void wizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            // Notify client that wizard has completed
            // NOTE: We need this custom event because the Return event cannot be
            // registered by window code - if WizardDialogBox registers an event handler with
            // the WizardLauncher's Return event, the event is not raised.
            if (this.WizardReturn != null)
            {
                this.WizardReturn(this, new WizardReturnEventArgs(e.Result, this.wizardData));
            }
            OnReturn(null);
        }
    }
}
