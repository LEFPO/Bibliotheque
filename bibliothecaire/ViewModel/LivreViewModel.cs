using System.Collections.ObjectModel;
using System.Windows.Input;
using bibliothecaire.Model;

namespace bibliothecaire.ViewModel
{
    public class LivreViewModel : BaseViewModel
    {
        private string _titre;
        private string _auteur;
        private string _genre;
        private Livre _livreSelectionne;

        public ObservableCollection<Livre> Livres { get; set; } = new ObservableCollection<Livre>();

        public string Titre
        {
            get => _titre;
            set => SetProperty(ref _titre, value);
        }

        public string Auteur
        {
            get => _auteur;
            set => SetProperty(ref _auteur, value);
        }

        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        public Livre LivreSelectionne
        {
            get => _livreSelectionne;
            set => SetProperty(ref _livreSelectionne, value);
        }

        public ICommand AjouterLivreCommand { get; }
        public ICommand SupprimerLivreCommand { get; }

        public LivreViewModel()
        {
            AjouterLivreCommand = new Command(AjouterLivre);
            SupprimerLivreCommand = new Command(SupprimerLivre);
        }

        private void AjouterLivre()
        {
            if (string.IsNullOrWhiteSpace(Titre) || string.IsNullOrWhiteSpace(Auteur))
                return;

            var nouveauLivre = new Livre(Livres.Count + 1, Titre, Auteur, Genre, DateOnly.FromDateTime(DateTime.Now));
            Livres.Add(nouveauLivre);

            // Réinitialisation
            Titre = Auteur = Genre = string.Empty;
        }

        private void SupprimerLivre()
        {
            if (LivreSelectionne != null)
                Livres.Remove(LivreSelectionne);
        }
    }
}