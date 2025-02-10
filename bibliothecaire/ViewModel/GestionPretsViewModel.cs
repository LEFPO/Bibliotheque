using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using bibliothecaire.Model;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace bibliothecaire.ViewModel
{
    public partial class GestionPretsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string rechercheTexte = string.Empty;

        [ObservableProperty]
        private bool afficherLivres = true;

        [ObservableProperty]
        private bool afficherLecteurs = false;

        public ObservableCollection<Livre> Livres { get; set; }
        public ObservableCollection<Lecteur> Lecteurs { get; set; }
        public ObservableCollection<Livre> LivresFiltres { get; set; }
        public ObservableCollection<Lecteur> LecteursFiltres { get; set; }

        public ICommand AfficherLivresCommand { get; }
        public ICommand AfficherLecteursCommand { get; }

        public GestionPretsViewModel()
        {
            _databaseService = new DatabaseService();

            Livres = _databaseService.ObtenirLivres();
            Lecteurs = _databaseService.ObtenirLecteurs();

            LivresFiltres = new ObservableCollection<Livre>(Livres);
            LecteursFiltres = new ObservableCollection<Lecteur>(Lecteurs);

            AfficherLivresCommand = new RelayCommand(() => SetAfficherLivres(true));
            AfficherLecteursCommand = new RelayCommand(() => SetAfficherLecteurs(true));
        }

        public void SetAfficherLivres(bool actif)
        {
            if (actif)
            {
                afficherLivres = true;
                afficherLecteurs = false;
                RechercheTexte = string.Empty;
                AppliquerFiltre();
                OnPropertyChanged(nameof(AfficherLivres));
                OnPropertyChanged(nameof(AfficherLecteurs));
            }
        }

        public void SetAfficherLecteurs(bool actif)
        {
            if (actif)
            {
                afficherLivres = false;
                afficherLecteurs = true;
                RechercheTexte = string.Empty;
                AppliquerFiltre();
                OnPropertyChanged(nameof(AfficherLivres));
                OnPropertyChanged(nameof(AfficherLecteurs));
            }
        }

        public void AppliquerFiltre()
        {
            if (AfficherLivres)
            {
                LivresFiltres.Clear();
                foreach (var livre in Livres.Where(l => l.Titre.Contains(RechercheTexte, StringComparison.OrdinalIgnoreCase)))
                {
                    LivresFiltres.Add(livre);
                }
            }
            else if (AfficherLecteurs)
            {
                LecteursFiltres.Clear();
                foreach (var lecteur in Lecteurs.Where(l => l.Nom.Contains(RechercheTexte, StringComparison.OrdinalIgnoreCase) || l.Prenom.Contains(RechercheTexte, StringComparison.OrdinalIgnoreCase)))
                {
                    LecteursFiltres.Add(lecteur);
                }
            }
        }
        
        [RelayCommand]
        public void GoToBack()
        {
            Shell.Current.GoToAsync("//LoginView");
        }
    }
}
