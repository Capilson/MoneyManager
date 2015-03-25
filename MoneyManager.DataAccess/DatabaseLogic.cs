﻿#region

using MoneyManager.Foundation.Model;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess {
    public class DatabaseLogic {
        public static void CreateDatabase() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                dbConn.CreateTable<Account>();
                dbConn.CreateTable<Category>();
                dbConn.CreateTable<FinancialTransaction>();
                dbConn.CreateTable<RecurringTransaction>();
            }
        }
    }
}