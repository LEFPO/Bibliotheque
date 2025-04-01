using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using bibliothecaire.Model;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace bibliothecaire.ViewModel;

public partial class HistoriqueViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Historique> historiqueComplet;
    [ObservableProperty] private ObservableCollection<Historique> historiqueFiltres;
    [ObservableProperty] private string filtreTexte;

    public HistoriqueViewModel(DatabaseService databaseService) : base(databaseService)
    {
        ChargerHistorique();
    }

    public void ChargerHistorique()
    {
        var historiques = _databaseService.ObtenirHistorique();

        HistoriqueComplet = historiques;
        HistoriqueFiltres = new ObservableCollection<Historique>(historiques);
    }

    // 🔁 Réagit à la saisie dans le champ de recherche
    partial void OnFiltreTexteChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            HistoriqueFiltres = new ObservableCollection<Historique>(HistoriqueComplet);
        }
        else
        {
            var filtre = value.ToLower();

            var resultats = HistoriqueComplet.Where(h =>
                (h.TitreLivre?.ToLower().Contains(filtre) ?? false) ||
                (h.NomLecteur?.ToLower().Contains(filtre) ?? false)).ToList();

            HistoriqueFiltres = new ObservableCollection<Historique>(resultats);
        }
    }

    [RelayCommand]
    private async Task RetourAsync()
    {
        await Shell.Current.GoToAsync("//GestionPretsView");
    }
}