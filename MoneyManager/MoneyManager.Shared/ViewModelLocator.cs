#region

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Views;

#endregion

namespace MoneyManager {
    public class ViewModelLocator {
        static ViewModelLocator() {
            
            DatabaseLogic.CreateDatabase();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //Logic
            SimpleIoc.Default.Register<IUserNotification, UserNotification>();
            SimpleIoc.Default.Register(CreateNavigationService);
            SimpleIoc.Default.Register<Utilities>();

            //DataAccess
            SimpleIoc.Default.Register<AccountDataAccess>();
            SimpleIoc.Default.Register<CategoryDataAccess>();
            SimpleIoc.Default.Register<TransactionDataAccess>();
            SimpleIoc.Default.Register<RecurringTransactionDataAccess>();
            SimpleIoc.Default.Register<SettingDataAccess>();

            //Repositories
            SimpleIoc.Default.Register<IRecurringTransactionRepository, RecurringTransactionRepository>();
            SimpleIoc.Default.Register<ITransactionRepository, TransactionRepository>();

            //ViewModels
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<BalanceViewModel>();
            SimpleIoc.Default.Register<CategoryListViewModel>();
            SimpleIoc.Default.Register<TransactionListViewModel>();
            SimpleIoc.Default.Register<TileSettingsViewModel>();
            SimpleIoc.Default.Register<GeneralSettingViewModel>();
            SimpleIoc.Default.Register<SettingDefaultsViewModel>();
            SimpleIoc.Default.Register<SelectCurrencyViewModel>();
            SimpleIoc.Default.Register<StatisticViewModel>();
            SimpleIoc.Default.Register<BackupViewModel>();
        }

        #region DataAccess

        public AccountDataAccess AccountDataAccess {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public CategoryDataAccess CategoryDataAccess {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        public TransactionDataAccess TransactionDataAccess {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public RecurringTransactionDataAccess RecurringTransactionDataAccess {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        public SettingDataAccess SettingDataAccess {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        #endregion DataAccess

        #region Logic

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("LicenseView", typeof(LicenseView));

            return navigationService;
        }

        #endregion

        #region Views

        public AddAccountViewModel AddAccountView {
            get { return ServiceLocator.Current.GetInstance<AddAccountViewModel>(); }
        }

        public AddTransactionViewModel AddTransactionView {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public BalanceViewModel BalanceView {
            get { return ServiceLocator.Current.GetInstance<BalanceViewModel>(); }
        }

        public CategoryListViewModel CategoryListView {
            get { return ServiceLocator.Current.GetInstance<CategoryListViewModel>(); }
        }

        public TransactionListViewModel TransactionListView {
            get { return ServiceLocator.Current.GetInstance<TransactionListViewModel>(); }
        }

        public TileSettingsViewModel TileSettingsView {
            get { return ServiceLocator.Current.GetInstance<TileSettingsViewModel>(); }
        }

        public GeneralSettingViewModel GeneralSettingView {
            get { return ServiceLocator.Current.GetInstance<GeneralSettingViewModel>(); }
        }

        public SettingDefaultsViewModel SettingDefaultsView {
            get { return ServiceLocator.Current.GetInstance<SettingDefaultsViewModel>(); }
        }

        public SelectCurrencyViewModel SelectCurrencyView {
            get { return ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>(); }
        }

        public StatisticViewModel StatisticView {
            get { return ServiceLocator.Current.GetInstance<StatisticViewModel>(); }
        }

        public BackupViewModel BackupView {
            get { return ServiceLocator.Current.GetInstance<BackupViewModel>(); }
        }

        #endregion Views
    }
}