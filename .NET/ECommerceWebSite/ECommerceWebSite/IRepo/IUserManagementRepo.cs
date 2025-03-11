using Dapper;
using ECommerceWebSite.Dto;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;
using MySqlX.XDevAPI.CRUD;

namespace ECommerceWebSite.IRepo
{
    public interface IUserManagementRepo
    {
        public Task<int> AddUser(UserForCreationDto user,string salt);
        public Task<Users> GetOwnProfile(Guid id);  //User Can get own profile
        public Task<APIResponse<bool>> UserRegistration(UserRegistrationDto user);//for UserRegistration(Including image)
        public Task<APIResponse<bool>> UpdateUserDetails(UserDetailsForUpdation user,Guid userid); //for update the details of the user(Without Image);
        public Task<APIResponse<bool>> UpdateUserProfileImage(UserProfileImageForUpdation user, Guid userid); //Here we only changing te Profile Image of the User
        public Task<APIResponse<bool>> DeleteProfileImage(Guid userid); //for remove the profile image of the user
        
        
        
        
        public Task<APIResponse<IEnumerable<CategoryNamesDto>>> GetCategories();  //to get the all categories names

    }
}
