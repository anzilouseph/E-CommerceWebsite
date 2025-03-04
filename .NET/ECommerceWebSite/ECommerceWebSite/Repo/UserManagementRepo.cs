using Dapper;
using ECommerceWebSite.Context;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.Repo
{
    public class UserManagementRepo : IUserManagementRepo
    {
        private readonly DapperContext _dapperContext;

        public UserManagementRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        //for add user
        public async Task<int> AddUser(UserForCreationDto user,string salt)
        {
            var query = "insert into Users (FullName,Phone,Email,Password,Salt) values (@FullName,@Phone,@Email,@Password,@salt)";
            var parameters = new DynamicParameters();
            parameters.Add("FullName", user.nameOfUser);
            parameters.Add("Phone", user.phoneOfUser);
            parameters.Add("Email", user.emailOfUser);
            parameters.Add("Password", user.passwordOfUser);
            parameters.Add("salt", salt);

            using(var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                return rowAffected;
            }
        }

        //for Get the profile of the user By the Id (only adin can access it)
        public async Task<Users> GetOwnProfile(Guid id)
        {
            var query = "select * from Users where UserId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(query, parameters);
                connection.Close();
                return result;
            }
        }



        //to get the all categories names
        public async Task<APIResponse<IEnumerable<CategoryNamesDto>>> GetCategories()
        {
            var query = "select CategoryName from Category order by CategoryName asc";

            using(var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<CategoryNamesDto>(query);
                connection.Close();
                
                if(result == null)
                {
                    return APIResponse<IEnumerable<CategoryNamesDto>>.Error("No Categories are their");
                }
                return APIResponse<IEnumerable<CategoryNamesDto>>.Success(result, "Success");
            }
        }



    }
}
