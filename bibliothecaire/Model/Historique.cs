using System.ComponentModel;

namespace bibliothecaire.Model
{
    public enum StatutHistorique
    {
        EnCours = 0,
        Terminé = 1,
        EnRetard = 2
    }

    public class Historique : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _idHistorique;
        private int _idLivre;
        private int _idLecteur;
        private DateOnly _dateEmprunt;
        private DateOnly _dateRetour;
        private StatutHistorique _statut;

        public Historique(int idHistorique, int idLivre, int idLecteur, DateOnly dateEmprunt, DateOnly dateRetour)
        {
            IdHistorique = idHistorique;
            IdLivre = idLivre;
            IdLecteur = idLecteur;
            DateEmprunt = dateEmprunt;
            DateRetour = dateRetour;
            Statut = StatutHistorique.EnCours; // Un emprunt commence toujours avec "EnCours"
        }

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

        public DateOnly DateEmprunt
        {
            get => _dateEmprunt;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentException("La date d'emprunt ne peut pas être dans le futur.");
                _dateEmprunt = value;
                OnPropertyChanged(nameof(DateEmprunt));
            }
        }

        public DateOnly DateRetour
        {
            get => _dateRetour;
            set
            {
                if (value <= _dateEmprunt)
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

        // Marquer l'historique comme terminé
        public void CloturerHistorique()
        {
            if (Statut == StatutHistorique.Terminé)
                throw new InvalidOperationException("Cet emprunt est déjà clôturé.");
            
            Statut = StatutHistorique.Terminé;
        }

        // Vérifier si le prêt dans l'historique est en retard
        public void VerifierRetard()
        {
            if (DateOnly.FromDateTime(DateTime.Now) > DateRetour)
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
