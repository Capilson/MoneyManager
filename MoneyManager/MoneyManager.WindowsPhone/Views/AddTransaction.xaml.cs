﻿#region

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

#endregion

namespace MoneyManager.Views {
    public sealed partial class AddTransaction {
        private readonly NavigationHelper navigationHelper;

        public AddTransaction() {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        private AddTransactionViewModel AddTransactionView {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        private ITransactionRepository transactionRepository {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>(); }
        }

        public NavigationHelper NavigationHelper {
            get { return navigationHelper; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            if (e.NavigationMode != NavigationMode.Back && AddTransactionView.IsEdit) {
                await AccountLogic.RemoveTransactionAmount(transactionRepository.Selected);
            }

            base.OnNavigatedTo(e);
        }

        private void DoneClick(object sender, RoutedEventArgs e) {
            if (transactionRepository.Selected.ChargedAccount == null) {
                ShowAccountRequiredMessage();
                return;
            }

            AddTransactionView.Save();
        }

        private async void ShowAccountRequiredMessage() {
            var dialog = new MessageDialog
                (
                Translation.GetTranslation("AccountRequiredMessage"),
                Translation.GetTranslation("MandatoryField")
                );
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;
            await dialog.ShowAsync();
        }

        private void CancelClick(object sender, RoutedEventArgs e) {
            AddTransactionView.Cancel();
        }
    }
}