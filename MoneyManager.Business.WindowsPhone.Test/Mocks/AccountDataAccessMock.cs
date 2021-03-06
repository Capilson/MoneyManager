﻿using System.Collections.Generic;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.Mocks {
    public class AccountDataAccessMock : IDataAccess<Account> {
        public List<Account> AccountTestList = new List<Account>();

        public void Save(Account itemToSave) {
            AccountTestList.Add(itemToSave);
        }

        public void Delete(Account item) {
            if (AccountTestList.Contains(item)) {
                AccountTestList.Remove(item);
            }
        }

        public List<Account> LoadList() {
            return new List<Account>();
        }
    }
}