using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using bibliothecaire.Model;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace bibliothecaire.ViewModel
{
    public partial class GestionPretsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty] private string rechercheTexte = string.Empty;
        [ObservableProperty] private bool afficherLivres = true;
        [ObservableProperty] private bool afficherLecteurs = false;

        [ObservableProperty] private Livre livreSelectionne;
        [ObservableProperty] private Lecteur lecteurSelectionne;

        public ObservableCollection<Livre> Livres { get; set; } = new();
        public ObservableCollection<Lecteur> Lecteurs { get; set; } = new();
        public ObservableCollection<Livre> LivresFiltres { get; set; } = new();
        public ObservableCollection<Lecteur> LecteursFiltres { get; set; } = new();

        public ICommand AfficherLivresCommand { get; }
        public ICommand AfficherLecteursCommand { get; }
        public ICommand AjouterCommand { get; }
        public ICommand ModifierCommand { get; }
        public ICommand SupprimerCommand { get; }

        // ✅ Correction : Injection de DatabaseService
        public GestionPretsViewModel(DatabaseService databaseService) : base(databaseService)
        {
            _databaseService = databaseService;

            ChargerDonnees();

            AfficherLivresCommand = new RelayCommand(() => SetAfficherLivres(true));
            AfficherLecteursCommand = new RelayCommand(() => SetAfficherLecteurs(true));
            AjouterCommand = new RelayCommand(Ajouter);
            ModifierCommand = new RelayCommand(Modifier);
            SupprimerCommand = new RelayCommand(Supprimer);
        }

        private void ChargerDonnees()
        {
            Livres = _databaseService.ObtenirLivres();
            Lecteurs = _databaseService.ObtenirLecteurs();

            LivresFiltres = new ObservableCollection<Livre>(Livres);
            LecteursFiltres = new ObservableCollection<Lecteur>(Lecteurs);
        }

        private async void Ajouter()
        {
            await Shell.Current.GoToAsync(nameof(View.AjoutView));
        }

        private async void Modifier()
        {
            if (AfficherLivres && LivreSelectionne != null)
            {
                var param = new Dictionary<string, object> { { "LivreSelectionne", LivreSelectionne } };
                await Shell.Current.GoToAsync(nameof(View.PopupModifierView), param);
            }
            else if (AfficherLecteurs && LecteurSelectionne != null)
            {
                var param = new Dictionary<string, object> { { "LecteurSelectionne", LecteurSelectionne } };
                await Shell.Current.GoToAsync(nameof(View.PopupModifierView), param);
            }
        }

        private void Supprimer()
        {
            if (AfficherLivres && LivreSelectionne != null)
            {
                bool success = _databaseService.SupprimerLivre(LivreSelectionne.IdLivre);
                if (success)
                {
                    Livres.Remove(LivreSelectionne);
                    LivresFiltres.Remove(LivreSelectionne);
                    LivreSelectionne = null;
                }
            }
            else if (AfficherLecteurs && LecteurSelectionne != null)
            {
                bool success = _databaseService.SupprimerLecteur(LecteurSelectionne.IdLecteur);
                if (success)
                {
                    Lecteurs.Remove(LecteurSelectionne);
                    LecteursFiltres.Remove(LecteurSelectionne);
                    LecteurSelectionne = null;
                }
            }
        }

        public void SetAfficherLivres(bool actif)
        {
            if (actif)
            {
                AfficherLivres = true;
                AfficherLecteurs = false;
                RechercheTexte = string.Empty;
                AppliquerFiltre();
            }
        }

        public void SetAfficherLecteurs(bool actif)
        {
            if (actif)
            {
                AfficherLivres = false;
                AfficherLecteurs = true;
                RechercheTexte = string.Empty;
                AppliquerFiltre();
            }
        }

        public void AppliquerFiltre()
        {
            if (AfficherLivres)
            {
                LivresFiltres.Clear();
                foreach (var livre in Livres.Where(l =>
                             l.Titre.Contains(RechercheTexte, StringComparison.OrdinalIgnoreCase)))
                {
                    LivresFiltres.Add(livre);
                }
            }
            else if (AfficherLecteurs)
            {
                LecteursFiltres.Clear();
                foreach (var lecteur in Lecteurs.Where(l =>
                             l.Nom.Contains(RechercheTexte, StringComparison.OrdinalIgnoreCase) ||
                             l.Prenom.Contains(RechercheTexte, StringComparison.OrdinalIgnoreCase)))
                {
                    LecteursFiltres.Add(lecteur);
                }
            }
        }
    }
}
