

namespace GestionMagasin.Models
{
    public class ArticleManager
    {
        private List<Article> articles = new List<Article>();

        public void AddArticle(Article article)
        {
            articles.Add(article);
        }

        public void UpdateArticle(int id, string newName, decimal newPrice, int newQuantity)
        {
            var article = articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.Name = newName;
                article.Price = newPrice;
                article.Quantity = newQuantity;
            }
        }

        public void DeleteArticle(int id)
        {
            articles.RemoveAll(a => a.Id == id);
        }

        public List<Article> GetArticles()
        {
            return articles;
        }
    }
}
