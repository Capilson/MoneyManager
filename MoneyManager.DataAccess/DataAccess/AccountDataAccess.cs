#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account> {
        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        protected override void SaveToDb(Account itemToAdd) {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
                if (AllAccounts == null) {
                    AllAccounts = new ObservableCollection<Account>();
                }

                AllAccounts.Add(itemToAdd);
                AllAccounts = new ObservableCollection<Account>(AllAccounts.OrderBy(x => x.Name));
                dbConn.Insert(itemToAdd);
            }
        }

        protected override void DeleteFromDatabase(Account itemToDelete) {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
                AllAccounts.Remove(itemToDelete);
                dbConn.Delete(itemToDelete);
            }
        }

        protected override List<Account> GetListFromDb() {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
                return dbConn.Table<Account>()
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }
    }
}