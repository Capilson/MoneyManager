﻿#region

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class AddTransactionViewModel {
        private readonly ITransactionRepository _transactionRepository;

        public AddTransactionViewModel(ITransactionRepository transactionRepository) {
            _transactionRepository = transactionRepository;
            IsNavigationBlocked = true;
        }

        #region Properties

        public ObservableCollection<Account> AllAccounts {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        public ObservableCollection<Category> AllCategories {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        public SettingDataAccess Settings {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        public DateTime EndDate { get; set; }

        public bool IsEndless { get; set; }

        public bool IsEdit { get; set; }

        public int Recurrence { get; set; }

        public bool IsTransfer { get; set; }

        public bool RefreshRealtedList { get; set; }

        #endregion Properties

        public string Title {
            get {
                string text = IsEdit
                    ? Translation.GetTranslation("EditTitle")
                    : Translation.GetTranslation("AddTitle");

                string type = TransactionTypeLogic.GetViewTitleForType(_transactionRepository.Selected.Type);

                return String.Format(text, type);
            }
        }

        public string AmountString {
            get { return AmountWithoutExchange.ToString(); }
            set {
                double amount;
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentUICulture, out amount)) {
                    AmountWithoutExchange = amount;
                }
            }
        }

        public double AmountWithoutExchange {
            get { return _transactionRepository.Selected.AmountWithoutExchange; }
            set {
                _transactionRepository.Selected.AmountWithoutExchange = value;
                CalculateNewAmount(value);
            }
        }

        public bool IsNavigationBlocked { get; set; }

        private void CalculateNewAmount(double value) {
            if (Math.Abs(_transactionRepository.Selected.ExchangeRatio) < 0.5) {
                _transactionRepository.Selected.ExchangeRatio = 1;
            }

            _transactionRepository.Selected.Amount = _transactionRepository.Selected.ExchangeRatio * value;
        }

        public async void SetCurrency(string currency) {
            _transactionRepository.Selected.Currency = currency;
            await LoadCurrencyRatio();
            _transactionRepository.Selected.IsExchangeModeActive = true;
            CalculateNewAmount(AmountWithoutExchange);
        }

        public async Task LoadCurrencyRatio() {
            _transactionRepository.Selected.ExchangeRatio =
                await CurrencyLogic.GetCurrencyRatio(Settings.DefaultCurrency, _transactionRepository.Selected.Currency);
        }

        public async void Save() {
            if (IsEdit) {
                await TransactionLogic.UpdateTransaction(_transactionRepository.Selected);
            } else {
                await TransactionLogic.SaveTransaction(_transactionRepository.Selected, RefreshRealtedList);
            }

            ((Frame) Window.Current.Content).GoBack();
        }

        public async void Cancel() {
            if (IsEdit) {
                await AccountLogic.AddTransactionAmount(_transactionRepository.Selected);
            }

            ((Frame) Window.Current.Content).GoBack();
        }
    }
}