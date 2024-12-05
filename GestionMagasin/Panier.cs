namespace GestionMagasin.Models
{
    public class Panier
    {
        public List<(Article Article, int Quantity, string Etat)> Articles { get; private set; }

        public Panier()
        {
            Articles = new List<(Article, int, string)>();
        }

        public void AjouterArticle(Article article, int quantity)
        {
            if (article.Quantity >= quantity)
            {
                var item = Articles.FirstOrDefault(a => a.Article.Id == article.Id);
                if (item.Article != null)
                {
                    // Si l'article existe déjà, on met à jour sa quantité
                    Articles.Remove(item);
                    Articles.Add((article, item.Quantity + quantity, "Commande invalide"));
                }
                else
                {
                    // Sinon on l'ajoute avec le statut "Commande invalide"
                    Articles.Add((article, quantity, "Commande invalide"));
                }
                article.Retirer(quantity);
            }
        }

        public void MettreAJourStatut(string nouveauStatut)
        {
            for (int i = 0; i < Articles.Count; i++)
            {
                Articles[i] = (Articles[i].Article, Articles[i].Quantity, nouveauStatut);
            }
        }

        public decimal CalculerTotal()
        {
            return Articles.Sum(a => a.Article.Price * a.Quantity);
        }
    }
}