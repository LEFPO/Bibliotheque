using bibliothecaire.Services;
using bibliothecaire.View;

namespace bibliothecaire;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    
        Routing.RegisterRoute(nameof(GestionPretsView), typeof(GestionPretsView));
        Routing.RegisterRoute(nameof(AjoutView), typeof(AjoutView));
        Routing.RegisterRoute(nameof(PopupModifierView), typeof(PopupModifierView));
        
    }

}