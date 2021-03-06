﻿using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Manager;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Views;

namespace MoneyManager.UserControls {
    public sealed partial class SelectCurrencyUserControl {
        public SelectCurrencyUserControl() {
            InitializeComponent();
        }

        private async void LoadCountries(object sender, RoutedEventArgs e) {
            if (ServiceLocator.Current.GetInstance<LicenseManager>().IsFeaturepackLicensed) {
                await ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>().LoadCountries();
            }
            else {
                var dialog = new MessageDialog(Translation.GetTranslation("ShowFeatureNotLicensedMessage"),
                    Translation.GetTranslation("FeatureNotLicensedTitle"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("RedirectLabel"), GoToPurchase));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("BackLabel"), NavigateBack));
                dialog.ShowAsync();
            }
        }

        private void GoToPurchase(IUICommand command) {
            ((Frame) Window.Current.Content).Navigate(typeof (LicenseView));
        }

        private void NavigateBack(IUICommand command) {
            ((Frame) Window.Current.Content).GoBack();
        }
    }
}