using System;
using System.Threading.Tasks;
using System.Windows.Input;
using bibliothecaire.Model;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace bibliothecaire.ViewModel
{
    public partial class PopupModifierViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly Action _fermerPopup;

        [ObservableProperty] private string nomOuTitre;
        [ObservableProperty] private string auteurOuPrenom;
        [ObservableProperty] private string genreOuTelephone;

        private readonly Livre _livre;
        private readonly Lecteur _lecteur;

        public PopupModifierViewModel(DatabaseService databaseService, Livre livre, Action fermerPopup)
        {
            _databaseService = databaseService;
            _fermerPopup = fermerPopup;
            _livre = livre;

            NomOuTitre = _livre.Titre;
            AuteurOuPrenom = _livre.Auteur;
            GenreOuTelephone = _livre.Genre;

            EnregistrerCommand = new AsyncRelayCommand(ModifierLivreAsync);
            FermerCommand = new RelayCommand(Fermer);
        }

        public PopupModifierViewModel(DatabaseService databaseService, Lecteur lecteur, Action fermerPopup)
        {
            _databaseService = databaseService;
            _fermerPopup = fermerPopup;
            _lecteur = lecteur;

            NomOuTitre = _lecteur.Nom;
            AuteurOuPrenom = _lecteur.Prenom;
            GenreOuTelephone = _lecteur.Telephone;

            EnregistrerCommand = new AsyncRelayCommand(ModifierLecteurAsync);
            FermerCommand = new RelayCommand(Fermer);
        }

        public ICommand EnregistrerCommand { get; }
        public ICommand FermerCommand { get; }

        private async Task ModifierLivreAsync()
        {
            try
            {
                if (_livre != null)
                {
                    _livre.Titre = NomOuTitre;
                    _livre.Auteur = AuteurOuPrenom;
                    _livre.Genre = GenreOuTelephone;

                    bool success = _databaseService.ModifierLivre(_livre);
                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Succès", "Livre modifié avec succès !", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible de modifier le livre.", "OK");
                    }
                }
                Fermer();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"Une erreur est survenue : {ex.Message}", "OK");
            }
        }

        private async Task ModifierLecteurAsync()
        {
            try
            {
                if (_lecteur != null)
                {
                    _lecteur.Nom = NomOuTitre;
                    _lecteur.Prenom = AuteurOuPrenom;
                    _lecteur.Telephone = GenreOuTelephone;

                    bool success = _databaseService.ModifierLecteur(_lecteur);
                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Succès", "Lecteur modifié avec succès !", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible de modifier le lecteur.", "OK");
                    }
                }
                Fermer();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"Une erreur est survenue : {ex.Message}", "OK");
            }
        }

        private void Fermer() => _fermerPopup.Invoke();
    }
}
