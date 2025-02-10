using System;
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
        
        public Lecteur() {}

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
                _nom = value;
                OnPropertyChanged(nameof(Nom));
            }
        }

        public string Prenom
        {
            get => _prenom;
            set
            {
                _prenom = value;
                OnPropertyChanged(nameof(Prenom));
            }
        }

        public string Telephone
        {
            get => _telephone;
            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, @"^(\+33|0)[1-9]\d{8}$"))
                {
                    Console.WriteLine($"⚠ Erreur : Numéro invalide ({value}). Valeur ignorée.");
                    value = "Non défini"; // Remplace le numéro invalide par un placeholder
                }

                _telephone = value;
                OnPropertyChanged(nameof(Telephone));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Adresse
        {
            get => _adresse;
            set
            {
                _adresse = value;
                OnPropertyChanged(nameof(Adresse));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
