using Dapper;
using ECommerceWebSite.Context;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.Models;

namespace ECommerceWebSite.Repo
{
    public class AuthenticationManagementRepo : IAuthenticationManagementRepo
    {
        private readonly DapperContext _context;

        public AuthenticationManagementRepo(DapperContext context)
        {
            _context = context;
        }


        //for Login
        public async Task<Users> Login(LoginDto log)
        {
            var query = "select * from Users where Email=@email";
            var parameters = new DynamicParameters();
            parameters.Add("email",log.emailOfUser);
            
            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(query, parameters);
                connection.Close();
                return result;
            }
        }

    }
}
