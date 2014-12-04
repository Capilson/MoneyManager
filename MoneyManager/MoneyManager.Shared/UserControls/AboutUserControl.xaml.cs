#region

using System;
using Windows.System;
using Windows.UI.Xaml.Input;
using MoneyManager.Business.Helper;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.UserControls
{
    public sealed partial class AboutUserControl
    {
        public AboutUserControl()
        {
            InitializeComponent();

            lblVersion.Text = Utilities.GetVersion();
        }


        private async void ComposeMail_OnTap(object sender, TappedRoutedEventArgs e)
        {
            Utilities.SendMail(Translation.GetTranslation("SupportMail"), Translation.GetTranslation("Feedback"), String.Empty);
        }

        private async void GoToWebsite_OnTap(object sender, TappedRoutedEventArgs e)
        {
            const string url = "http://www.apply-solutions.ch/moneyfoxbeta";
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        private async void GoToTwitter_OnTap(object sender, TappedRoutedEventArgs e)
        {
            const string url = "http://twitter.com/npadrutt";
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}