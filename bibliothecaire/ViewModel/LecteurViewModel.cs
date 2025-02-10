using System.Collections.ObjectModel;
using System.Windows.Input;
using bibliothecaire.Model;

namespace bibliothecaire.ViewModel
{
    public class LecteurViewModel : BaseViewModel
    {
        private string _nom;
        private string _prenom;
        private string _email;
        private Lecteur _lecteurSelectionne;

        public ObservableCollection<Lecteur> Lecteurs { get; set; } = new ObservableCollection<Lecteur>();

        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        public string Prenom
        {
            get => _prenom;
            set => SetProperty(ref _prenom, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public Lecteur LecteurSelectionne
        {
            get => _lecteurSelectionne;
            set => SetProperty(ref _lecteurSelectionne, value);
        }

        public ICommand AjouterLecteurCommand { get; }
        public ICommand SupprimerLecteurCommand { get; }

        public LecteurViewModel()
        {
            AjouterLecteurCommand = new Command(AjouterLecteur);
            SupprimerLecteurCommand = new Command(SupprimerLecteur);
        }

        private void AjouterLecteur()
        {
            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Email))
                return;

            var nouveauLecteur = new Lecteur(Lecteurs.Count + 1, Nom, Prenom, "Adresse", "Téléphone", Email);
            Lecteurs.Add(nouveauLecteur);

            // Réinitialisation
            Nom = Prenom = Email = string.Empty;
        }

        private void SupprimerLecteur()
        {
            if (LecteurSelectionne != null)
                Lecteurs.Remove(LecteurSelectionne);
        }
    }
}