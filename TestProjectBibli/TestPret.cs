using System;
using Xunit;
using bibliothecaire.Model;

namespace Bibliothecaire.Tests
{
    public class TestPret
    {
        // Test de la méthode VérifierRetard() - Vérification du statut en retard
        [Fact]
        public void Test_VerifierRetard_PretEnRetard()
        {
            var pret = new Pret(1, 10, 5, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-5));
            pret.VerifierRetard();
            Assert.Equal(StatutPret.EnRetard, pret.Statut);
        }

        // Test de la méthode CloturerPret() - Clôture d'un prêt
        [Fact]
        public void Test_CloturerPret_Success()
        {
            var pret = new Pret(1, 10, 5, DateTime.Today, DateTime.Today.AddDays(10));
            pret.CloturerPret();
            Assert.Equal(StatutPret.Termine, pret.Statut);
        }

        // Test de la méthode CloturerPret() - Tentative de clôturer un prêt déjà terminé
        [Fact]
        public void Test_CloturerPret_DejaTermine()
        {
            var pret = new Pret(1, 10, 5, DateTime.Today, DateTime.Today.AddDays(10));
            pret.CloturerPret();
            Assert.Throws<InvalidOperationException>(() => pret.CloturerPret());
        }

        // Test de la méthode DateRetourPret - Vérification de la date retour valide
        [Fact]
        public void Test_CreerPret_DateRetourInvalide()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Pret(1, 10, 5, DateTime.Today, DateTime.Today.AddDays(-1));
            });
        }
    }
}