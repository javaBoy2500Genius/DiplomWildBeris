using CommunityToolkit.Mvvm.ComponentModel;
using DiplomWildBeris.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.ViewModels
{
    public partial class BaseViewModel(IDialogService dialogService) : ObservableObject
    {
        [ObservableProperty]
        protected bool isBusy;

        protected Action? currentDismissAction;


        private readonly IDialogService _dialogService = dialogService;

        partial void OnIsBusyChanged(bool value)
        {
            try
            {

                if (value)
                {
                    currentDismissAction = _dialogService.ShowActivityIndicator();
                }
                else
                {
                    currentDismissAction?.Invoke();
                    currentDismissAction = null;
                }
            }
            catch { }
        }


        public static bool IsNotNull(params string[] values) => !values.Any(string.IsNullOrWhiteSpace);
        public static bool IsNotNull(params object[] values) => !values.Any(x => x == null);

    }
}
