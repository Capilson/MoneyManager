#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class CategoryDataAccess : AbstractDataAccess<Category> {
        public Category SelectedCategory { get; set; }
        public ObservableCollection<Category> AllCategories { get; set; }

        protected override void SaveToDb(Category category) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (AllCategories == null) {
                    LoadList();
                }

                AllCategories.Add(category);
                AllCategories = new ObservableCollection<Category>(AllCategories.OrderBy(x => x.Name));
                db.Insert(category);
            }
        }

        protected override void DeleteFromDatabase(Category category) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (AllCategories != null) {
                    AllCategories.Remove(category);
                }
                db.Delete(category);
            }
        }

        protected override List<Category> GetListFromDb() {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                return db.Table<Category>()
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }
    }
}