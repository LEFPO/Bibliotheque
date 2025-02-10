using System.Diagnostics;
using System.Windows.Input;
using bibliothecaire.Services;
using bibliothecaire.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace bibliothecaire.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string identifiant = "jdupont";

        [ObservableProperty]
        private string motDePasse = "motdepasse123";
        
        public LoginViewModel()
        {
            
        }

        [RelayCommand]
        private async Task SeConnecterAsync()
        {
            try
            {
                Debug.WriteLine("🔹 Tentative de connexion...");

                if (string.IsNullOrWhiteSpace(Identifiant) || string.IsNullOrWhiteSpace(MotDePasse))
                {
                    Debug.WriteLine("❌ Identifiant ou mot de passe vide !");
                    await Application.Current.MainPage.DisplayAlert("Erreur",
                        "Veuillez entrer un identifiant et un mot de passe.", "OK");
                    return;
                }

                bool estValide = _databaseService.VerifierConnexion(Identifiant, MotDePasse);

                if (estValide)
                {
                    Debug.WriteLine("✅ Connexion réussie ! Navigation en cours...");

                    if (Shell.Current != null)
                    {
                        Debug.WriteLine("🔹 Shell détecté, navigation vers GestionPretsView.");
                        await Shell.Current.GoToAsync("//GestionPretsView");
                    }
                    else
                    {
                        Debug.WriteLine("❌ Shell.Current est NULL !");
                        await Application.Current.MainPage.DisplayAlert("Erreur",
                            "Problème de navigation : Shell est null.", "OK");
                    }
                }
                else
                {
                    Debug.WriteLine("❌ Identifiants incorrects !");
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Identifiants incorrects.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ERREUR CONNEXION : {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Erreur",
                    $"Problème lors de la connexion : {ex.Message}", "OK");
            }
        }
    }
}
