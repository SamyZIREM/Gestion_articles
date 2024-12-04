using System.Windows.Forms;
using GestionMagasin.Models;

namespace GestionMagasin
{
    public class MainForm : Form
    {
        private ArticleManager articleManager = new ArticleManager();
        private Panier panier = new Panier();

        private ListBox articlesListBox, panierListBox;
        private Button addToCartButton, removeFromCartButton, viewCartButton;
        private Button addButton, editButton, deleteButton;  // Boutons pour gestion des articles
        private Label articlesLabel, panierLabel;  // Labels pour les titres des sections

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
                Top = 70 // Déplacer la liste des articles un peu plus bas pour faire de la place pour le titre
            };
            this.Controls.Add(articlesListBox);

            // Titre "Articles ajoutés"
            articlesLabel = new Label
            {
                Text = "Articles ajoutés",
                Left = 50,
                Top = 40, // Position du titre au-dessus de la liste des articles
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
                Top = 70 // Déplacer la liste du panier un peu plus bas pour faire de la place pour le titre
            };
            this.Controls.Add(panierListBox);

            // Titre "Panier"
            panierLabel = new Label
            {
                Text = "Panier",
                Left = 500,
                Top = 40, // Position du titre au-dessus de la liste du panier
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

            // Bouton Afficher le Total
            viewCartButton = new Button
            {
                Text = "Afficher le Total",
                Left = 660,
                Top = 470,
                Width = 150
            };
            viewCartButton.Click += ViewCartButton_Click;
            this.Controls.Add(viewCartButton);

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
                Left = 210,  // Espace de 10px après le bouton Ajouter
                Top = 530,
                Width = 150
            };
            editButton.Click += EditButton_Click;
            this.Controls.Add(editButton);

            deleteButton = new Button
            {
                Text = "Supprimer Article",
                Left = 370,  // Espace de 10px après le bouton Modifier
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
        }

        private void RefreshCart()
        {
            panierListBox.Items.Clear();
            foreach (var item in panier.Articles)
            {
                panierListBox.Items.Add($"{item.Article.Name} x {item.Quantity} - {item.Article.Price * item.Quantity}€");
            }
        }

        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            if (articlesListBox.SelectedIndex >= 0)
            {
                var selectedArticle = articleManager.GetArticles()[articlesListBox.SelectedIndex];
                var quantity = 1; // Par défaut, ajouter 1 article (ajouter un formulaire pour spécifier une quantité)
                panier.AjouterArticle(selectedArticle, quantity);
                LoadArticles();
                RefreshCart();
            }
        }

        private void RemoveFromCartButton_Click(object sender, EventArgs e)
        {
            if (panierListBox.SelectedIndex >= 0)
            {
                var selectedItem = panier.Articles[panierListBox.SelectedIndex];
                panier.RetirerArticle(selectedItem.Article, 1); // Par défaut, retirer 1 article
                LoadArticles();
                RefreshCart();
            }
        }

        private void ViewCartButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Total du Panier : {panier.CalculerTotal()}€", "Total", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
