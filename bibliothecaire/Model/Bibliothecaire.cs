using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace bibliothecaire.Model
{
    public class Bibliothecaire : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _idBibliothecaire;
        private string _nom;
        private string _prenom;
        private string _identifiant;
        private string _motDePasseHash;

        public Bibliothecaire(int id, string nom, string prenom, string identifiant, string motDePasse)
        {
            IdBibliothecaire = id;
            Nom = nom;
            Prenom = prenom;
            Identifiant = identifiant;
            MotDePasse = motDePasse;  // Utilise le setter pour hacher le mot de passe
        }

        public int IdBibliothecaire
        {
            get => _idBibliothecaire;
            set
            {
                _idBibliothecaire = value;
                OnPropertyChanged(nameof(IdBibliothecaire));
            }
        }

        public string Nom
        {
            get => _nom;
            set
            {
                if (CheckRequiredField(value) && CheckLength(value, 50))
                {
                    _nom = value;
                    OnPropertyChanged(nameof(Nom));
                }
                else
                {
                    throw new ArgumentException("Le nom ne peut pas être vide et doit contenir moins de 50 caractères.");
                }
            }
        }

        public string Prenom
        {
            get => _prenom;
            set
            {
                if (CheckRequiredField(value) && CheckLength(value, 50))
                {
                    _prenom = value;
                    OnPropertyChanged(nameof(Prenom));
                }
                else
                {
                    throw new ArgumentException("Le prénom ne peut pas être vide et doit contenir moins de 50 caractères.");
                }
            }
        }

        public string Identifiant
        {
            get => _identifiant;
            set
            {
                if (CheckRequiredField(value) && CheckLength(value, 30))
                {
                    _identifiant = value;
                    OnPropertyChanged(nameof(Identifiant));
                }
                else
                {
                    throw new ArgumentException("L'identifiant ne peut pas être vide et doit contenir moins de 30 caractères.");
                }
            }
        }

        public string MotDePasse
        {
            set
            {
                if (CheckPasswordStrength(value))
                {
                    _motDePasseHash = HashPassword(value);
                    OnPropertyChanged(nameof(MotDePasse));
                }
                else
                {
                    throw new ArgumentException("Le mot de passe doit contenir au moins 8 caractères, avec une majuscule, un chiffre et un caractère spécial.");
                }
            }
        }

        public bool VerifierMotDePasse(string motDePasse)
        {
            return HashPassword(motDePasse) == _motDePasseHash;
        }

        // 🔹 Vérifie si une chaîne n'est pas vide ou null
        private bool CheckRequiredField(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        // 🔹 Vérifie si une chaîne respecte une longueur maximale
        private bool CheckLength(string value, int maxLength)
        {
            return value.Length <= maxLength;
        }

        // 🔹 Vérifie la robustesse d'un mot de passe
        private bool CheckPasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        // 🔹 Hachage sécurisé du mot de passe (SHA256)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
