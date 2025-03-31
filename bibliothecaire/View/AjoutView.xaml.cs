using System.Diagnostics;
using bibliothecaire.ViewModel;

namespace bibliothecaire.View
{
    public partial class AjoutView : ContentPage
    {
        public AjoutView(AjoutViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }


}