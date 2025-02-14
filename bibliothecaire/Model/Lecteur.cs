using System.ComponentModel;
using System.Text.RegularExpressions;

namespace bibliothecaire.Model
{
    public class Lecteur : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _idLecteur;
        private string _nom;
        private string _prenom;
        private string _telephone;
        private string _email;
        private string _adresse;

        public Lecteur(int idLecteur, string nom, string prenom, string telephone, string email, string adresse)
        {
            IdLecteur = idLecteur;
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
            Email = email;
            Adresse = adresse;
        }
        

        public int IdLecteur
        {
            get => _idLecteur;
            set
            {
                _idLecteur = value;
                OnPropertyChanged(nameof(IdLecteur));
            }
        }

        public string Nom
        {
            get => _nom;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                    throw new ArgumentException("Le nom est obligatoire et doit contenir au maximum 50 caractères.");
                _nom = value;
                OnPropertyChanged(nameof(Nom));
            }
        }

        public string Prenom
        {
            get => _prenom;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                    throw new ArgumentException("Le prénom est obligatoire et doit contenir au maximum 50 caractères.");
                _prenom = value;
                OnPropertyChanged(nameof(Prenom));
            }
        }

        public string Telephone
        {
            get => _telephone;
            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, @"^(\+32|0)[1-9]\d{8}$"))
                    throw new ArgumentException("Format du téléphone invalide. Il doit être sous la forme +32xxxxxxxxx ou 0xxxxxxxxx.");

                _telephone = value;
                OnPropertyChanged(nameof(Telephone));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Format d'email invalide.");
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Adresse
        {
            get => _adresse;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("L'adresse est obligatoire et doit contenir au maximum 100 caractères.");
                _adresse = value;
                OnPropertyChanged(nameof(Adresse));
            }
        }

        public bool EstValide(out string messageErreur)
        {
            messageErreur = "";
            try
            {
                _ = Nom;
                _ = Prenom;
                _ = Telephone;
                _ = Email;
                _ = Adresse;
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
