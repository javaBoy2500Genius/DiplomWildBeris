using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using DiplomWildBeris.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.Services
{
    public interface IDialogService
    {
        Task ShowToast(string text);
        Task ShowToast(string text, ToastDuration t);
        Task ShowAlertAsync(string title, string message, string accept);
        Task<bool> ShowConfirmAlertAsync(string title, string message, string accept, string dismiss);
        Action ShowActivityIndicator();
    }
    public class DialogService : IDialogService
    {
        public Task ShowToast(string text) => Toast.Make(text, ToastDuration.Short).Show();
        public Task ShowToast(string text, ToastDuration t) => Toast.Make(text, t).Show();

        public async Task ShowAlertAsync(string title, string message, string accept) => await Application.Current.MainPage.DisplayAlert(title, message, accept);


        public async Task<bool> ShowConfirmAlertAsync(string title, string message, string accept, string dismiss) => await Application.Current.MainPage.DisplayAlert(title, message, accept, dismiss);
        public Action ShowActivityIndicator()
        {
            var popup = new BusyPopup();
            Application.Current.MainPage.ShowPopup(popup);
            return () => popup.Close();
        }
    }
}
