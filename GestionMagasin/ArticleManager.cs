using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GestionMagasin.Models
{
    public class ArticleManager
    {
        private List<Article> articles = new List<Article>();
        private string filePath = "articles.json"; // Chemin du fichier JSON

        public void AddArticle(Article article)
        {
            articles.Add(article);
            SaveArticles();  // Sauvegarder après ajout
        }

        public void UpdateArticle(int id, string newName, decimal newPrice, int newQuantity)
        {
            var article = articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.Name = newName;
                article.Price = newPrice;
                article.Quantity = newQuantity;
                SaveArticles();  // Sauvegarder après modification
            }
        }

        public void DeleteArticle(int id)
        {
            articles.RemoveAll(a => a.Id == id);
            SaveArticles();  // Sauvegarder après suppression
        }

        public List<Article> GetArticles()
        {
            LoadArticles(); // Charger les articles avant de les retourner
            return articles;
        }

        // Sauvegarder les articles dans un fichier JSON
        private void SaveArticles()
        {
            try
            {
                var json = JsonSerializer.Serialize(articles);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la sauvegarde des articles : " + ex.Message);
            }
        }

        // Charger les articles à partir du fichier JSON
        private void LoadArticles()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    articles = JsonSerializer.Deserialize<List<Article>>(json) ?? new List<Article>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors du chargement des articles : " + ex.Message);
                }
            }
        }
    }
}
