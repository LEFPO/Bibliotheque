using Npgsql;
using System;
using System.Diagnostics;

namespace bibliothecaire.Services
{
    public static class DatabaseInitializer
    {
        private static readonly string ConnexionServeur = "Host=localhost;Port=5432;Username=postgres;Password=Arkana10021994;";
        private static readonly string NomBase = "bibliotheque_db";
        private static readonly string ConnexionComplete = $"{ConnexionServeur}Database={NomBase};";

        public static void Initialize()
        {
            try
            {
                if (!DatabaseExists())
                {
                    CreateDatabase();
                    Debug.WriteLine("✅ Base de données créée !");
                }
                else
                {
                    Debug.WriteLine("ℹ️ Base de données déjà existante.");
                }

                CreateTablesIfNotExist();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erreur d'initialisation de la base : {ex.Message}");
            }
        }

        private static bool DatabaseExists()
        {
            using var conn = new NpgsqlConnection(ConnexionServeur);
            conn.Open();

            using var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{NomBase}'", conn);
            return cmd.ExecuteScalar() != null;
        }

        private static void CreateDatabase()
        {
            using var conn = new NpgsqlConnection(ConnexionServeur);
            conn.Open();

            using var cmd = new NpgsqlCommand($"CREATE DATABASE \"{NomBase}\"", conn);
            cmd.ExecuteNonQuery();
        }

        private static void CreateTablesIfNotExist()
        {
            using var conn = new NpgsqlConnection(ConnexionComplete);
            conn.Open();

            var requetes = new[]
            {
                // Bibliothécaire
                @"CREATE TABLE IF NOT EXISTS bibliothecaire (
                    id SERIAL PRIMARY KEY,
                    nom TEXT NOT NULL,
                    prenom TEXT NOT NULL,
                    identifiant TEXT UNIQUE NOT NULL,
                    mot_de_passe TEXT NOT NULL
                );",

                // Livres
                @"CREATE TABLE IF NOT EXISTS livres (
                    id_livre SERIAL PRIMARY KEY,
                    titre TEXT NOT NULL,
                    auteur TEXT NOT NULL,
                    genre TEXT,
                    date_publication DATE,
                    statut INT
                );",

                // Lecteurs
                @"CREATE TABLE IF NOT EXISTS lecteurs (
                    id_lecteur SERIAL PRIMARY KEY,
                    nom TEXT NOT NULL,
                    prenom TEXT NOT NULL,
                    telephone TEXT,
                    email TEXT,
                    adresse TEXT NOT NULL
                );",

                // Historique
                @"CREATE TABLE IF NOT EXISTS historique (
                    id_historique SERIAL PRIMARY KEY,
                    id_livre INT NOT NULL,
                    id_lecteur INT NOT NULL,
                    date_emprunt DATE NOT NULL,
                    date_retour DATE,
                    statut SMALLINT NOT NULL CHECK (statut IN (0, 1, 2)),
                    FOREIGN KEY (id_livre) REFERENCES livres(id_livre),
                    FOREIGN KEY (id_lecteur) REFERENCES lecteurs(id_lecteur)
                );",

                // Prêt
                @"CREATE TABLE IF NOT EXISTS pret (
                    id_pret SERIAL PRIMARY KEY,
                    date_pret TIMESTAMP NOT NULL,
                    date_retour_pret TIMESTAMP,
                    statut SMALLINT NOT NULL,
                    date_emprunt TIMESTAMP NOT NULL,
                    id_livre INT NOT NULL,
                    id_lecteur INT NOT NULL,
                    FOREIGN KEY (id_livre) REFERENCES livres(id_livre),
                    FOREIGN KEY (id_lecteur) REFERENCES lecteurs(id_lecteur)
                );",

                // Rappel
                @"CREATE TABLE IF NOT EXISTS rappel (
                    id_rappel SERIAL PRIMARY KEY,
                    date_envoie DATE NOT NULL,
                    type SMALLINT NOT NULL CHECK (type IN (0, 1)),
                    status BOOLEAN NOT NULL,
                    id_lecteur INT NOT NULL,
                    FOREIGN KEY (id_lecteur) REFERENCES lecteurs(id_lecteur)
                );"
            };


            foreach (var sql in requetes)
            {
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }

            Debug.WriteLine("✅ Tables vérifiées/créées !");
        }
    }
}
