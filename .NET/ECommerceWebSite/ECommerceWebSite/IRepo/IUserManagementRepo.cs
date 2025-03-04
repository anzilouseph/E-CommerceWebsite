using ECommerceWebSite.Dto;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.IRepo
{
    public interface IUserManagementRepo
    {
        public Task<int> AddUser(UserForCreationDto user,string salt);
        public Task<Users> GetOwnProfile(Guid id);  //User Can get own profile


        public Task<APIResponse<IEnumerable<CategoryNamesDto>>> GetCategories();  //to get the all categories names
         
    }
}
