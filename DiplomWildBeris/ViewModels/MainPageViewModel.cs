using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiplomWildBeris.Models;
using DiplomWildBeris.Services;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.ViewModels
{
    public partial class MainPageViewModel(IRealmService<Item> realmServiceItems, IDialogService dialogService) : BaseViewModel(dialogService)
    {
        private readonly IRealmService<Item> _realmServiceItems = realmServiceItems;
        private static readonly char[] splitArray = [' '];

        IQueryable<Item>? allItems;

        [ObservableProperty]
        IQueryable<Item>? items;

        [ObservableProperty]
        string search = string.Empty;



        partial void OnSearchChanged(string value) => Searching();


        protected internal async Task GetItems()
        {
            allItems = await _realmServiceItems.GetItemsAsync();
            Items = allItems;
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

        protected void Searching()
        {
            if (!IsNotNull(allItems))
                return;


            if (!IsNotNull(Search))
            {
                Items = allItems;
                return;
            }
            var mainWords = Search.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);

            if (!IsNotNull(mainWords) || mainWords.Length < 1)
            {
                Items = allItems;
                return;
            }

            Items =
                 allItems.AsEnumerable().Where(x =>
                     mainWords.FirstOrDefault(
                                keyword =>
                                (
                                     x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                     ||
                                     (IsNotNull(x.Topic) && IsNotNull(x.Topic.TopicName) && x.Topic.TopicName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                     ||
                                     (IsNotNull(x.Topic) && IsNotNull(x.Topic.TopicDescription) && x.Topic.TopicDescription.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                )) != null

             ).AsQueryable();




        }


    }
}
