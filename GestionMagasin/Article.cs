namespace GestionMagasin.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Article(int id, string name, decimal price, int quantity)
        {
            Id =new Random().Next(1000, 9999);
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public void Ajouter(int quantity)
        {
            Quantity += quantity;
        }

        public void Retirer(int quantity)
        {
            if (Quantity >= quantity)
                Quantity -= quantity;
        }

        public string Afficher()
        {
            return $"{Name} - {Price}€ - {Quantity} unités disponibles";
        }
    }
}
