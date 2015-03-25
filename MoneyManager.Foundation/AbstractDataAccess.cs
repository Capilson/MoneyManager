#region

using System;
using System.Collections.Generic;
using MoneyManager.Foundation.OperationContracts;
using Xamarin;

#endregion

namespace MoneyManager.Foundation {
    public abstract class AbstractDataAccess<T> : IDataAccess<T> {

        /// <summary>
        /// Save or update the passed item on the database
        /// </summary>
        /// <param name="itemToSave">item to save</param>
        public void Save(T itemToSave) {
            try {
                SaveToDb(itemToSave);
            } catch (Exception ex) {
                Insights.Report(ex);
            }
        }

        /// <summary>
        /// Delete the passed item from the database
        /// </summary>
        /// <param name="itemToDelete">item to delete</param>
        public void Delete(T itemToDelete) {
            try {
                DeleteFromDatabase(itemToDelete);
            } catch (Exception ex) {
                Insights.Report(ex);
            }
        }

        /// <summary>
        /// Loads and returns a table
        /// </summary>
        /// <returns>list of items</returns>
        public List<T> LoadList() {
            try {
                return GetListFromDb();
            } catch (Exception ex) {
                Insights.Report(ex);
            }
            return new List<T>();
        }

        protected abstract void SaveToDb(T itemToAdd);

        protected abstract void DeleteFromDatabase(T itemToDelete);

        protected abstract List<T> GetListFromDb();
    }
}