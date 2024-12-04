using System;
using System.Windows.Forms;
using GestionMagasin.Models;

namespace GestionMagasin
{
    public class ArticleForm : Form
    {
        private TextBox nameTextBox, priceTextBox, quantityTextBox;
        private Button saveButton;
        private ArticleManager articleManager;
        private Article article;

        public ArticleForm(ArticleManager manager, Article existingArticle)
        {
            articleManager = manager;
            article = existingArticle;

            // Configuration du formulaire
            this.Text = article == null ? "Ajouter un Article" : "Modifier un Article";
            this.Width = 400;
            this.Height = 300;

            // Champ Nom
            var nameLabel = new Label { Text = "Nom :", Left = 20, Top = 20 };
            this.Controls.Add(nameLabel);

            nameTextBox = new TextBox { Left = 100, Top = 20, Width = 250 };
            this.Controls.Add(nameTextBox);

            // Champ Prix
            var priceLabel = new Label { Text = "Prix :", Left = 20, Top = 60 };
            this.Controls.Add(priceLabel);

            priceTextBox = new TextBox { Left = 100, Top = 60, Width = 250 };
            this.Controls.Add(priceTextBox);

            // Champ Quantité
            var quantityLabel = new Label { Text = "Quantité :", Left = 20, Top = 100 };
            this.Controls.Add(quantityLabel);

            quantityTextBox = new TextBox { Left = 100, Top = 100, Width = 250 };
            this.Controls.Add(quantityTextBox);

            // Bouton Sauvegarder
            saveButton = new Button { Text = "Sauvegarder", Left = 100, Top = 150, Width = 100 };
            saveButton.Click += SaveButton_Click;
            this.Controls.Add(saveButton);

            // Charger les données si modification
            if (article != null)
            {
                nameTextBox.Text = article.Name;
                priceTextBox.Text = article.Price.ToString();
                quantityTextBox.Text = article.Quantity.ToString();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(priceTextBox.Text, out var price) && int.TryParse(quantityTextBox.Text, out var quantity))
            {
                if (article == null)
                {
                    articleManager.AddArticle(new Article
                    {
                        Id = new Random().Next(1000, 9999),
                        Name = nameTextBox.Text,
                        Price = price,
                        Quantity = quantity
                    });
                }
                else
                {
                    articleManager.UpdateArticle(article.Id, nameTextBox.Text, price, quantity);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez entrer des valeurs valides pour le prix et la quantité.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
