using DiplomWildBeris.Helpers;
using DiplomWildBeris.ViewModels;

namespace DiplomWildBeris.View;

public partial class ItemView : ContentView
{
    private readonly ItemViewModel? vm;
    public ItemView()
    {
        InitializeComponent();
        BindingContext = vm = ServiceHelper.GetService<ItemViewModel>();
    }

  
  
}