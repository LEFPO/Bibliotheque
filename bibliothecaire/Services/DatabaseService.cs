using System.Diagnostics;
using Npgsql;
using bibliothecaire.Model;
using System.Collections.ObjectModel;

namespace bibliothecaire.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                                "Host=localhost;Port=5432;Database=bibliotheque_db;Username=postgres;Password=Arkana10021994";
        }

        /// <summary>
        /// Ajoute un livre dans la base de données.
        /// </summary>
        public bool AjouterLivre(Livre livre)
        {
            if (!livre.EstValide(out string erreur))
            {
                Debug.WriteLine($"❌ Erreur validation : {erreur}");
                return false;
            }

            string requete = "INSERT INTO livres (titre, auteur, genre, date_publication, statut) VALUES (@titre, @auteur, @genre, @date_publication, @statut)";
            var parametres = new Dictionary<string, object>
            {
                { "@titre", livre.Titre },
                { "@auteur", livre.Auteur },
                { "@genre", livre.Genre },
                { "@date_publication", livre.DatePublication },
                { "@statut", (int)livre.Statut }
            };

            return ExecuterRequete(requete, parametres);
        }

        /// <summary>
        /// Récupère un livre par son ID_Livres.
        /// </summary>
        public Livre ObtenirLivreParId(int id)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string requete = "SELECT * FROM livres WHERE id_livre = @id_livre";

                using var cmd = new NpgsqlCommand(requete, conn);
                cmd.Parameters.AddWithValue("@id_livre", id);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Livre(
                        idLivre: reader.GetInt32(0),
                        titre: reader.GetString(1),
                        auteur: reader.GetString(2),
                        genre: reader.GetString(3),
                        datePublication: DateOnly.FromDateTime(reader.GetDateTime(4))
                    );
                }
            }
            catch (PostgresException ex)
            {
                Debug.WriteLine($"❌ Erreur SQL : {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erreur inconnue : {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Met à jour un livre dans la base.
        /// </summary>
        public bool ModifierLivre(Livre livre)
        {
            if (!livre.EstValide(out string erreur))
            {
                Debug.WriteLine($"❌ Erreur validation : {erreur}");
                return false;
            }

            string requete = "UPDATE livres SET titre=@titre, auteur=@auteur, genre=@genre, date_publication=@date_publication, statut=@statut WHERE id_livre=@id_livre";
            var parametres = new Dictionary<string, object>
            {
                { "@id_livre", livre.IdLivre },
                { "@titre", livre.Titre },
                { "@auteur", livre.Auteur },
                { "@genre", livre.Genre },
                { "@date_publication", livre.DatePublication },
                { "@statut", (int)livre.Statut }
            };

            return ExecuterRequete(requete, parametres);
        }

        /// <summary>
        /// Supprime un livre en base.
        /// </summary>
        public bool SupprimerLivre(int id)
        {
            string requete = "DELETE FROM livres WHERE id_livre = @id_livre";
            var parametres = new Dictionary<string, object>
            {
                { "@id_livre", id }
            };

            return ExecuterRequete(requete, parametres);
        }

        /// <summary>
        /// Récupère tous les livres.
        /// </summary>
        public ObservableCollection<Livre> ObtenirLivres()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                string requete = "SELECT id_livre, titre, auteur, genre, date_publication, statut FROM livres";
                using var cmd = new NpgsqlCommand(requete, conn);
                using var reader = cmd.ExecuteReader();

                var livres = new ObservableCollection<Livre>();

                while (reader.Read())
                {
                    livres.Add(new Livre(
                        idLivre: reader.GetInt32(0),
                        titre: reader.GetString(1),
                        auteur: reader.GetString(2),
                        genre: reader.GetString(3),
                        datePublication: DateOnly.FromDateTime(reader.GetDateTime(4))
                    ));
                }

                Debug.WriteLine($"📚 {livres.Count} livres récupérés !");
                return livres;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erreur SQL (livres) : {ex.Message}");
            }

            return new ObservableCollection<Livre>();
        }

        /// <summary>
        /// Met à jour un lecteur dans la base.
        /// </summary>
        public bool ModifierLecteur(Lecteur lecteur)
        {
            if (!lecteur.EstValide(out string erreur))
            {
                Debug.WriteLine($"❌ Erreur validation : {erreur}");
                return false;
            }

            string requete = "UPDATE lecteurs SET nom = @nom, prenom = @prenom, telephone = @telephone, email = @email, adresse = @adresse WHERE id_lecteur = @id_lecteur";
            var parametres = new Dictionary<string, object>
            {
                { "@id_lecteur", lecteur.IdLecteur },
                { "@nom", lecteur.Nom },
                { "@prenom", lecteur.Prenom },
                { "@telephone", lecteur.Telephone ?? (object)DBNull.Value },
                { "@email", lecteur.Email ?? (object)DBNull.Value },
                { "@adresse", lecteur.Adresse }
            };

            return ExecuterRequete(requete, parametres);
        }

        public ObservableCollection<Lecteur> ObtenirLecteurs()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                string requete = "SELECT id_lecteur, nom, prenom, telephone, email, adresse FROM lecteurs";
                using var cmd = new NpgsqlCommand(requete, conn);
                using var reader = cmd.ExecuteReader();

                var lecteurs = new ObservableCollection<Lecteur>();

                while (reader.Read())
                {
                    lecteurs.Add(new Lecteur(
                        idLecteur: reader.GetInt32(0),
                        nom: reader.GetString(1),
                        prenom: reader.GetString(2),
                        telephone: reader.IsDBNull(3) ? "" : reader.GetString(3),
                        email: reader.IsDBNull(4) ? "" : reader.GetString(4),
                        adresse: reader.GetString(5)
                    ));
                }

                Debug.WriteLine($"👤 {lecteurs.Count} lecteurs récupérés !");
                return lecteurs;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erreur SQL (lecteurs) : {ex.Message}");
            }

            return new ObservableCollection<Lecteur>();
        }

        public bool SupprimerLecteur(int idLecteur)
        {
            string requete = "DELETE FROM lecteurs WHERE id_lecteur = @id_lecteur";
            var parametres = new Dictionary<string, object>
            {
                { "@id_lecteur", idLecteur }
            };

            return ExecuterRequete(requete, parametres);
        }

        public bool AjouterLecteur(Lecteur lecteur)
        {
            string erreur = ""; // ✅ Initialisation

            if (lecteur == null || !lecteur.EstValide(out erreur))
            {
                Debug.WriteLine($"❌ Erreur validation : {erreur}");
                return false;
            }

            string requete = "INSERT INTO lecteurs (nom, prenom, telephone, email, adresse) VALUES (@nom, @prenom, @telephone, @email, @adresse)";
            var parametres = new Dictionary<string, object>
            {
                { "@nom", lecteur.Nom },
                { "@prenom", lecteur.Prenom },
                { "@telephone", lecteur.Telephone ?? (object)DBNull.Value },
                { "@email", lecteur.Email ?? (object)DBNull.Value },
                { "@adresse", lecteur.Adresse }
            };

            return ExecuterRequete(requete, parametres);
        }

        /// <summary>
        /// Exécute une requête SQL avec gestion automatique des erreurs et de la connexion.
        /// </summary>
        public bool ExecuterRequete(string requete, Dictionary<string, object> parametres)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand(requete, conn);

                foreach (var param in parametres)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (PostgresException ex)
            {
                Debug.WriteLine($"❌ Erreur SQL : {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erreur inconnue : {ex.Message}");
            }

            return false;
        }
    }
}
