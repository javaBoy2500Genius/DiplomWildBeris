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
    public partial class ItemViewModel(IDialogService dialogService, IRealmService<Item> realmService) : BaseViewModel(dialogService)
    {
        private readonly IDialogService _dialogService = dialogService;
        private readonly IRealmService<Item> _realmItems = realmService;

        [ObservableProperty]
        string title = "", description = "";

        [ObservableProperty]
        bool isOpenCreateItem;

        [ObservableProperty]
        IQueryable<Item>? items;

        [RelayCommand]
        async Task CreateItem()
        {
            try
            {
                IsBusy = true;

                if (!IsNotNull(Description, Title)) throw new Exception($"{nameof(Description)}{nameof(Title)}empty");

                await _realmItems.AddItemAsync(new ()
                {
                    Title = Title,
                    Topic = new ()
                    {
                        TopicDescription = Description
                    }
                });

                await _dialogService.ShowAlertAsync("", "Вопрос успешно создан", "ОК");
                _ = GetItems();
                OpenPopup();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                switch (ex.Message)
                {
                    case string msg when msg.Contains($"{nameof(Description)}{nameof(Title)}empty"):
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

        protected internal async Task GetItems()
        {
            Items = (await _realmItems.GetItemsAsync());

        }

        [RelayCommand]
        void OpenPopup()
        {
            try
            {
                IsOpenCreateItem = !IsOpenCreateItem;
                if (!IsOpenCreateItem)
                {
                    Title = string.Empty;
                    Description = string.Empty;
                }
            }
            catch { }

        }
    }
}
