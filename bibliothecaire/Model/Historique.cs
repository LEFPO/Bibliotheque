using System;
using System.ComponentModel;

namespace bibliothecaire.Model
{
    public enum StatutHistorique
    {
        EnCours = 0,
        Termine = 1,
        EnRetard = 2
    }

    public class Historique : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _idHistorique;
        private int _idLivre;
        private int _idLecteur;
        private DateTime _dateEmprunt;
        private DateTime? _dateRetour;
        private StatutHistorique _statut;

        public string TitreLivre { get; set; }
        public string NomLecteur { get; set; }

        public Historique(int idHistorique, int idLivre, int idLecteur, DateTime dateEmprunt, DateTime dateRetour)
        {
            IdHistorique = idHistorique;
            IdLivre = idLivre;
            IdLecteur = idLecteur;
            DateEmprunt = dateEmprunt;
            DateRetour = dateRetour;
            Statut = StatutHistorique.EnCours;
        }

        public Historique() { }

        public int IdHistorique
        {
            get => _idHistorique;
            set
            {
                _idHistorique = value;
                OnPropertyChanged(nameof(IdHistorique));
            }
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

        public int IdLecteur
        {
            get => _idLecteur;
            set
            {
                _idLecteur = value;
                OnPropertyChanged(nameof(IdLecteur));
            }
        }

        public DateTime DateEmprunt
        {
            get => _dateEmprunt;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La date d'emprunt ne peut pas être dans le futur.");
                _dateEmprunt = value;
                OnPropertyChanged(nameof(DateEmprunt));
            }
        }

        public DateTime? DateRetour
        {
            get => _dateRetour;
            set
            {
                if (value.HasValue && value.Value <= _dateEmprunt)
                    throw new ArgumentException("La date de retour doit être après la date d'emprunt.");
                _dateRetour = value;
                OnPropertyChanged(nameof(DateRetour));
            }

        }

        public StatutHistorique Statut
        {
            get => _statut;
            set
            {
                _statut = value;
                OnPropertyChanged(nameof(Statut));
            }
        }

        public void CloturerHistorique()
        {
            if (Statut == StatutHistorique.Termine)
                throw new InvalidOperationException("Cet emprunt est déjà clôturé.");
            Statut = StatutHistorique.Termine;
        }

        public void VerifierRetard()
        {
            if (DateTime.Now > DateRetour)
            {
                Statut = StatutHistorique.EnRetard;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
