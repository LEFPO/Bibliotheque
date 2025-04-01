using System.ComponentModel;

namespace bibliothecaire.Model
{
    public enum StatutPret
    {
        EnCours,
        Termine,
        EnRetard
    }

    public class Pret : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _idPret;
        private DateTime _datePret;
        private DateTime _dateRetourPret;
        private StatutPret _statut;
        private DateTime _dateEmprunt;
        private int _idLivre;
        private int _idLecteur;

        public string TitreLivre { get; set; }
        public string NomLecteur { get; set; }

        public Pret(int idPret, int idLivre, int idLecteur, DateTime datePret, DateTime dateRetourPret)
        {
            IdPret = idPret;
            IdLivre = idLivre;
            IdLecteur = idLecteur;
            DatePret = datePret;
            DateRetourPret = dateRetourPret;
            Statut = StatutPret.EnCours; // Un prêt commence toujours avec "EnCours"
        }

        public int IdPret
        {
            get => _idPret;
            set
            {
                _idPret = value;
                OnPropertyChanged(nameof(IdPret));
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

        public DateTime DatePret
        {
            get => _datePret;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La date de prêt ne peut pas être dans le futur.");
                _datePret = value;
                OnPropertyChanged(nameof(DatePret));
            }
        }

        public DateTime DateRetourPret
        {
            get => _dateRetourPret;
            set
            {
                if (value <= _datePret)
                    throw new ArgumentException("La date de retour doit être après la date de prêt.");
                _dateRetourPret = value;
                OnPropertyChanged(nameof(DateRetourPret));
            }
        }

        public StatutPret Statut
        {
            get => _statut;
            set
            {
                _statut = value;
                OnPropertyChanged(nameof(Statut));
            }
        }

        // Marquer le prêt comme terminé
        public void CloturerPret()
        {
            if (Statut == StatutPret.Termine)
                throw new InvalidOperationException("Ce prêt est déjà clôturé.");
            
            Statut = StatutPret.Termine;
        }

        // Vérifier si le prêt est en retard
        public void VerifierRetard()
        {
            if (DateTime.Now > DateRetourPret)
            {
                Statut = StatutPret.EnRetard;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
