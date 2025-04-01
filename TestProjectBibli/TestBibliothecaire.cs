using System;
using Xunit;
using bibliothecaire.Model;

namespace Bibliothecaire.Tests
{
    public class TestBibliothecaire
    {
        // Test de la méthode VerifierMotDePasse
        [Theory]
        [InlineData("Motdepasse1!", true)]
        [InlineData("motdepasse", false)]
        [InlineData("MOTDEPASSE", false)]
        [InlineData("Motde1", false)]
        public void Test_VerifierMotDePasse(string input, bool expected)
        {
            var bibliothecaire = new bibliothecaire.Model.Bibliothecaire(1, "Nom", "Prenom", "identifiant", input);
            bool result = bibliothecaire.VerifierMotDePasse(input);
            Assert.Equal(expected, result);
        }

        // Test de la méthode Nom
        [Fact]
        public void Test_Nom_Valid()
        {
            var bibliothecaire = new bibliothecaire.Model.Bibliothecaire(1, "Nom", "Prenom", "identifiant", "Motdepasse1!");
            Assert.Equal("Nom", bibliothecaire.Nom);
        }

        // Test de la méthode MotDePasse (validation de la robustesse)
        [Fact]
        public void Test_MotDePasse_Invalid()
        {
            var exception = Assert.Throws<ArgumentException>(() => 
                new bibliothecaire.Model.Bibliothecaire(1, "Nom", "Prenom", "identifiant", "motdepasse")
            );
            Assert.Equal("Le mot de passe doit contenir au moins 8 caractères, avec une majuscule, un chiffre et un caractère spécial.", exception.Message);
        }
    }
}