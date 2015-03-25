using System.Collections.Generic;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction> {

        /// <summary>
        /// Save or update the passed transaction
        /// </summary>
        /// <param name="transaction">transaction to save.</param>
        protected override void SaveToDb(FinancialTransaction transaction) {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection()) {
                //if (transaction.Id == 0) {
                //    db.InsertWithChildren(transaction);
                //} else {
                //    db.UpdateWithChildren(transaction);
                //}
                
                db.Insert(transaction);
            }
        }

        /// <summary>
        /// Delete the passed transaction
        /// </summary>
        /// <param name="transaction">transaction to delete</param>
        protected override void DeleteFromDatabase(FinancialTransaction transaction) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                db.Delete(transaction);
            }
        }

        /// <summary>
        /// Loads and returns the transaction list
        /// </summary>
        /// <returns>List of transaction.</returns>
        protected override List<FinancialTransaction> GetListFromDb() {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                return db.Table<FinancialTransaction>().ToList();
            }
        }


        //public IEnumerable<FinancialTransaction> GetUnclearedTransactions() {
        //    return GetUnclearedTransactions(DateTime.Today);
        //}

        //public IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date) {
        //    if (AllTransactions == null) {
        //        LoadList();
        //    }

        //    return AllTransactions.Where(x => x.Cleared == false
        //                                      && x.Date.Date <= date.Date).ToList();
        //}

        //public List<FinancialTransaction> LoadRecurringList() {
        //    return AllTransactions
        //        .Where(x => x.IsRecurring)
        //        .Where(x => x.RecurringTransaction != null)
        //        .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
        //        .ToList();
        //}
    }
}