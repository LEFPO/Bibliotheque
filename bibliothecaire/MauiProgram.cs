using bibliothecaire.ViewModel;
using bibliothecaire.View;
using bibliothecaire.Services;
using Microsoft.Extensions.Logging;

namespace bibliothecaire;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ✅ Enregistrer les services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<AppShell>();

        // ✅ Enregistrer les ViewModel
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<GestionPretsViewModel>();
        builder.Services.AddTransient<AjoutViewModel>();
        builder.Services.AddTransient<PopupModifierViewModel>();

        // ✅ Enregistrer les Vues
        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<GestionPretsView>();
        builder.Services.AddTransient<AjoutView>();
        builder.Services.AddTransient<PopupModifierView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

}