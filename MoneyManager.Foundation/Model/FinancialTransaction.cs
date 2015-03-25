﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using PropertyChanged;
using SQLite.Net.Attributes;

#endregion

namespace MoneyManager.DataAccess.Model {
    [ImplementPropertyChanged]
    [Table("FinancialTransactions")]
    public class FinancialTransaction {
        #region Properties

        private IEnumerable<Account> allAccounts {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        private IEnumerable<Category> allCategories {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        private AccountDataAccess accountData {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private IEnumerable<RecurringTransaction> allRecurringTransactions {
            get {
                return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>().AllRecurringTransactions;
            }
        }

        #endregion Properties

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public DateTime Date { get; set; }

        public double AmountWithoutExchange { get; set; }

        public double Amount { get; set; }

        public bool IsExchangeModeActive { get; set; }

        public double ExchangeRatio { get; set; }

        public string Currency { get; set; }

        public int? CategoryId { get; set; }

        public bool Cleared { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }

        public bool IsRecurring { get; set; }

        public int? ReccuringTransactionId { get; set; }

        [Ignore]
        public Account ChargedAccount {
            get {
                if (allAccounts == null) {
                    accountData.LoadList();
                }
                return allAccounts.FirstOrDefault(x => x.Id == ChargedAccountId);
            }
            set { ChargedAccountId = value.Id; }
        }

        [Ignore]
        public Account TargetAccount {
            get {
                if (allAccounts == null) {
                    accountData.LoadList();
                }
                return allAccounts.FirstOrDefault(x => x.Id == TargetAccountId);
            }
            set { TargetAccountId = value.Id; }
        }

        [Ignore]
        public Category Category {
            get {
                return allCategories != null
                    ? allCategories.FirstOrDefault(x => x.Id == CategoryId)
                    : new Category();
            }
            set {
                CategoryId = value == null
                    ? (int?) null
                    : value.Id;
            }
        }

        [Ignore]
        public RecurringTransaction RecurringTransaction {
            get { return allRecurringTransactions.FirstOrDefault(x => x.Id == ReccuringTransactionId); }
            set { ReccuringTransactionId = value.Id; }
        }

        [Ignore]
        public bool ClearTransactionNow {
            get { return Date.Date <= DateTime.Now.Date; }
        }

        [Ignore]
        public bool IsTransfer {
            get { return Type == (int) TransactionType.Transfer; }
        }
    }
}