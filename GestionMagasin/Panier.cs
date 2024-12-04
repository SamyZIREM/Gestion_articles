namespace GestionMagasin.Models
{
    public class Panier
    {
        public List<(Article Article, int Quantity)> Articles { get; private set; }

        public Panier()
        {
            Articles = new List<(Article, int)>();
        }

        public void AjouterArticle(Article article, int quantity)
        {
            if (article.Quantity >= quantity)
            {
                var item = Articles.FirstOrDefault(a => a.Article.Id == article.Id);
                if (item.Article != null)
                {
                    item.Quantity += quantity;
                }
                else
                {
                    Articles.Add((article, quantity));
                }
                article.Retirer(quantity);
            }
        }

        public void RetirerArticle(Article article, int quantity)
        {
            // Chercher l'élément du panier
            var panierItem = Articles.FirstOrDefault(p => p.Article.Id == article.Id);

            // Vérifier si panierItem est une valeur par défaut (l'élément n'a pas été trouvé)
            if (!panierItem.Equals(default((Article Article, int Quantity))))
            {
                // Vérifier si la quantité à retirer est valide
                if (quantity <= panierItem.Quantity)
                {
                    panierItem.Quantity -= quantity;

                    // Si la quantité devient 0 ou moins, retirer l'article du panier
                    if (panierItem.Quantity <= 0)
                    {
                        Articles.Remove(panierItem);
                    }

                    // Ajouter la quantité retirée à l'article (dans le stock)
                    article.Quantity += quantity;
                }
                else
                {
                    MessageBox.Show("La quantité à retirer est supérieure à la quantité présente dans le panier.");
                }
            }
            else
            {
                MessageBox.Show("Cet article n'est pas dans le panier.");
            }
        }

        public decimal CalculerTotal()
        {
            return Articles.Sum(a => a.Article.Price * a.Quantity);
        }
    }
}
