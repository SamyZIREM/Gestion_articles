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
            this.Width = 450;
            this.Height = 300;

            // Marges pour l'alignement
            int labelWidth = 100;
            int inputWidth = 200;
            int marginTop = 20;
            int marginLeftLabel = 50;
            int marginLeftInput = marginLeftLabel + labelWidth + 10;

            // Champ Nom
            var nameLabel = new Label
            {
                Text = "Nom :",
                Left = marginLeftLabel,
                Top = marginTop,
                Width = labelWidth
            };
            this.Controls.Add(nameLabel);

            nameTextBox = new TextBox
            {
                Left = marginLeftInput,
                Top = marginTop - 3,
                Width = inputWidth
            };
            this.Controls.Add(nameTextBox);

            // Champ Prix
            var priceLabel = new Label
            {
                Text = "Prix :",
                Left = marginLeftLabel,
                Top = marginTop + 40,
                Width = labelWidth
            };
            this.Controls.Add(priceLabel);

            priceTextBox = new TextBox
            {
                Left = marginLeftInput,
                Top = marginTop + 37,
                Width = inputWidth
            };
            this.Controls.Add(priceTextBox);

            // Champ Quantité
            var quantityLabel = new Label
            {
                Text = "Quantité :",
                Left = marginLeftLabel,
                Top = marginTop + 80,
                Width = labelWidth
            };
            this.Controls.Add(quantityLabel);

            quantityTextBox = new TextBox
            {
                Left = marginLeftInput,
                Top = marginTop + 77,
                Width = inputWidth
            };
            this.Controls.Add(quantityTextBox);

            // Bouton Sauvegarder
            saveButton = new Button
            {
                Text = "Sauvegarder",
                Left = (this.Width / 2) - 50, // Centré
                Top = marginTop + 130,
                Width = 100
            };
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
                    articleManager.AddArticle(new Article(
                        new Random().Next(1000, 9999),  
                        nameTextBox.Text,           
                        price,                       
                        quantity                       
                    ));
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
