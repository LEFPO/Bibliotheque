using System.ComponentModel;

namespace bibliothecaire.Model
{
    public enum StatutLivre
    {
        EnStock,
        Emprunté
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
            Statut = StatutLivre.EnStock; // Par défaut, un livre est disponible
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
                if (CheckRequiredField(value) && CheckLength(value, 100))
                {
                    _titre = value;
                    OnPropertyChanged(nameof(Titre));
                }
                else
                {
                    throw new ArgumentException("Le titre ne peut pas être vide et doit contenir moins de 100 caractères.");
                }
            }
        }

        public string Auteur
        {
            get => _auteur;
            set
            {
                if (CheckRequiredField(value) && CheckLength(value, 50))
                {
                    _auteur = value;
                    OnPropertyChanged(nameof(Auteur));
                }
                else
                {
                    throw new ArgumentException("L'auteur ne peut pas être vide et doit contenir moins de 50 caractères.");
                }
            }
        }

        public string Genre
        {
            get => _genre;
            set
            {
                if (CheckRequiredField(value) && CheckLength(value, 30))
                {
                    _genre = value;
                    OnPropertyChanged(nameof(Genre));
                }
                else
                {
                    throw new ArgumentException("Le genre ne peut pas être vide et doit contenir moins de 30 caractères.");
                }
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

        // Marquer le livre comme emprunté
        public void Emprunter()
        {
            if (Statut == StatutLivre.Emprunté)
                throw new InvalidOperationException("Ce livre est déjà emprunté.");
            
            Statut = StatutLivre.Emprunté;
        }

        // Marquer le livre comme retourné
        public void Retourner()
        {
            if (Statut == StatutLivre.EnStock)
                throw new InvalidOperationException("Ce livre est déjà disponible.");
            
            Statut = StatutLivre.EnStock;
        }

        // Vérifie si une chaîne n'est pas vide
        private bool CheckRequiredField(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        // Vérifie la longueur d'une chaîne
        private bool CheckLength(string value, int maxLength)
        {
            return value.Length <= maxLength;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
