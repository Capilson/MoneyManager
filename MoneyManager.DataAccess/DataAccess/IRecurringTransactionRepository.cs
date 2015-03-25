using System;
using System.Collections.Generic;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.DataAccess.DataAccess {
    public interface IRecurringTransactionRepository : IRepository<RecurringTransaction> {

        IEnumerable<FinancialTransaction> GetUnclearedTransactions();

        IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date);

        List<FinancialTransaction> LoadRecurringList();
    }
}
