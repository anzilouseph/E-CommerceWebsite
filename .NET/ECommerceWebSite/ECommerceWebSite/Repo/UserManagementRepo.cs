using System.Linq;
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
        private readonly IWebHostEnvironment _env;
        public UserManagementRepo(DapperContext dapperContext, IWebHostEnvironment env)
        {
            _dapperContext = dapperContext;
            _env = env;
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
            var query = "select CategoryId,CategoryName from Category order by CategoryName asc";

            using(var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<CategoryNamesDto>(query);
                connection.Close();
                
                if(result.Count()==0)
                {
                    return APIResponse<IEnumerable<CategoryNamesDto>>.Error("No Categories are their");
                }
                return APIResponse<IEnumerable<CategoryNamesDto>>.Success(result, "Success");
            }
        }



        //for UserRegistration(Including image)
        public async Task<APIResponse<bool>> UserRegistration(UserRegistrationDto user)  //for UserRegistration(Including image)
        {
            string filePath = string.Empty;
            string newFileName = string.Empty;
            if (user.image != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "Media", "UserImages");


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                var fileExtension = Path.GetExtension(user.image.FileName).ToLower();

                var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return APIResponse<bool>.Error("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                }
                newFileName = $"{user.emailOfUser}{fileExtension}";
                filePath = Path.Combine(folderPath, newFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await user.image.CopyToAsync(stream)
;
                }
                Console.WriteLine(user.image.FileName);
                Console.WriteLine(filePath);
                Console.WriteLine(newFileName);
            }

            user.passwordOfUser = forPasswordHasing.HashPassword(user.passwordOfUser, out string salt);
            var query = "insert into users (FullName,Phone,Email,Password,Salt,ProfileImage) values (@FullName,@Phone,@Email,@Password,@Salt,@ProfileImage)";
            var parameters = new DynamicParameters();

            parameters.Add("FullName",user.nameOfUser);
            parameters.Add("Phone", user.phnOfUser);
            parameters.Add("Email", user.emailOfUser);
            parameters.Add("Password", user.passwordOfUser);
            parameters.Add("Salt", salt);
            parameters.Add("ProfileImage", newFileName);

            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query,parameters);
                connection.Close();
                if(rowAffected == 0)
                {
                    return APIResponse<bool>.Error("Unable to insert");
                }
                return APIResponse<bool>.Success(true, "User Added SuccessFully");

            }

        }

        ////for getting a user by his ID;
        //public async Task<APIResponse<UserToListDto>> GetUserById(Guid id)
        //{
        //    var query = "select * from Users where UserId = @id";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("id",id);

        //    using(var connection =  _dapperContext.CreateConnection())
        //    {
        //        connection.Open();
        //        var result = await connection.QueryFirstOrDefaultAsync<Users>(query,parameters);
        //        connection.Close ();

        //        if(result == null)
        //        {
        //            return APIResponse<UserToListDto>.Error("No User in this ID");
        //        }
        //        var masked = new UserToListDto()
        //        {
        //            idOfUser = result.UserId,
        //            nameOfUser = result.FullName,
        //            phoneOfUser = result.Phone,
        //            emailOfUser = result.Email,
        //            roleOfUser = result.Role,
        //            profileImage = result.profileImage,
        //        };
        //        return APIResponse<UserToListDto>.Success(masked, "Success");
        //    }
        //}
    }
}
