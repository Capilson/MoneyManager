using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class TransactionRepository : ITransactionRepository {
        private ObservableCollection<FinancialTransaction> _data;
        private readonly IDataAccess<FinancialTransaction> _dataAccess;

        public TransactionRepository(IDataAccess<FinancialTransaction> dataAccess) {
            _dataAccess = dataAccess;
        }

        public FinancialTransaction Selected { get; set; }

        public ObservableCollection<FinancialTransaction> Data {
            get { return _data ?? (_data = new ObservableCollection<FinancialTransaction>(_dataAccess.LoadList())); }
            set {
                if (_data != null && _data == value) {
                    return;
                }
                _data = value;
            }
        }

        public void Save(FinancialTransaction item) {
            throw new NotImplementedException();
        }

        public void Delete(FinancialTransaction item) {
            throw new NotImplementedException();
        }

        public IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId) {
            return Data
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

    }
}