using System.Collections.ObjectModel;
using System.Windows.Input;
using bibliothecaire.Model;
using bibliothecaire.Services;
using bibliothecaire.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;

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

        [ObservableProperty] private ObservableCollection<Livre> livres = new();
        [ObservableProperty] private ObservableCollection<Lecteur> lecteurs = new();
        [ObservableProperty] private ObservableCollection<Livre> livresFiltres = new();
        [ObservableProperty] private ObservableCollection<Lecteur> lecteursFiltres = new();

        public ICommand AfficherLivresCommand { get; }
        public ICommand AfficherLecteursCommand { get; }
        public ICommand AjouterCommand { get; }
        public ICommand ModifierCommand { get; }
        public ICommand SupprimerCommand { get; }
        
        public ICommand FairePretCommand { get; }

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
            FairePretCommand = new RelayCommand(FairePret);
        }

        public void ChargerDonnees()
        {
            Livres.Clear();
            foreach (var livre in _databaseService.ObtenirLivres())
            {
                Livres.Add(livre);
            }

            LivresFiltres.Clear();
            foreach (var livre in Livres)
            {
                LivresFiltres.Add(livre);
            }

            Lecteurs.Clear();
            foreach (var lecteur in _databaseService.ObtenirLecteurs())
            {
                Lecteurs.Add(lecteur);
            }

            LecteursFiltres.Clear();
            foreach (var lecteur in Lecteurs)
            {
                LecteursFiltres.Add(lecteur);
            }
        }

        private async void Ajouter()
        {
            await Shell.Current.GoToAsync(nameof(AjoutView));
        }




        private void Modifier()
        {
            if (AfficherLivres && LivreSelectionne != null)
            {
                var popup = new PopupModifierView();
                popup.BindingContext = new PopupModifierViewModel(_databaseService, LivreSelectionne, () =>
                {
                    popup.Close();
                });

                Application.Current.MainPage.ShowPopup(popup);
            }
            else if (AfficherLecteurs && LecteurSelectionne != null)
            {
                var popup = new PopupModifierView();
                popup.BindingContext = new PopupModifierViewModel(_databaseService, LecteurSelectionne, () =>
                {
                    popup.Close();
                });

                Application.Current.MainPage.ShowPopup(popup);
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erreur", "Aucun élément sélectionné.", "OK");
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
        
        private async void FairePret()
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert("Test", "Le bouton fonctionne bien !", "OK");
                await Shell.Current.GoToAsync(nameof(PretView));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"Exception : {ex.Message}", "OK");
            }
        }

    }
}
