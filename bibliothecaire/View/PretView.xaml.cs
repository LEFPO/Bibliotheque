using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bibliothecaire.ViewModel;

namespace bibliothecaire.View;

public partial class PretView : ContentPage
{
    public PretView(PretViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}