namespace Discount.API.Entities
{
    public class Coupons
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}
