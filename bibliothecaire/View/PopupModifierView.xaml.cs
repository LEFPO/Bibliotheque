using bibliothecaire.ViewModel;
using Microsoft.Maui.Controls;

namespace bibliothecaire.View
{
    public partial class PopupModifierView : ContentPage
    {
        public PopupModifierView(PopupModifierViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}