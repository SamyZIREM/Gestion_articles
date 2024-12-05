namespace GestionMagasin.Models{
    public class Promotion
{
    private Dictionary<string, decimal> promotions;

    public Promotion()
    {
        promotions = new Dictionary<string, decimal>
            {
                { "PROMO10", 10 }, // redudction de 10 euro par exemple
                { "PROMO20", 20 },
                { "PROMO50", 50 }
            };
    }

    public decimal GetPromotionValue(string code)
    {
        if (promotions.ContainsKey(code.ToUpper()))
        {
            return promotions[code.ToUpper()];
        }
        return 0;
    }
}

} 