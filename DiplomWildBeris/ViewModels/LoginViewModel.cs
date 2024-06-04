using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiplomWildBeris.Models;
using DiplomWildBeris.Services;
using DiplomWildBeris.View;
using Realms;


namespace DiplomWildBeris.ViewModels
{
    public partial class LoginViewModel(IDialogService dialogService, IUserService userServices, IRealmService<User> realmUsers, ISeed seed) : BaseViewModel(dialogService)
    {
        private readonly IDialogService _dialogService = dialogService;
        private readonly IUserService _userServices = userServices;
        private readonly IRealmService<User> _realmUsers = realmUsers;
        private readonly ISeed _seed = seed;
        [ObservableProperty]
        string login = "", password = "";


        protected internal async Task OnAppearing()
        {
            Login = Preferences.Get(nameof(Login), "");
            Password = Preferences.Get(nameof(Password), "");

            if (!(await _realmUsers.GetItemsAsync()).Any())
            {
                _ = _seed.SeedData();
            }

        }


        [RelayCommand]
        async Task Authorize()
        {
            try
            {
                IsBusy = true;
                if (!IsNotNull(Password, Login)) throw new Exception($"{nameof(Password)}{nameof(Login)}empty");
                var user = (await _realmUsers.GetItemsAsync(x => x.Login == Login)).FirstOrDefault() ?? throw new Exception($"user login not found");

                if (_userServices.Decrypt(user.Password) != Password)
                    throw new Exception("user passwod not equal");

                Preferences.Set(nameof(Login), Login);
                Preferences.Set(nameof(Password), Password);

                if (user.Role == UserRole.User)
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                else
                    await Shell.Current.GoToAsync($"//{nameof(AdminMainPage)}");
            }
            catch (Exception ex)
            {
                IsBusy = false;
                switch (ex.Message)
                {
                    case string msg when msg.Contains($"{nameof(Password)}{nameof(Login)}empty"):
                        await _dialogService.ShowAlertAsync("Ошибка", "Заполните все поля", "ОК");
                        break;
                    case string msg when msg.Contains($"uncorrect{nameof(Login)}"):
                        await _dialogService.ShowAlertAsync("Ошибка", "Пользователь не найден", "ОК");
                        break;
                    case string msg when msg.Contains("user login not found"):
                        await _dialogService.ShowAlertAsync("Ошибка", "Неверное имя пользователя", "ОК");
                        break;
                    case string msg when msg.Contains("user passwod not equal"):
                        await _dialogService.ShowAlertAsync("Ошибка", "Неверный пароль", "ОК");
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
    }
}
