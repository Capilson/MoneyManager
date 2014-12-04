﻿#region

using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.Business.Helper
{
    public class Utilities
    {
        public static string GetVersion()
        {
            return Assembly.Load(new AssemblyName("MoneyManager.WindowsPhone")).FullName.Split('=')[1].Split(',')[0];
        }

        public static async Task<bool> IsDeletionConfirmed()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("DeleteEntryQuestionMessage"),
                Translation.GetTranslation("DeleteQuestionTitle"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            var result = await dialog.ShowAsync();

            return result.Label == Translation.GetTranslation("YesLabel");
        }

        public static DateTime GetEndOfMonth()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        }

        public async static void SendMail(string recipient, string subject, string body)
        {
            await Launcher.LaunchUriAsync(
                new Uri(
                    String.Format("mailto:{0}?subject={1}&body={2}", recipient,
                        subject, body)));
        }
    }
}