﻿using System.Diagnostics;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace bibliothecaire.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(DatabaseService databaseService) : base(databaseService) { }

        [ObservableProperty]
        private string identifiant = "jdupont";

        [ObservableProperty]
        private string motDePasse = "motdepasse123";

        [ObservableProperty]
        private string messageErreur; // ✅ Propriété pour afficher les erreurs

        [RelayCommand]
        private async Task SeConnecterAsync()
        {
            try
            {
                Debug.WriteLine("🔹 Tentative de connexion...");

                if (string.IsNullOrWhiteSpace(Identifiant) || string.IsNullOrWhiteSpace(MotDePasse))
                {
                    Debug.WriteLine("❌ Identifiant ou mot de passe vide !");
                    MessageErreur = "Veuillez entrer un identifiant et un mot de passe.";
                    return;
                }

                // ✅ Nouvelle façon de vérifier la connexion avec la requête SQL directe
                bool estValide = _databaseService.ConnexionBibliothecaireValide(Identifiant, MotDePasse);

                if (estValide)
                {
                    Debug.WriteLine("✅ Connexion réussie ! Navigation en cours...");
                    MessageErreur = ""; // ✅ Supprime l'erreur en cas de succès

                    if (Shell.Current != null)
                    {
                        Debug.WriteLine("🔹 Shell détecté, navigation vers GestionPretsView.");
                        await Shell.Current.GoToAsync("//GestionPretsView");
                    }
                    else
                    {
                        Debug.WriteLine("❌ Shell.Current est NULL !");
                        MessageErreur = "Problème de navigation : Shell est null.";
                    }
                }
                else
                {
                    Debug.WriteLine("❌ Identifiants incorrects !");
                    MessageErreur = "Identifiants incorrects.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ERREUR CONNEXION : {ex.Message}");
                MessageErreur = $"Problème lors de la connexion : {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task CreerCompte()
        {
            try
            {
                Debug.WriteLine("🔹 Navigation vers la page de création de compte...");
                await Shell.Current.GoToAsync("//CreerBibliothecaireView"); // Assure-toi que le route est bien défini dans AppShell
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ERREUR NAVIGATION : {ex.Message}");
                MessageErreur = $"Problème de navigation : {ex.Message}";
            }
        }
    }
}
