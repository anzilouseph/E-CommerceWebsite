using ECommerceWebSite.Dto;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.IService
{
    public interface IAuthenticationManagementService 
    {
        public Task<APIResponse<string>> Login(LoginDto log); 
    }
}
