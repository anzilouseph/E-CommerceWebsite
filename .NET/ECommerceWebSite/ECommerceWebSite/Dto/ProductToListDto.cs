namespace ECommerceWebSite.Dto
{
    public class ProductToListDto
    {
        public Guid idOfProduct { get; set; }
        public string nameOfProduct { get; set; }
        public string descriptionOfProduct { get; set; }
        public decimal priceOfProduct { get; set; }
        public int availableQuantity { get; set; }
        public Guid idOfCategory { get; set; }
        public string urlOfImage { get; set; }
    }
}
