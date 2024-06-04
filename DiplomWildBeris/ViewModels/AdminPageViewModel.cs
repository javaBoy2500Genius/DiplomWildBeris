using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiplomWildBeris.Models;
using DiplomWildBeris.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.ViewModels
{
    public partial class AdminPageViewModel(IDialogService dialogService, IUserService userService, IRealmService<User> realmService) : BaseViewModel(dialogService)
    {
        private readonly IDialogService _dialogService = dialogService;
        private readonly IUserService _userServices = userService;
        private readonly IRealmService<User> _realmUsers = realmService;

        [ObservableProperty]
        string loginReg = "", passwordReg = "";

        [ObservableProperty]
        bool isOpenCreateUser;

        [ObservableProperty]
        IQueryable<User>? users;

        [RelayCommand]
        async Task RegisterUser()
        {
            try
            {
                IsBusy = true;
                if (!IsNotNull(PasswordReg, LoginReg)) throw new Exception($"{nameof(PasswordReg)}{nameof(LoginReg)}empty");
                var user = (await _realmUsers.GetItemsAsync(x => x.Login == LoginReg)).FirstOrDefault();
                if (user != null) throw new Exception($"user login not found");

                await _realmUsers.AddItemAsync(new User() { Login = LoginReg, Password = _userServices.Encrypt(PasswordReg), Role = UserRole.User });

                await _dialogService.ShowAlertAsync("", "Пользователь успешно создан", "ОК");
                _ = GetUsers();
                OpenPopup();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                switch (ex.Message)
                {
                    case string msg when msg.Contains($"{nameof(PasswordReg)}{nameof(LoginReg)}empty"):
                        await _dialogService.ShowAlertAsync("Ошибка", "Заполните все поля", "ОК");
                        break;

                    case string msg when msg.Contains("user login  found"):
                        await _dialogService.ShowAlertAsync("Ошибка", "Пользователь с таким логином сущетсвует", "ОК");
                        break;

                    default:
                        await _dialogService.ShowAlertAsync("Ошибка в обработке запроса", ex.Message, "ОК");
                        break;

                }

            }
            finally
            {
                IsBusy = false;
            }

        }

        protected internal async Task GetUsers()
        {
            Users = (await _realmUsers.GetItemsAsync(x => x.RoleRaw != (int)UserRole.Admin));

        }

        [RelayCommand]
        void OpenPopup()
        {
            try
            {
                IsOpenCreateUser = !IsOpenCreateUser;
                if (!IsOpenCreateUser)
                {
                    LoginReg = string.Empty;
                    PasswordReg = string.Empty;
                }
            }
            catch { }

        }
    }
}
