namespace ECommerceWebSite.Dto
{
    public class ProductForUpdationDto
    {
        public string? nameOfProduct { get; set; }
        public string? descriptionOfProduct { get; set; }
        public decimal? priceOfProduct { get; set; }
        public int? availableQuantity { get; set; }
        public Guid? idOfCategory { get; set; }
        public IFormFile? product_image { get; set; }



        //public string? nameOfProduct { get; set; }  // Now optional
        //public string? descriptionOfProduct { get; set; }  // Now optional
        //public decimal? priceOfProduct { get; set; }  // Now optional
        //public int? availableQuantity { get; set; }  // Now optional
        //public Guid? idOfCategory { get; set; }  // Now optional
        //public IFormFile? product_image { get; set; }  // Now optional
    }


}
