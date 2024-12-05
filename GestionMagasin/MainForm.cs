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

        private Button validateOrderButton;
        private Commande commande = new Commande();

        private Button cancelOrderButton;

        public MainForm()
        {

            validateOrderButton = new Button
            {
                Text = "Valider la Commande",
                Left = 750,
                Top = 510,
                Width = 200
            };
            validateOrderButton.Click += ValidateOrderButton_Click;
            this.Controls.Add(validateOrderButton);

            // Ajoutez le bouton "Annuler la Commande"
            cancelOrderButton = new Button
            {
                Text = "Annuler la Commande",
                Left = 750,
                Top = 550,
                Width = 200
            };
            cancelOrderButton.Click += CancelOrderButton_Click;
            this.Controls.Add(cancelOrderButton);

            // Configuration du formulaire
            this.Text = "Gestion des Articles et du Panier";
            this.Width = 1000;
            this.Height = 700;

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
                Text = "Ajouter au Panier ->",
                Left = 400,
                Top = 300,
                Width = 150
            };
            addToCartButton.Click += AddToCartButton_Click;
            this.Controls.Add(addToCartButton);
            addToCartButton.BringToFront();

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
                Text = "Panier (prix total)",
                Left = 500,
                Top = 40,
                Width = 200
            };
            this.Controls.Add(panierLabel);

            // Bouton Retirer du Panier
            removeFromCartButton = new Button
            {
                Text = "Retirer du Panier",
                Left = 600,
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
                Top = 500,
                Width = 150
            };
            addButton.Click += AddButton_Click;
            this.Controls.Add(addButton);

            editButton = new Button
            {
                Text = "Modifier Article",
                Left = 210,
                Top = 500,
                Width = 150
            };
            editButton.Click += EditButton_Click;
            this.Controls.Add(editButton);

            deleteButton = new Button
            {
                Text = "Supprimer Article",
                Left = 370,
                Top = 500,
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
            panierListBox.Items.Clear();

            foreach (var (article, quantity, etat) in panier.Articles)
            {
                panierListBox.Items.Add($"{article.Name} x {quantity} - {article.Price * quantity}€ - {etat}");
            }

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

        private void ValidateOrderButton_Click(object sender, EventArgs e)
        {
            if (panier.Articles.Count == 0)
            {
                MessageBox.Show("Votre panier est vide. Ajoutez des articles avant de valider la commande.");
                return;
            }

            // Créer une nouvelle commande
            commande = new Commande();
            commande.AjouterPanier(panier); // Associer le panier à la commande

            // Valider la commande
            commande.ValiderCommande(); // Assurez-vous que l'état est bien mis à jour

            // Générer un récapitulatif
            string recapitulatif = $"Commande ID : {commande.IdCommande}\n";
            recapitulatif += "Récapitulatif de votre commande :\n";
            foreach (var (article, quantity, etat) in panier.Articles)
            {
                recapitulatif += $"- {article.Name} x {quantity} = {article.Price * quantity}€\n";
            }
            recapitulatif += $"Total : {panier.CalculerTotal()}€";

            // Afficher le récapitulatif
            MessageBox.Show(recapitulatif, "Commande validée");

            // Mettre à jour le statut des articles
            panier.MettreAJourStatut("Validée");

            // Rafraîchir le panier
            RefreshCart();
        }

        private void CancelOrderButton_Click(object sender, EventArgs e)
        {
            if (commande.EtatCommande == EtatCommande.Validee)
            {
                // Confirmer l'annulation
                var confirmation = MessageBox.Show("Êtes-vous sûr de vouloir annuler la commande ?", "Annulation de commande", MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    commande.AnnulerCommande(); // Annuler la commande
                    panier.MettreAJourStatut("Annulée"); // Mettre à jour le statut des articles du panier

                    // Rafraîchir l'affichage du panier
                    RefreshCart();

                    MessageBox.Show("La commande a été annulée.");
                }
            }
            else if (commande.EtatCommande == EtatCommande.Annulee)
            {
                MessageBox.Show("Cette commande a déjà été annulée.");
            }
            else
            {
                MessageBox.Show("La commande doit être validée avant d'être annulée.");
            }
        }




    }
}