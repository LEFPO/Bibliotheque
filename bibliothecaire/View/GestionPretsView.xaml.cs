using bibliothecaire.ViewModel;

namespace bibliothecaire.View
{
    public partial class GestionPretsView : ContentPage
    {
        private GestionPretsViewModel _viewModel;

        public GestionPretsView()
        {
            InitializeComponent();
            _viewModel = new GestionPretsViewModel();
            BindingContext = _viewModel;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.AppliquerFiltre();
        }
    }
}