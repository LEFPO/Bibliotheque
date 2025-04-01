using System.Diagnostics;
using bibliothecaire.Model;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace bibliothecaire.ViewModel;

public partial class CreerBibliothecaireViewModel : BaseViewModel
{
    public CreerBibliothecaireViewModel(DatabaseService databaseService) : base(databaseService) { }

    [ObservableProperty]
    private string nom;

    [ObservableProperty]
    private string prenom;

    [ObservableProperty]
    private string identifiant;

    [ObservableProperty]
    private string motDePasse;

    [ObservableProperty]
    private string messageErreur;

    [RelayCommand]
    private async Task CreerCompteAsync()
    {
        try
        {
            Debug.WriteLine("🔹 Création d'un bibliothécaire...");

            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) ||
                string.IsNullOrWhiteSpace(Identifiant) || string.IsNullOrWhiteSpace(MotDePasse))
            {
                MessageErreur = "Tous les champs sont obligatoires.";
                return;
            }

            // Vérifie si l'identifiant existe déjà
            bool existe = _databaseService.ExisteBibliothecaireParIdentifiant(Identifiant);

            if (existe)
            {
                MessageErreur = "Cet identifiant est déjà utilisé.";
                return;
            }

            // Création et insertion du bibliothécaire
            var nouveauBiblio = new Bibliothecaire(
                id: 0,
                nom: Nom,
                prenom: Prenom,
                identifiant: Identifiant,
                motDePasse: MotDePasse
            );

            _databaseService.AjouterBibliothecaire(nouveauBiblio);

            Debug.WriteLine("✅ Compte créé !");
            MessageErreur = "Compte créé avec succès.";

            await Shell.Current.GoToAsync("//LoginView");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ ERREUR CRÉATION : {ex.Message}");
            MessageErreur = $"Erreur : {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task RetourAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("//LoginView");
        }
        catch (Exception ex)
        {
            MessageErreur = $"Erreur navigation : {ex.Message}";
        }
    }
}
