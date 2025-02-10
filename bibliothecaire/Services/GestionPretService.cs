using System.Collections.ObjectModel;
using bibliothecaire.Model;
using Npgsql;

namespace bibliothecaire.Services
{
    public class GestionPretService
    {
        private readonly DatabaseService _databaseService;
        
        public ObservableCollection<Livre> TousLesLivres { get; private set; }
        public ObservableCollection<Lecteur> TousLesLecteurs { get; private set; }

        public ObservableCollection<Livre> ListeLivres { get; private set; }
        public ObservableCollection<Lecteur> ListeLecteurs { get; private set; }

        public GestionPretService()
        {
            _databaseService = new DatabaseService();
            TousLesLivres = new ObservableCollection<Livre>();
            TousLesLecteurs = new ObservableCollection<Lecteur>();
            ListeLivres = new ObservableCollection<Livre>();
            ListeLecteurs = new ObservableCollection<Lecteur>();
        }

        // Charger toutes les données
        public void ChargerLivres()
        {
            TousLesLivres.Clear();
            ListeLivres.Clear();

            foreach (var livre in _databaseService.ObtenirLivres())
            {
                TousLesLivres.Add(livre);
                ListeLivres.Add(livre);
            }
        }

        public void ChargerLecteurs()
        {
            TousLesLecteurs.Clear();
            ListeLecteurs.Clear();

            foreach (var lecteur in _databaseService.ObtenirLecteurs())
            {
                TousLesLecteurs.Add(lecteur);
                ListeLecteurs.Add(lecteur);
            }
        }

        // Appliquer un filtre sur les livres
        public void FiltrerLivres(string filtre)
        {
            ListeLivres.Clear();
            foreach (var livre in TousLesLivres)
            {
                if (livre.Titre.Contains(filtre, StringComparison.OrdinalIgnoreCase) ||
                    livre.Auteur.Contains(filtre, StringComparison.OrdinalIgnoreCase))
                {
                    ListeLivres.Add(livre);
                }
            }
        }

        // Appliquer un filtre sur les lecteurs
        public void FiltrerLecteurs(string filtre)
        {
            ListeLecteurs.Clear();
            foreach (var lecteur in TousLesLecteurs)
            {
                if (lecteur.Nom.Contains(filtre, StringComparison.OrdinalIgnoreCase) ||
                    lecteur.Prenom.Contains(filtre, StringComparison.OrdinalIgnoreCase))
                {
                    ListeLecteurs.Add(lecteur);
                }
            }
        }
    }
}
