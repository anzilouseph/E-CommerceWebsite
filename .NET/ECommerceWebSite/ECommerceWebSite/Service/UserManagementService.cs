using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.IService;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.Service
{
    public class UserManagementService :IUserManagementService
    {
        private readonly IUserManagementRepo _repo;
        
        public UserManagementService(IUserManagementRepo repo)
        {
            _repo = repo;
        }

        //for Add User
        public async Task<APIResponse<bool>> AddUser(UserForCreationDto user)
        {
           user.passwordOfUser = forPasswordHasing.HashPassword(user.passwordOfUser,out string salt);

            var rowAffected = await _repo.AddUser(user,salt);

            if(rowAffected==0)
            {
                return APIResponse<bool>.Error("Unable to add user");
            }
            return APIResponse<bool>.Success(true, "User Added Successfully");
        }


        //for User to get their own profile
        public async Task<APIResponse<UserToListDto>> GetOwnProfile(Guid id)
        {
            var result = await _repo.GetOwnProfile(id);
            if (result == null)
            {
                return APIResponse<UserToListDto>.Error("No user in this Id");
            }
            var masked = new UserToListDto()
            {
                idOfUser = result.UserId,
                nameOfUser = result.FullName,
                phoneOfUser = result.Phone,
                emailOfUser = result.Email,
                roleOfUser = result.Role,
            };

            return APIResponse<UserToListDto>.Success(masked, "Success");
        }
    }
}
