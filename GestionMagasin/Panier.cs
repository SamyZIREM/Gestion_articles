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
            var item = Articles.FirstOrDefault(a => a.Article.Id == article.Id);
            if (item.Article != null)
            {
                if (item.Quantity > quantity)
                {
                    item.Quantity -= quantity;
                }
                else
                {
                    Articles.Remove(item);
                }
                article.Ajouter(quantity);
            }
        }

        public decimal CalculerTotal()
        {
            return Articles.Sum(a => a.Article.Price * a.Quantity);
        }
    }
}
