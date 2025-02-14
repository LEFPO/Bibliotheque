using System.Diagnostics;
using bibliothecaire.ViewModel;

namespace bibliothecaire.View
{
    public partial class AjoutView : ContentPage
    {
        public AjoutView(AjoutViewModel viewModel)
        {
            Debug.WriteLine("🔹 Chargement de AjoutView...");
            InitializeComponent();
            Debug.WriteLine("✅ AjoutView chargé avec succès !");
            BindingContext = viewModel; // ✅ Injection correcte
        }
    }
}