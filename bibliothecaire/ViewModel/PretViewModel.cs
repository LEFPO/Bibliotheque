using System.Collections.ObjectModel;
using System.Windows.Input;
using bibliothecaire.Model;
using Microsoft.Maui.Controls;

namespace bibliothecaire.ViewModel
{
    public class PretViewModel : BaseViewModel
    {
        private string _rechercheTexte;
        private Pret _pretSelectionne;
        private ObservableCollection<Pret> _listePrets;

        public string RechercheTexte
        {
            get => _rechercheTexte;
            set => SetProperty(ref _rechercheTexte, value);
        }

        public Pret PretSelectionne
        {
            get => _pretSelectionne;
            set => SetProperty(ref _pretSelectionne, value);
        }

        public ObservableCollection<Pret> ListePrets
        {
            get => _listePrets;
            set => SetProperty(ref _listePrets, value);
        }

        public ICommand AjouterPretCommand { get; }
        public ICommand SupprimerPretCommand { get; }
        public ICommand FiltrerCommand { get; }

        public PretViewModel()
        {
            ListePrets = new ObservableCollection<Pret>
            {
                new Pret(1, 101, 201, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(14))),
                new Pret(2, 102, 202, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
            };

            AjouterPretCommand = new Command(AjouterPret);
            SupprimerPretCommand = new Command(SupprimerPret);
            FiltrerCommand = new Command(FiltrerPrets);
        }

        private void AjouterPret()
        {
            // Simule l'ajout d'un prêt avec un livre et un lecteur fictifs
            ListePrets.Add(new Pret(ListePrets.Count + 1, 103, 203, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(10))));
        }

        private void SupprimerPret()
        {
            if (PretSelectionne != null)
            {
                ListePrets.Remove(PretSelectionne);
            }
        }

        private void FiltrerPrets()
        {
            // Simule un filtre de recherche (à adapter selon une vraie base de données)
            if (!string.IsNullOrWhiteSpace(RechercheTexte))
            {
                ListePrets = new ObservableCollection<Pret>(ListePrets.Where(p => p.IdLivre.ToString().Contains(RechercheTexte)));
                OnPropertyChanged(nameof(ListePrets));
            }
        }
    }
}
