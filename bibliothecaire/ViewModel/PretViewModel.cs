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
    public partial class PretViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty] private ObservableCollection<Pret> prets;
        [ObservableProperty] private Pret pretSelectionne;
        [ObservableProperty] private ObservableCollection<Livre> livres;
        [ObservableProperty] private ObservableCollection<Lecteur> lecteurs;
        [ObservableProperty] private Livre livreSelectionne;
        [ObservableProperty] private Lecteur lecteurSelectionne;
        [ObservableProperty] private DateTime datePret = DateTime.Now;
        [ObservableProperty] private DateTime dateRetour = DateTime.Now.AddDays(14);

        public ICommand AjouterPretCommand { get; }
        public ICommand ModifierPretCommand { get; }
        public ICommand SupprimerPretCommand { get; }

        public PretViewModel()
        {
            _databaseService = new DatabaseService();
            Prets = _databaseService.ObtenirPrets();
            Livres = _databaseService.ObtenirLivres();
            Lecteurs = _databaseService.ObtenirLecteurs();

            AjouterPretCommand = new RelayCommand(AjouterPret);
            ModifierPretCommand = new RelayCommand(ModifierPret);
            SupprimerPretCommand = new RelayCommand(SupprimerPret);
        }

        private void AjouterPret()
        {
            if (LivreSelectionne == null || LecteurSelectionne == null)
            {
                Application.Current.MainPage.DisplayAlert("Erreur", "Sélectionnez un livre et un lecteur.", "OK");
                return;
            }

            var nouveauPret = new Pret(0, LivreSelectionne.IdLivre, LecteurSelectionne.IdLecteur, DatePret, DateRetour);
            _databaseService.AjouterPret(nouveauPret);
            Prets.Add(nouveauPret);
        }

        private void ModifierPret()
        {
            if (PretSelectionne == null)
                return;

            _databaseService.ModifierPret(PretSelectionne);
        }

        private void SupprimerPret()
        {
            if (PretSelectionne == null)
                return;

            _databaseService.SupprimerPret(PretSelectionne.IdPret);
            Prets.Remove(PretSelectionne);
        }
    }
}
