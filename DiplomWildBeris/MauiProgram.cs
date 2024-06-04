using CommunityToolkit.Maui;
using DiplomWildBeris.Models;
using DiplomWildBeris.Services;
using DiplomWildBeris.View;
using DiplomWildBeris.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Realms;
using System.Reflection;

namespace DiplomWildBeris
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                 .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("FontAwesome6BrandsRegular400.otf", "FAB");
                    fonts.AddFont("FontAwesome6FreeSolid900.otf", "FAS");
                    fonts.AddFont("FontAwesome6FreeRegular400.otf", "FAR");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IUserService, UserServices>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton(typeof(IRealmService<>), typeof(RealmService<>));
            builder.Services.AddSingleton<ISeed, RealmService<RealmObject>>();

            builder.Services.AddScoped<LoginPage, LoginViewModel>();
            builder.Services.AddScoped<AdminPage, AdminPageViewModel>();
            builder.Services.AddScoped<ItemView, ItemViewModel>();
            builder.Services.AddScoped<MainPage, MainPageViewModel>();
            builder.Services.AddScoped<AdminMainPage, AdminMainPageViewModel>();
            //var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DiplomWildBeris.appsettings.json")!;

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();


            builder.Configuration.AddConfiguration(config);



            return builder.Build();
        }
        public static IServiceProvider Services { get; private set; }
    }

    //public static IServiceProvider Services { get; private set; }

    public class UserSetting
    {
        public string Key { get; set; } = null!;

    }
}
