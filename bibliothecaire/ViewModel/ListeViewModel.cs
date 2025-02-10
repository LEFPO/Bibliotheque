using System.Collections.ObjectModel;
using bibliothecaire.Model;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace bibliothecaire.ViewModel
{
    public partial class ListeViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Livre> listeLivres;

        [ObservableProperty]
        private ObservableCollection<Lecteur> listeLecteurs;

        public ListeViewModel()
        {
            _databaseService = new DatabaseService();
            ListeLivres = _databaseService.ObtenirLivres();
            ListeLecteurs = _databaseService.ObtenirLecteurs();
        }
    }
}