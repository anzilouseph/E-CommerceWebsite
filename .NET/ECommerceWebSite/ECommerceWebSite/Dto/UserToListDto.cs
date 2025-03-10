namespace ECommerceWebSite.Dto
{
    public class UserToListDto
    {
        public Guid idOfUser { get; set; }
        public string nameOfUser { get; set; }
        public string phoneOfUser { get; set; }
        public string emailOfUser { get; set; }       
        public string roleOfUser { get; set; }     
        public string profileImage { get; set; }
    }
}
