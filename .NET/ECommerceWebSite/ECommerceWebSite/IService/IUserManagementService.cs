using ECommerceWebSite.Dto;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.IService
{
    public interface IUserManagementService
    {
        public Task<APIResponse<bool>> AddUser(UserForCreationDto user);
        public Task<APIResponse<UserToListDto>> GetOwnProfile(Guid id); //user can get his own profile


    }
}
