using System.Collections.ObjectModel;
using System.Diagnostics;
using bibliothecaire.Model;
using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace bibliothecaire.ViewModel;

public partial class PretViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Pret> prets;
    [ObservableProperty] private Pret pretSelectionne;
    [ObservableProperty] private ObservableCollection<Livre> livres;
    [ObservableProperty] private ObservableCollection<Lecteur> lecteurs;
    [ObservableProperty] private Livre livreSelectionne;
    [ObservableProperty] private Lecteur lecteurSelectionne;
    [ObservableProperty] private DateTime datePret = DateTime.Now;
    [ObservableProperty] private DateTime dateRetour = DateTime.Now.AddDays(14);
    [ObservableProperty] private StatutPret statutSelectionne;

    public ObservableCollection<StatutPret> Statuts { get; } = new(Enum.GetValues(typeof(StatutPret)).Cast<StatutPret>());

    public ICommand AjouterPretCommand { get; }
    public ICommand ModifierPretCommand { get; }
    public ICommand SupprimerPretCommand { get; }

    public PretViewModel(DatabaseService databaseService) : base(databaseService)
    {
        ChargerDonnees();

        AjouterPretCommand = new RelayCommand(AjouterPret);
        ModifierPretCommand = new RelayCommand(ModifierPret);
        SupprimerPretCommand = new RelayCommand(SupprimerPret);
    }

    public void ChargerDonnees()
    {
        Prets = _databaseService.ObtenirPrets();
        Livres = _databaseService.ObtenirLivres();
        Lecteurs = _databaseService.ObtenirLecteurs();
    }

    private void AjouterPret()
    {
        try
        {
            if (LivreSelectionne == null || LecteurSelectionne == null)
            {
                Application.Current.MainPage.DisplayAlert("Erreur", "Sélectionnez un livre et un lecteur.", "OK");
                return;
            }

            var nouveauPret = new Pret(0, LivreSelectionne.IdLivre, LecteurSelectionne.IdLecteur, DatePret, DateRetour)
            {
                TitreLivre = LivreSelectionne.Titre,
                NomLecteur = LecteurSelectionne.Nom
            };

            _databaseService.AjouterPret(nouveauPret);
            _databaseService.AjouterHistorique(nouveauPret); // ✅ Ajout dans historique
            Prets.Add(nouveauPret);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ ERREUR AjouterPret : {ex.Message}");
            Application.Current.MainPage.DisplayAlert("Erreur", $"Erreur lors de l'ajout : {ex.Message}", "OK");
        }
    }

    private void ModifierPret()
    {
        if (PretSelectionne == null)
        {
            Application.Current.MainPage.DisplayAlert("Erreur", "Aucun prêt sélectionné.", "OK");
            return;
        }

        try
        {
            PretSelectionne.DateRetourPret = DateRetour;
            PretSelectionne.Statut = StatutSelectionne;

            _databaseService.ModifierPret(PretSelectionne);
            Application.Current.MainPage.DisplayAlert("Succès", "Le prêt a été modifié avec succès.", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ ERREUR ModifierPret : {ex.Message}");
            Application.Current.MainPage.DisplayAlert("Erreur", $"Erreur lors de la modification : {ex.Message}", "OK");
        }
    }

    private void SupprimerPret()
    {
        if (PretSelectionne == null)
            return;

        _databaseService.SupprimerPret(PretSelectionne.IdPret);
        Prets.Remove(PretSelectionne);
    }

    [RelayCommand]
    private async Task RetourAsync()
    {
        await Shell.Current.GoToAsync("//GestionPretsView");
    }

    // 🔁 Quand un prêt est sélectionné, remplir les champs
    partial void OnPretSelectionneChanged(Pret value)
    {
        if (value != null)
        {
            DateRetour = value.DateRetourPret;
            StatutSelectionne = value.Statut;
        }
    }
}
