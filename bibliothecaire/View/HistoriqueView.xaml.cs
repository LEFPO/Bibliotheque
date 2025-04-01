using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bibliothecaire.ViewModel;

namespace bibliothecaire.View;

public partial class HistoriqueView : ContentPage
{
    public HistoriqueView(HistoriqueViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HistoriqueViewModel vm)
        {
            vm.ChargerHistorique(); // 🔁 recharge automatique à chaque affichage
        }
    }
}