namespace ECommerceWebSite.Models
{
    public class Wishlist
    {
        public Guid WishlistId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public string AddedAt { get; set; }
       
    }
}
