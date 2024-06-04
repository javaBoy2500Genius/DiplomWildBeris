using DiplomWildBeris.Helpers;
using DiplomWildBeris.ViewModels;

namespace DiplomWildBeris.View;

public partial class AdminPage : ContentView
{
    private readonly AdminPageViewModel? vm;
    public AdminPage()
    {
        InitializeComponent();
        BindingContext = vm = ServiceHelper.GetService<AdminPageViewModel>()!;
    }
    

    void SwichPassVisible(Object sender, System.EventArgs e)
    {
        PassEntry.IsPassword = !PassEntry.IsPassword;
    }
}