using bibliothecaire.ViewModel;
using bibliothecaire.View;
using bibliothecaire.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace bibliothecaire;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()//Pour pouvoir entre autre utiliser la popup pour pouvoir modifier un lecteur ou un livre
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ✅ Enregistrer les services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<GestionPretsViewModel>();

        // AjoutViewModel utilisant IServiceProvider
        builder.Services.AddTransient<AjoutViewModel>(s =>
            new AjoutViewModel(
                s.GetRequiredService<DatabaseService>(),
                s
            ));



        // ✅ Enregistrer les ViewModel
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<GestionPretsViewModel>();
        builder.Services.AddTransient<AjoutViewModel>();
        builder.Services.AddTransient<PopupModifierViewModel>();
        builder.Services.AddTransient<PretViewModel>();
        builder.Services.AddTransient<CreerBibliothecaireViewModel>();
        builder.Services.AddTransient<HistoriqueViewModel>();


        // ✅ Enregistrer les Vues
        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<GestionPretsView>();
        builder.Services.AddTransient<AjoutView>();
        builder.Services.AddTransient<PopupModifierView>();
        builder.Services.AddTransient<PretView>();
        builder.Services.AddTransient<CreerBibliothecaireView>();
        builder.Services.AddTransient<HistoriqueView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

}