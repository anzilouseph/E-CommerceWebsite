using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.IService;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.Service
{
    public class AuthenticationManagementService : IAuthenticationManagementService
    {
        private readonly IAuthenticationManagementRepo _repo;
        private readonly IConfiguration _config;
        public AuthenticationManagementService(IAuthenticationManagementRepo repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        //for login
        public async Task<APIResponse<string>> Login(LoginDto log)
        {
            var result = await _repo.Login(log);

            if(result == null)
            {
                return APIResponse<string>.Error("Invalid Email");
            }
            var verfyPasswordVariable = forPasswordHasing.VerifyPassword(log.passwordOfUser, result.Password, result.Salt);
            if(!verfyPasswordVariable)
            {
                return APIResponse<string>.Error("Password is Incorrect");
            }
            var masked = new UserToListDto()
            {
                idOfUser = result.UserId,
                nameOfUser = result.FullName,
                phoneOfUser = result.Phone,
                emailOfUser = result.Email,
                roleOfUser = result.Role,
            };

            var tokenGenerate = new GenerateJWT(_config);

            var accessToken = tokenGenerate.GenerateToken(masked);

            if(accessToken == null)
            {
                return APIResponse<string>.Error(" Failed to generate Token "); 
            }
            return APIResponse<string>.Success(accessToken, "success");


        }

    }
}
