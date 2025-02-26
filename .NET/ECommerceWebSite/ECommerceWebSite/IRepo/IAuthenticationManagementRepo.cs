using ECommerceWebSite.Dto;
using ECommerceWebSite.Models;

namespace ECommerceWebSite.IRepo
{
    public interface IAuthenticationManagementRepo
    {
        public Task<Users> Login(LoginDto log);
    }
}
