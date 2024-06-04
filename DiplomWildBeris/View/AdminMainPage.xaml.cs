using DiplomWildBeris.ViewModels;

namespace DiplomWildBeris.View;

public partial class AdminMainPage : ContentPage
{
    private readonly AdminMainPageViewModel vm;
    public AdminMainPage(AdminMainPageViewModel _vm)
    {
        InitializeComponent();
        BindingContext = vm = _vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        vm.OnAppearing();
    }
}