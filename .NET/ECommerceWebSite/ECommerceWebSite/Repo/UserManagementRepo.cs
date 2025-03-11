using System.Linq;
using Dapper;
using ECommerceWebSite.Context;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;
using Microsoft.OpenApi.Validations;
using Org.BouncyCastle.Crypto.Prng;

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

        //for update the details of the user(Without Image);
        public async Task<APIResponse<bool>> UpdateUserDetails(UserDetailsForUpdation user, Guid userid)
        {
            var queryBuilder = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("id", userid);
            var binded = new Users()
            {
                FullName = user.nameOfUser,
                Phone = user.phoneOfUser,
                Email = user.emailOfUser,
            };

            foreach(var prop in typeof(Users).GetProperties())
            {
                if(prop.Name!="UserId" && prop.Name!= "Password" && prop.Name != "Role" && prop.Name != "Salt" && prop.Name != "ProfileImage")
                {
                    var value = prop.GetValue(binded);
                    if(value != null)
                    {
                        if(!string.IsNullOrEmpty(value.ToString()))
                        {
                            queryBuilder.Add($"{prop.Name}=@{prop.Name}");
                            parameters.Add($"{prop.Name}", value);
                        }
                       
                    }
                }
            }
            var query = $"update Users set {string.Join(",", queryBuilder)} where UserId =@id";
            using(var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.QueryFirstOrDefaultAsync(query,parameters);
                connection.Close();
                if(rowAffected ==0)
                {
                    return APIResponse<bool>.Error("Unable to Insert Data");
                }
                return APIResponse<bool>.Success(true, "Success");
            }
        }

        //Here we only changing te Profile Image of the User
        public async Task<APIResponse<bool>> UpdateUserProfileImage(UserProfileImageForUpdation user, Guid userid)
        {
            string filePath = string.Empty;
            string newFileName = string.Empty;

            var parameters = new DynamicParameters();
            var retrevingQuery = "select * from Users where UserId = @userid";
            parameters.Add("userid",userid);
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var existingData = await connection.QueryFirstOrDefaultAsync<Users>(retrevingQuery,parameters);
                connection.Close();
                if(existingData ==null)
                {
                    return APIResponse<bool>.Error("No user in this ID");
                }


                
                if (user.profileimage != null)
                {
                    var folderPath = Path.Combine(_env.WebRootPath, "Media", "UserImages");


                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }


                    var fileExtension = Path.GetExtension(user.profileimage.FileName).ToLower();

                    var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return APIResponse<bool>.Error("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                    }
                    newFileName = $"{existingData.Email}{fileExtension}";
                    filePath = Path.Combine(folderPath, newFileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.profileimage.CopyToAsync(stream)
    ;
                    }
                    Console.WriteLine(user.profileimage.FileName);
                    Console.WriteLine(filePath);
                    Console.WriteLine(newFileName);
                }
                else
                {
                    return APIResponse<bool>.Error("No Imagees selected");
                }

                var query = "update Users set ProfileImage=@newFileName where UserId=@userid";
                parameters.Add("newFileName", newFileName);

                connection.Close();
                var result = await connection.ExecuteAsync(query,parameters);
                connection.Close();
                if(result==0)
                {
                    return APIResponse<bool>.Error("Unable to update");
                }
                return APIResponse<bool>.Success(true, "Image Updated Successfully");
            }
        }


        //for remove the profile image of the user
        public async Task<APIResponse<bool>> DeleteProfileImage(Guid userid)
        {
            var query = "select * from Users where UserId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("id", userid);
            using(var connection = _dapperContext.CreateConnection())
            {
                connection. Open();
                var existing_data = await connection.QueryFirstOrDefaultAsync<Users>(query, parameters);
                connection.Close();

                if (existing_data == null)
                {
                    return APIResponse<bool>.Error("No user in this ID");
                }

                if(existing_data.profileImage!=null)
                {
                    var folderPath = Path.Combine(_env.WebRootPath, "Media", "UserImages");
                    var filePath = Path.Combine(folderPath, existing_data.profileImage);

                    if(System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    var updateQuery = "Update Users set ProfileImage=NULL where UserId=@id";

                    connection.Open();
                    var rowAffected = await connection.ExecuteAsync(updateQuery, parameters);
                    connection.Close();

                    if(rowAffected==0)
                    {
                        return APIResponse<bool>.Error("Failed to update user profile.");
                    }

                    return APIResponse<bool>.Success(true, "Profile image deleted successfully.");
                }
                else
                {
                    return APIResponse<bool>.Error("No profile image to delete.");
                }



            }
        }




    }
}
