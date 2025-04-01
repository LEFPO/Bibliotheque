using bibliothecaire.Model;

namespace TestProjectBibli;


public class UnitTest1
{
        // Tests mot de passe
        [Theory]
        [InlineData("Motdepasse1!", true)]
        [InlineData("motdepasse", false)]
        [InlineData("MOTDEPASSE", false)]
        [InlineData("Motde1", false)]
        public void Test_VerifierMotDePasse(string input, bool expected)
        {
            var b = new bibliothecaire.Model.Bibliothecaire(1, "Nom", "Prenom", "identifiant", input);
            bool result = b.VerifierMotDePasse(input);
            Assert.Equal(expected, result);
        }

        // Test prêt - date correcte
        [Fact]
        public void Test_CreerPret_DatesValides()
        {
            var pret = new Pret(1, 10, 5, DateTime.Today, DateTime.Today.AddDays(14));
            Assert.Equal(StatutPret.EnCours, pret.Statut);
        }

        // Test prêt - date retour avant date prêt => exception
        [Fact]
        public void Test_CreerPret_DateRetourInvalide()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var pret = new Pret(1, 10, 5, DateTime.Today, DateTime.Today.AddDays(-1));
            });
        }

        // Test clôture de prêt
        [Fact]
        public void Test_CloturerPret_Succes()
        {
            var pret = new Pret(1, 10, 5, DateTime.Today, DateTime.Today.AddDays(10));
            pret.CloturerPret();
            Assert.Equal(StatutPret.Termine, pret.Statut);
        }

        [Fact]
        public void Test_CloturerPret_DejaTermine()
        {
            var pret = new Pret(1, 10, 5, DateTime.Today, DateTime.Today.AddDays(10));
            pret.CloturerPret();
            Assert.Throws<InvalidOperationException>(() => pret.CloturerPret());
        }

        // Historique - Vérification du statut en retard
        [Fact]
        public void Test_Historique_VerifierRetard()
        {
            var hist = new Historique
            {
                IdLivre = 1,
                IdLecteur = 2,
                DateEmprunt = DateTime.Today.AddDays(-20),
                DateRetour = DateTime.Today.AddDays(-5),
                Statut = StatutHistorique.EnCours
            };

            hist.VerifierRetard();
            Assert.Equal(StatutHistorique.EnRetard, hist.Statut);
        }

        [Fact]
        public void Test_Historique_CloturerTermine()
        {
            var hist = new Historique
            {
                IdLivre = 1,
                IdLecteur = 2,
                DateEmprunt = DateTime.Today.AddDays(-10),
                DateRetour = DateTime.Today.AddDays(5),
                Statut = StatutHistorique.EnCours
            };

            hist.CloturerHistorique();
            Assert.Equal(StatutHistorique.Termine, hist.Statut);
        }

        [Fact]
        public void Test_Historique_CloturerDejaTermine()
        {
            var hist = new Historique
            {
                IdLivre = 1,
                IdLecteur = 2,
                DateEmprunt = DateTime.Today.AddDays(-10),
                DateRetour = DateTime.Today.AddDays(5),
                Statut = StatutHistorique.Termine
            };

            Assert.Throws<InvalidOperationException>(() => hist.CloturerHistorique());
        }

}