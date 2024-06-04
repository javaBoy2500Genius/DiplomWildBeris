namespace DiplomWildBeris.View;
using DiplomWildBeris.ViewModels;
public partial class LoginPage : ContentPage
{

    private readonly LoginViewModel _vm;
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();

        BindingContext = _vm = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.OnAppearing();
    }
    void SwichPassVisible(Object sender, System.EventArgs e)
    {
        PassEntry.IsPassword = !PassEntry.IsPassword;
    }
}