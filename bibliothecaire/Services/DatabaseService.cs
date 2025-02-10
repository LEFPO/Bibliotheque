using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using bibliothecaire.Model;
using Npgsql;

namespace bibliothecaire.Services
{
    public class DatabaseService 
    {
        private readonly string _connectionString = "Host=localhost;Port=5432;Database=bibliotheque_db;Username=postgres;Password=Arkana10021994;";

        public bool VerifierConnexion(string identifiant, string motDePasse)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();

                // 🔹 Hachage du mot de passe en SHA256
                string motDePasseHache = HashPassword(motDePasse);

                // 🔹 Log pour vérifier ce que l'application envoie
                Console.WriteLine($"Tentative de connexion avec : {identifiant} / {motDePasse} (Hash: {motDePasseHache})");

                string sql = "SELECT COUNT(*) FROM bibliothecaire WHERE LOWER(identifiant) = LOWER(@identifiant) AND mot_de_passe = @motDePasse;";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@identifiant", identifiant);
                cmd.Parameters.AddWithValue("@motDePasse", motDePasseHache);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                Console.WriteLine($"Résultat de la requête : {count}");

                return count > 0; // ✅ Retourne vrai si l'utilisateur existe
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur de connexion : {ex.Message}");
                return false;
            }
        }

        // 🔹 Fonction de hachage SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public ObservableCollection<Livre> ObtenirLivres()
        {
            var listeLivres = new ObservableCollection<Livre>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT id_livre, titre, auteur, genre, date_publication FROM livre", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                listeLivres.Add(new Livre(
                    reader.GetInt32(0), 
                    reader.GetString(1), 
                    reader.GetString(2), 
                    reader.GetString(3), 
                    DateOnly.FromDateTime(reader.GetDateTime(4))
                ));
            }
            return listeLivres;
        }

        public ObservableCollection<Lecteur> ObtenirLecteurs()
        {
            var listeLecteurs = new ObservableCollection<Lecteur>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT id_lecteur, nom, prenom, telephone, email, adresse FROM lecteur", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    string numero = reader.GetString(3); // Téléphone
                    if (string.IsNullOrEmpty(numero))
                    {
                        Console.WriteLine("⚠ Numéro de téléphone manquant, valeur remplacée.");
                        numero = "Non défini";
                    }

                    var lecteur = new Lecteur(
                        reader.GetInt32(0), // id_lecteur
                        reader.GetString(1), // nom
                        reader.GetString(2), // prenom
                        numero, // téléphone corrigé
                        reader.GetString(4), // email
                        reader.GetString(5)  // adresse
                    );

                    listeLecteurs.Add(lecteur);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erreur lors du chargement d'un lecteur : {ex.Message}");
                }
            }

            return listeLecteurs;
        }
    }
}
