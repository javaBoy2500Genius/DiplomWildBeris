using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiplomWildBeris.Helpers;
using DiplomWildBeris.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.ViewModels
{
    public partial class AdminMainPageViewModel(IDialogService dialog) : BaseViewModel(dialog)
    {

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangeStateCommand))]
        bool canStateChange;

        private string currentState = StateKey.ItemView;
        public string CurrentState
        {
            get => currentState; set
            {
                currentState = value;
                OnPropertyChanged(nameof(CurrentState));

                if (value == StateKey.ItemView)
                    _ = ServiceHelper.GetService<ItemViewModel>().GetItems();

                else
                    _ = ServiceHelper.GetService<AdminPageViewModel>().GetUsers();


            }
        }
        protected internal void OnAppearing()
        {
            CurrentState = StateKey.ItemView;
        }
        [RelayCommand(CanExecute = nameof(CanStateChange))]
        async void ChangeState(string state)
        {
            if (state == CurrentState ||
                !typeof(StateKey).GetFields().Any(x => x.Name.Equals(state)))
                return;


            CurrentState = state;
        }

        [RelayCommand]
        void LoginExit()
        {
            try
            {
                Application.Current.MainPage = new AppShell();
            }
            catch { }
        }

    }

    public static class StateKey
    {
        public const string Loading = nameof(Loading);
        public const string AdminPage = nameof(AdminPage);
        public const string ItemView = nameof(ItemView);




    }
}
