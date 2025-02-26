using ECommerceWebSite.Dto;
using ECommerceWebSite.Models;

namespace ECommerceWebSite.IRepo
{
    public interface IUserManagementRepo
    {
        public Task<int> AddUser(UserForCreationDto user,string salt);
        public Task<Users> GetOwnProfile(Guid id);  //User Can get own profile
         
    }
}
