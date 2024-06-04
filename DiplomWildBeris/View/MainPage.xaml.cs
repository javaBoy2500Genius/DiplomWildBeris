using DiplomWildBeris.Helpers;
using DiplomWildBeris.ViewModels;

namespace DiplomWildBeris.View
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel? vm;

        public MainPage(MainPageViewModel _vm)
        {
            InitializeComponent();
            BindingContext = vm = _vm;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = vm?.GetItems();
        }


    }

}
