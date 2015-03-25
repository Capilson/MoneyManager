﻿#region

using System;
using Windows.UI.Xaml.Data;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation.Model;

#endregion

namespace MoneyManager.Converter {
    public class TransactionStyleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var transaction = value as FinancialTransaction;

            if (transaction == null) return 0;

            return transaction.Cleared
                ? 1
                : 0.7;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}