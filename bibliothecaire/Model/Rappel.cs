using System.ComponentModel;

namespace bibliothecaire.Model
{
    public enum TypeRappel
    {
        RetourLivre,
        FraisImpayés
    }

    public class Rappel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _idRappel;
        private DateOnly _dateEnvoie;
        private TypeRappel _type;
        private bool _status;

        public Rappel(int idRappel, DateOnly dateEnvoie, TypeRappel type, bool status)
        {
            IdRappel = idRappel;
            DateEnvoie = dateEnvoie;
            Type = type;
            Status = status;
        }

        public int IdRappel
        {
            get => _idRappel;
            set
            {
                _idRappel = value;
                OnPropertyChanged(nameof(IdRappel));
            }
        }

        public DateOnly DateEnvoie
        {
            get => _dateEnvoie;
            set
            {
                if (value < DateOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentException("La date d'envoi ne peut pas être dans le passé.");
                
                _dateEnvoie = value;
                OnPropertyChanged(nameof(DateEnvoie));
            }
        }

        public TypeRappel Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public bool Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        // Marquer le rappel comme envoyé
        public void Envoyer()
        {
            if (Status)
                throw new InvalidOperationException("Le rappel a déjà été envoyé.");
            
            Status = true;
        }

        // Vérifier si le rappel est en retard
        public bool EstEnRetard()
        {
            return DateOnly.FromDateTime(DateTime.Now) > DateEnvoie && !Status;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
