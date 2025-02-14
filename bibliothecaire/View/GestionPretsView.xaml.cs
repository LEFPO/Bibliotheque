using bibliothecaire.ViewModel;
using Microsoft.Maui.Controls;

namespace bibliothecaire.View
{
    public partial class GestionPretsView : ContentPage
    {
        private GestionPretsViewModel _viewModel;

        public GestionPretsView(GestionPretsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.AppliquerFiltre();
        }
    }
}