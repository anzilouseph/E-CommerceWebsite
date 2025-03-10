namespace ECommerceWebSite.Dto
{
    public class UserRegistrationDto
    {
        public string nameOfUser { get; set; }  
        public string phnOfUser { get; set; }
        public string emailOfUser { get; set; }
        public string passwordOfUser { get; set; }  
        public IFormFile image {  get; set; }
    }
}
