using GestionMagasin.Models;

namespace GestionMagasin
{
    public class MainForm : Form
    {
        private ArticleManager articleManager = new ArticleManager();
        private ListBox articlesListBox;
        private Button addButton, editButton, deleteButton;

        public MainForm()
        {
            // Configuration du formulaire
            this.Text = "Gestion des Articles";
            this.Width = 800;
            this.Height = 600;

            // Liste des articles
            articlesListBox = new ListBox
            {
                Width = 600,
                Height = 400,
                Left = 50,
                Top = 50
            };
            this.Controls.Add(articlesListBox);

            // Bouton Ajouter
            addButton = new Button
            {
                Text = "Ajouter",
                Left = 50,
                Top = 470,
                Width = 100
            };
            addButton.Click += AddButton_Click;
            this.Controls.Add(addButton);

            // Bouton Modifier
            editButton = new Button
            {
                Text = "Modifier",
                Left = 160,
                Top = 470,
                Width = 100
            };
            editButton.Click += EditButton_Click;
            this.Controls.Add(editButton);

            // Bouton Supprimer
            deleteButton = new Button
            {
                Text = "Supprimer",
                Left = 270,
                Top = 470,
                Width = 100
            };
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            // Charger les articles au démarrage
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
