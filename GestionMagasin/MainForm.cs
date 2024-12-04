using System;
using System.Windows.Forms;
using GestionMagasin.Models;

namespace GestionMagasin
{
    public class MainForm : Form
    {
        private ArticleManager articleManager = new ArticleManager();
        private Panier panier = new Panier();

        private ListBox articlesListBox, panierListBox;
        private Button addToCartButton, removeFromCartButton;
        private Button addButton, editButton, deleteButton;
        private Label articlesLabel, panierLabel, totalLabel;

        public MainForm()
        {
            // Configuration du formulaire
            this.Text = "Gestion des Articles et du Panier";
            this.Width = 1000;
            this.Height = 600;

            // Liste des articles
            articlesListBox = new ListBox
            {
                Width = 400,
                Height = 400,
                Left = 50,
                Top = 70
            };
            this.Controls.Add(articlesListBox);

            // Titre "Articles ajoutés"
            articlesLabel = new Label
            {
                Text = "Articles ajoutés (prix à l'unité)",
                Left = 50,
                Top = 40,
                Width = 200
            };
            this.Controls.Add(articlesLabel);

            // Bouton Ajouter au Panier
            addToCartButton = new Button
            {
                Text = "Ajouter au Panier",
                Left = 50,
                Top = 470,
                Width = 150
            };
            addToCartButton.Click += AddToCartButton_Click;
            this.Controls.Add(addToCartButton);

            // Liste du panier
            panierListBox = new ListBox
            {
                Width = 400,
                Height = 400,
                Left = 500,
                Top = 70
            };
            this.Controls.Add(panierListBox);

            // Titre "Panier"
            panierLabel = new Label
            {
                Text = "Panier",
                Left = 500,
                Top = 40,
                Width = 200
            };
            this.Controls.Add(panierLabel);

            // Bouton Retirer du Panier
            removeFromCartButton = new Button
            {
                Text = "Retirer du Panier",
                Left = 500,
                Top = 470,
                Width = 150
            };
            removeFromCartButton.Click += RemoveFromCartButton_Click;
            this.Controls.Add(removeFromCartButton);

            // Label pour afficher le total du panier
            totalLabel = new Label
            {
                Text = "Total : 0€",
                Left = 660,
                Top = 510,
                Width = 150
            };
            this.Controls.Add(totalLabel);

            // Boutons pour la gestion des articles
            addButton = new Button
            {
                Text = "Ajouter Article",
                Left = 50,
                Top = 530,
                Width = 150
            };
            addButton.Click += AddButton_Click;
            this.Controls.Add(addButton);

            editButton = new Button
            {
                Text = "Modifier Article",
                Left = 210,
                Top = 530,
                Width = 150
            };
            editButton.Click += EditButton_Click;
            this.Controls.Add(editButton);

            deleteButton = new Button
            {
                Text = "Supprimer Article",
                Left = 370,
                Top = 530,
                Width = 150
            };
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            // Charger les articles
            LoadArticles();
        }

        private void LoadArticles()
        {
            articlesListBox.Items.Clear();
            foreach (var article in articleManager.GetArticles())
            {
                articlesListBox.Items.Add($"{article.Name} - {article.Price}€ - {article.Quantity} unités");
            }

            // Actualiser le total du panier
            UpdateTotal();
        }


        private void RemoveFromCartButton_Click(object sender, EventArgs e)
        {
            if (panierListBox.SelectedIndex >= 0)
            {
                // Supprimer l'article de la liste
                panier.Articles.RemoveAt(panierListBox.SelectedIndex);

                // Rafraîchir l'affichage du panier
                RefreshCart();
            }
        }

        private void RefreshCart()
        {
            panierListBox.Items.Clear(); // Effacer les anciens éléments

            foreach (var item in panier.Articles)
            {
                // Affichage de l'article et de sa quantité
                panierListBox.Items.Add($"{item.Article.Name} x {item.Quantity} - {item.Article.Price * item.Quantity}€");
            }

            // Actualiser le total du panier
            UpdateTotal();
        }


        private void UpdateTotal()
        {
            totalLabel.Text = $"Total : {panier.CalculerTotal()}€";
        }

        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            if (articlesListBox.SelectedIndex >= 0)
            {
                var selectedArticle = articleManager.GetArticles()[articlesListBox.SelectedIndex];

                // Créer une fenêtre pour la saisie de la quantité
                var quantityForm = new Form
                {
                    Text = "Quantité à ajouter",
                    Width = 300,
                    Height = 200
                };

                var label = new Label
                {
                    Text = $"Quantité de {selectedArticle.Name} à ajouter :",
                    Left = 10,
                    Top = 10,
                    Width = 250
                };
                quantityForm.Controls.Add(label);

                // TextBox pour la saisie manuelle de la quantité
                var quantityTextBox = new TextBox
                {
                    Left = 10,
                    Top = 40,
                    Width = 100,
                    Text = "1" // Valeur par défaut
                };
                quantityForm.Controls.Add(quantityTextBox);

                var okButton = new Button
                {
                    Text = "OK",
                    Left = 100,
                    Top = 100,
                    Width = 100
                };
                okButton.Click += (s, args) =>
                {
                    int quantity;
                    if (int.TryParse(quantityTextBox.Text, out quantity) && quantity > 0 && quantity <= selectedArticle.Quantity)
                    {
                        panier.AjouterArticle(selectedArticle, quantity);
                        LoadArticles();
                        RefreshCart();
                        quantityForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Quantité invalide. Veuillez saisir un nombre positif dans la plage disponible.");
                    }
                };
                quantityForm.Controls.Add(okButton);

                quantityForm.ShowDialog();
            }
        }


        // Gestion des articles (ajouter, modifier, supprimer)
        private void AddButton_Click(object sender, EventArgs e)
        {
            var form = new ArticleForm(articleManager, null);
            form.FormClosed += (s, args) => LoadArticles();
            form.ShowDialog();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (articlesListBox.SelectedIndex >= 0)
            {
                var selectedArticle = articleManager.GetArticles()[articlesListBox.SelectedIndex];
                var form = new ArticleForm(articleManager, selectedArticle);
                form.FormClosed += (s, args) => LoadArticles();
                form.ShowDialog();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (articlesListBox.SelectedIndex >= 0)
            {
                var selectedArticle = articleManager.GetArticles()[articlesListBox.SelectedIndex];
                articleManager.DeleteArticle(selectedArticle.Id);
                LoadArticles();
            }
        }
    }
}
