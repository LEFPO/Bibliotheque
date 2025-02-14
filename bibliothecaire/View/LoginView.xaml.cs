using bibliothecaire.ViewModel;

namespace bibliothecaire.View;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; 
    }
}

