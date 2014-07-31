﻿using MoneyManager.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MoneyManager.UserControls
{
    public sealed partial class SettingsOverviewUserControl
    {
        public SettingsOverviewUserControl()
        {
            InitializeComponent();
        }

        private void CategoryTap(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(SettingsCategory));
        }
    }
}