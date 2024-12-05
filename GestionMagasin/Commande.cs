using System;
using System.Collections.Generic;

namespace GestionMagasin.Models
{
    public enum EtatCommande
    {
        EnAttente,
        Validee,
        Annulee
    }

    public class Commande
    {
        public Guid IdCommande { get; private set; }
        public List<Panier> Paniers { get; private set; }
        public EtatCommande EtatCommande { get; private set; }

        public Commande()
        {
            IdCommande = Guid.NewGuid();
            Paniers = new List<Panier>();
            EtatCommande = EtatCommande.EnAttente;
        }

        public void AjouterPanier(Panier panier)
        {
            if (panier != null)
            {
                Paniers.Add(panier);
            }
        }

        public void ValiderCommande()
        {
            EtatCommande = EtatCommande.Validee;
        }

        public void AnnulerCommande()
        {
            // Vérifier si la commande est valide avant de l'annuler
            if (EtatCommande == EtatCommande.Validee)
            {
                EtatCommande = EtatCommande.Annulee;
            }
            else
            {
                MessageBox.Show("La commande ne peut pas être annulée car elle n'est pas validée.");
            }
        }

        public decimal CalculerTotal()
        {
            decimal total = 0;
            foreach (var panier in Paniers)
            {
                total += panier.CalculerTotal();
            }
            return total;
        }

        public string GenererRecapitulatif()
        {
            var recap = $"Commande ID : {IdCommande}\nÉtat : {EtatCommande}\n";
            recap += "Articles :\n";

            foreach (var panier in Paniers)
            {
                foreach (var item in panier.Articles)
                {
                    recap += $"- {item.Article.Name} x {item.Quantity} = {item.Article.Price * item.Quantity}€\n";
                }
            }

            recap += $"Total : {CalculerTotal()}€\n";
            return recap;
        }
    }
}