using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class RecurringTransactionRepository :IRecurringTransactionRepository {
        private readonly IDataAccess<RecurringTransaction> _dataAccess;

        private ObservableCollection<RecurringTransaction> _data;

        public RecurringTransactionRepository(IDataAccess<RecurringTransaction> dataAccess) {
            _dataAccess = dataAccess;
        }

        public ObservableCollection<RecurringTransaction> Data {
            get { return _data ?? (_data = new ObservableCollection<RecurringTransaction>(_dataAccess.LoadList())); }
            set {
                if (_data != null && _data == value) {
                    return;
                }
                _data = value;
            }
        }

        public RecurringTransaction Selected { get; set; }

        public void Save(RecurringTransaction item) {
            throw new NotImplementedException();
        }

        public void Delete(RecurringTransaction item) {
            throw new NotImplementedException();
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions() {
            throw new NotImplementedException();
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date) {
            throw new NotImplementedException();
        }

        public List<FinancialTransaction> LoadRecurringList() {
            throw new NotImplementedException();
        }
    }
}
