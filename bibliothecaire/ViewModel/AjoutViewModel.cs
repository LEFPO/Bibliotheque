using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using bibliothecaire.Model;
using bibliothecaire.Services;
using bibliothecaire.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace bibliothecaire.ViewModel
{
    public partial class AjoutViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider; // ✅ Injection de ServiceProvider

        public Color CouleurLivre => AjouterLivre ? Colors.Green : Colors.Gray;
        public Color CouleurLecteur => AjouterLecteur ? Colors.Green : Colors.Gray;

        [ObservableProperty] private bool ajouterLivre = true;
        [ObservableProperty] private bool ajouterLecteur = false;

        // Champs pour un livre
        [ObservableProperty] private string titre = string.Empty;
        [ObservableProperty] private string auteur = string.Empty;
        [ObservableProperty] private string genre = string.Empty;
        [ObservableProperty] private DateOnly datePublication = DateOnly.FromDateTime(DateTime.Now);

        // Champs pour un lecteur
        [ObservableProperty] private string nom = string.Empty;
        [ObservableProperty] private string prenom = string.Empty;
        [ObservableProperty] private string telephone = string.Empty;
        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string adresse = string.Empty;

        public AjoutViewModel(DatabaseService databaseService, IServiceProvider serviceProvider)
        {
            _databaseService = databaseService;
            _serviceProvider = serviceProvider; // ✅ Utilisation de ServiceProvider

            AjouterCommand = new AsyncRelayCommand(AjouterAsync);
            FermerCommand = new AsyncRelayCommand(Fermer);
            BasculerLivreCommand = new RelayCommand(() => BasculerAjout(true));
            BasculerLecteurCommand = new RelayCommand(() => BasculerAjout(false));
        }

        public ICommand AjouterCommand { get; }
        public ICommand FermerCommand { get; }
        public ICommand BasculerLivreCommand { get; }
        public ICommand BasculerLecteurCommand { get; }

        private async Task AjouterAsync()
        {
            try
            {
                Debug.WriteLine("🔹 Début de l'ajout...");

                // ✅ Récupération du ViewModel via IServiceProvider
                var gestionPretsViewModel = _serviceProvider.GetRequiredService<GestionPretsViewModel>();

                if (AjouterLivre)
                {
                    Debug.WriteLine($"📚 Ajout d'un livre : {Titre}, {Auteur}, {Genre}");

                    if (string.IsNullOrWhiteSpace(Titre) || string.IsNullOrWhiteSpace(Auteur) || string.IsNullOrWhiteSpace(Genre))
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Tous les champs sont obligatoires pour un livre.", "OK");
                        return;
                    }

                    var nouveauLivre = new Livre(0, Titre, Auteur, Genre, DatePublication);
                    bool success = _databaseService.AjouterLivre(nouveauLivre);

                    if (success)
                    {
                        Debug.WriteLine("✅ Livre ajouté avec succès !");
                        
                        // ✅ Mise à jour de l'UI en temps réel
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            gestionPretsViewModel.Livres.Add(nouveauLivre);
                            gestionPretsViewModel.LivresFiltres.Add(nouveauLivre);
                        });

                        await Application.Current.MainPage.DisplayAlert("Succès", "Livre ajouté avec succès !", "OK");
                    }
                    else
                    {
                        Debug.WriteLine("❌ Erreur lors de l'ajout du livre !");
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible d'ajouter le livre.", "OK");
                    }
                }
                else if (AjouterLecteur)
                {
                    Debug.WriteLine($"👤 Ajout d'un lecteur : {Nom}, {Prenom}, {Telephone}, {Email}");

                    if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) || string.IsNullOrWhiteSpace(Telephone) || string.IsNullOrWhiteSpace(Email))
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Tous les champs sont obligatoires pour un lecteur.", "OK");
                        return;
                    }

                    var nouveauLecteur = new Lecteur(0, Nom, Prenom, Telephone, Email, Adresse);
                    bool success = _databaseService.AjouterLecteur(nouveauLecteur);

                    if (success)
                    {
                        Debug.WriteLine("✅ Lecteur ajouté avec succès !");
                        
                        // ✅ Mise à jour de l'UI en temps réel
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            gestionPretsViewModel.Lecteurs.Add(nouveauLecteur);
                            gestionPretsViewModel.LecteursFiltres.Add(nouveauLecteur);
                        });

                        await Application.Current.MainPage.DisplayAlert("Succès", "Lecteur ajouté avec succès !", "OK");
                    }
                    else
                    {
                        Debug.WriteLine("❌ Erreur lors de l'ajout du lecteur !");
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Impossible d'ajouter le lecteur.", "OK");
                    }
                }

                await Fermer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ERREUR CRITIQUE : {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Erreur fatale", $"Une erreur est survenue : {ex.Message}", "OK");
            }
        }

        private async Task Fermer()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void BasculerAjout(bool ajouterLivre)
        {
            AjouterLivre = ajouterLivre;
            AjouterLecteur = !ajouterLivre;
        }
    }
}
