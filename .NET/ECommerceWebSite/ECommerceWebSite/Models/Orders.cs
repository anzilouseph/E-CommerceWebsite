namespace ECommerceWebSite.Models
{
    public class Orders
    {
        public Guid OrderId { get; set; }   
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
    }
}
