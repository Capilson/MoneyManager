﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Xamarin;

#endregion

namespace MoneyManager.Business.Logic {
    public class TransactionLogic {
        #region Properties

        private static AccountDataAccess accountDataAccess {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private static ITransactionRepository TransactionRepository {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>(); }
        }

        private static FinancialTransaction selectedTransaction {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>().Selected; }
            set { ServiceLocator.Current.GetInstance<ITransactionRepository>().Selected = value; }
        }

        private static IRecurringTransactionRepository RecurringTransactionRepository {
            get { return ServiceLocator.Current.GetInstance<IRecurringTransactionRepository>(); }
        }

        private static AddTransactionViewModel addTransactionView {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        private static SettingDataAccess settings {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        #endregion Properties

        public static async Task SaveTransaction(FinancialTransaction transaction, bool refreshRelatedList = false,
            bool skipRecurring = false) {
            if (transaction.IsRecurring && !skipRecurring) {
                RecurringTransaction recurringTransaction =
                    RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);
                RecurringTransactionRepository.Save(recurringTransaction);
                transaction.RecurringTransaction = recurringTransaction;
            }

            TransactionRepository.Save(transaction);

            if (refreshRelatedList) {
                ServiceLocator.Current.GetInstance<TransactionListViewModel>()
                    .SetRelatedTransactions(accountDataAccess.SelectedAccount.Id);
            }
            await AccountLogic.AddTransactionAmount(transaction);
        }

        public static void GoToAddTransaction(TransactionType transactionType, bool refreshRelatedList = false) {
            addTransactionView.IsEdit = false;
            addTransactionView.IsEndless = true;
            addTransactionView.RefreshRealtedList = refreshRelatedList;
            addTransactionView.IsTransfer = transactionType == TransactionType.Transfer;
            SetDefaultTransaction(transactionType);
            SetDefaultAccount();
        }

        public static void PrepareEdit(FinancialTransaction transaction) {
            addTransactionView.IsEdit = true;
            addTransactionView.IsTransfer = transaction.Type == (int) TransactionType.Transfer;
            if (transaction.ReccuringTransactionId.HasValue && transaction.RecurringTransaction != null) {
                addTransactionView.IsEndless = transaction.RecurringTransaction.IsEndless;
                addTransactionView.Recurrence = transaction.RecurringTransaction.Recurrence;
            }
            selectedTransaction = transaction;
        }

        public static async Task DeleteTransaction(FinancialTransaction transaction, bool skipConfirmation = false) {
            if (skipConfirmation || await Utilities.IsDeletionConfirmed()) {
                await CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionLogic.Delete(transaction.RecurringTransaction));

                TransactionRepository.Delete(transaction);

                await AccountLogic.RemoveTransactionAmount(transaction);
                AccountLogic.RefreshRelatedTransactions();
                ServiceLocator.Current.GetInstance<BalanceViewModel>().UpdateBalance();
            }
        }

        public static void DeleteAssociatedTransactionsFromDatabase(int accountId) {
            if (TransactionRepository.Data == null) return;

            List<FinancialTransaction> transactionsToDelete = TransactionRepository.Data
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .ToList();

            foreach (FinancialTransaction transaction in transactionsToDelete) {
                TransactionRepository.Delete(transaction);
            }
        }

        public static async Task UpdateTransaction(FinancialTransaction transaction) {
            CheckIfRecurringWasRemoved(transaction);
            await AccountLogic.AddTransactionAmount(transaction);
            TransactionRepository.Save(transaction);

            RecurringTransaction recurringTransaction =
                RecurringTransactionLogic.GetRecurringFromFinancialTransaction(transaction);

            await
                CheckForRecurringTransaction(transaction,
                    () => RecurringTransactionRepository.Save(recurringTransaction));

            AccountLogic.RefreshRelatedTransactions();
        }

        private static async Task CheckForRecurringTransaction(FinancialTransaction transaction,
            Action recurringTransactionAction) {
            if (!transaction.IsRecurring) return;

            var dialog =
                new MessageDialog(Translation.GetTranslation("ChangeSubsequentTransactionsMessage"),
                    Translation.GetTranslation("ChangeSubsequentTransactionsTitle"));

            dialog.Commands.Add(new UICommand(Translation.GetTranslation("RecurringLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("JustThisLabel")));

            dialog.DefaultCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();

            if (result.Label == Translation.GetTranslation("RecurringLabel")) {
                recurringTransactionAction();
            }
        }

        private static void CheckIfRecurringWasRemoved(FinancialTransaction transaction) {
            if (!transaction.IsRecurring && transaction.ReccuringTransactionId.HasValue) {
                RecurringTransactionRepository.Delete(transaction.RecurringTransaction);
                transaction.ReccuringTransactionId = null;
            }
        }

        private static void SetDefaultTransaction(TransactionType transactionType) {
            selectedTransaction = new FinancialTransaction {
                Type = (int) transactionType,
                IsExchangeModeActive = false,
                Currency = ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency
            };
        }

        private static void SetDefaultAccount() {
            try {
                if (accountDataAccess.AllAccounts == null) {
                    accountDataAccess.LoadList();
                }

                if (accountDataAccess.AllAccounts.Any()) {
                    selectedTransaction.ChargedAccount = accountDataAccess.AllAccounts.First();
                }

                if (accountDataAccess.AllAccounts.Any() && settings.DefaultAccount != -1) {
                    selectedTransaction.ChargedAccount =
                        accountDataAccess.AllAccounts.First(x => x.Id == settings.DefaultAccount);
                }

                if (accountDataAccess.SelectedAccount != null) {
                    selectedTransaction.ChargedAccount = accountDataAccess.SelectedAccount;
                }
            } catch (Exception ex) {
                Insights.Report(ex, ReportSeverity.Error);
            }
        }

        public static async Task ClearTransactions() {
            IEnumerable<FinancialTransaction> transactions = RecurringTransactionRepository.GetUnclearedTransactions();
            foreach (FinancialTransaction transaction in transactions) {
                try {
                    await AccountLogic.AddTransactionAmount(transaction);
                } catch (Exception ex) {
                    Insights.Report(ex, ReportSeverity.Error);
                }
            }
        }
    }
}