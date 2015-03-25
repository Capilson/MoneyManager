#region

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;
using QKit.JumpList;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class TransactionListViewModel : ViewModelBase {
        private AccountDataAccess accountData {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public string Title {
            get { return accountData.SelectedAccount.Name; }
        }

        private readonly ITransactionRepository _transactionRepository;

        public TransactionListViewModel(ITransactionRepository transactionRepository) {
            _transactionRepository = transactionRepository;
        }

        public List<JumpListGroup<FinancialTransaction>> RelatedTransactions { set; get; }

        public void SetRelatedTransactions(int accountId) {
            IEnumerable<FinancialTransaction> related = _transactionRepository.GetRelatedTransactions(accountId);

            var dateInfo = new DateTimeFormatInfo();
            RelatedTransactions = related.ToGroups(x => x.Date,
                x => dateInfo.GetMonthName(x.Date.Month) + " " + x.Date.Year);

            RelatedTransactions =
                RelatedTransactions.OrderByDescending(x => ((FinancialTransaction) x.First()).Date).ToList();

            foreach (var list in RelatedTransactions) {
                list.Reverse();
            }
        }
    }
}