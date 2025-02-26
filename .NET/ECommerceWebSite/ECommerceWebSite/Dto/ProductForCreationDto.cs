using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Dto
{
    public class ProductForCreationDto
    {
       
        public string nameOfProduct { get; set; }
        public string descriptionOfProduct { get; set; }
        public decimal priceOfProduct { get; set; }
        public int availableQuantity { get; set; }
        public string idOfCategory { get; set; }
        public IFormFile product_image { get; set; }
    }
}
