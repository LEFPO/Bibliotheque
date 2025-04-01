namespace bibliothecaire;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        //Initialisation et vérificaton de la base de donnée
        Services.DatabaseInitializer.Initialize();
        
        MainPage = new AppShell();
    }
}