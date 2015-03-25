using System.Collections.Generic;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.OperationContracts {
    public interface ITransactionRepository : IRepository<FinancialTransaction> {
        IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId);
    }
}
