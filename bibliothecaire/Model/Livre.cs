using System.ComponentModel;

namespace bibliothecaire.Model
{
    public enum StatutLivre
    {
        EnStock = 0,
        Emprunté = 1
    }

    public class Livre : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _idLivre;
        private string _titre;
        private string _auteur;
        private string _genre;
        private StatutLivre _statut;
        private DateOnly _datePublication;

        public Livre(int idLivre, string titre, string auteur, string genre, DateOnly datePublication)
        {
            IdLivre = idLivre;
            Titre = titre;
            Auteur = auteur;
            Genre = genre;
            DatePublication = datePublication;
            Statut = StatutLivre.EnStock;
        }

        public int IdLivre
        {
            get => _idLivre;
            set
            {
                _idLivre = value;
                OnPropertyChanged(nameof(IdLivre));
            }
        }

        public string Titre
        {
            get => _titre;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("Le titre est obligatoire et doit contenir moins de 100 caractères.");
                _titre = value;
                OnPropertyChanged(nameof(Titre));
            }
        }

        public string Auteur
        {
            get => _auteur;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                    throw new ArgumentException("L'auteur est obligatoire et doit contenir moins de 50 caractères.");
                _auteur = value;
                OnPropertyChanged(nameof(Auteur));
            }
        }

        public string Genre
        {
            get => _genre;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 30)
                    throw new ArgumentException("Le genre est obligatoire et doit contenir moins de 30 caractères.");
                _genre = value;
                OnPropertyChanged(nameof(Genre));
            }
        }

        public DateOnly DatePublication
        {
            get => _datePublication;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentException("La date de publication ne peut pas être dans le futur.");
                _datePublication = value;
                OnPropertyChanged(nameof(DatePublication));
            }
        }

        public StatutLivre Statut
        {
            get => _statut;
            set
            {
                _statut = value;
                OnPropertyChanged(nameof(Statut));
            }
        }

        public bool EstValide(out string messageErreur)
        {
            messageErreur = "";
            try
            {
                _ = Titre;
                _ = Auteur;
                _ = Genre;
                _ = DatePublication;
                return true;
            }
            catch (ArgumentException ex)
            {
                messageErreur = ex.Message;
                return false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
