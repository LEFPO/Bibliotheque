using bibliothecaire.ViewModel;

namespace bibliothecaire.View;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }
}