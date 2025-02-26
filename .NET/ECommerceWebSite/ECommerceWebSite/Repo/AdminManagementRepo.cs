using System.Linq.Expressions;
using System.Security.Permissions;
using Dapper;
using ECommerceWebSite.Context;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace ECommerceWebSite.Repo
{
    public class AdminManagementRepo : IAdminManagementRepo
    {
        private readonly DapperContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminManagementRepo(DapperContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _env = webHostEnvironment;
        }



        //BELOW ARE THE FUNCTIONALITIES FOR USER MANAGEMENT

        //for Get the profile of the user By the Id (only adin can access it)
        public async Task<Users> GetUserById(Guid id)
        {
            var query = "select * from Users where UserId=@id";
            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(query, parameters);
                connection.Close();
                return result;
            }
        }


        //for GetAllUsers only admin can get this
        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            var query = "select * from Users  WHERE Role='User' order by FullName asc";
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Users>(query);
                connection.Close();
                return result;
            }

        }


        //for add user
        public async Task<int> AddUserByAdmin(UserForCreationByAdmin user, string salt)
        {
            var query = "insert into Users (FullName,Phone,Email,Password,Salt,Role) values (@FullName,@Phone,@Email,@Password,@salt,@role)";
            var parameters = new DynamicParameters();
            parameters.Add("FullName", user.nameOfUser);
            parameters.Add("Phone", user.phoneOfUser);
            parameters.Add("Email", user.emailOfUser);
            parameters.Add("Password", user.passwordOfUser);
            parameters.Add("salt", salt);
            parameters.Add("role", user.roleOfUser);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                return rowAffected;
            }
        }

        //for get users by their name (mutlitple users can have same name)
        public async Task<IEnumerable<Users>> GetUsersByName(string name)
        {
           
            var query = $"select * from Users  WHERE Role='User' and FullName like @name";
            var parameters = new DynamicParameters();
            parameters.Add("name",$"%{name}%");
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Users>(query,parameters);
                connection.Close();
                return result;
            }
        }

        //to get the total number of Users
        public async Task<int> GetUsersCount()
        {
            var query = "select count(*) from Users where Role='User'";
            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<int>(query);
                connection.Close();
                return result;
            }
        }

        // for delete the user
        public async Task<int> DeleteUserById(Guid id)
        {
            var query = "Delete from Users where UserId=@id";
            var parameters= new DynamicParameters();
            parameters.Add("id", id);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query,parameters);
                connection.Close();
                return result;
            }
        }




        //BELOW ARE THE FUNCTIONALITIES FOR PRODUCT MANAGEMENT

        //to add a product to the Product table
        public async Task<APIResponse<bool>> AddProduct(ProductForCreationDto product)
        {
            
            Guid categoryId = Guid.Parse(product.idOfCategory);

            string filePath = string.Empty;
            string newFileName = string.Empty;
            if (product.product_image != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "Media", "ProductImage");


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                var fileExtension = Path.GetExtension(product.product_image.FileName).ToLower();

                var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return APIResponse<bool>.Error("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                }
                 newFileName = $"{product.nameOfProduct}{fileExtension}";
                filePath = Path.Combine(folderPath, newFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.product_image.CopyToAsync(stream)
;
                }
                Console.WriteLine(product.product_image.FileName);
                Console.WriteLine(filePath);
                Console.WriteLine(newFileName);
            }
            var query = "insert into products(ProductName,Description,Price,StockQuantity,ImageUrl,CategoryId)values(@ProductName,@Description,@Price,@StockQuantity,@ImageUrl,@CategoryId)";

            var parameters = new DynamicParameters();
            parameters.Add("ProductName", product.nameOfProduct);
            parameters.Add("Description", product.descriptionOfProduct);
            parameters.Add("Price", product.priceOfProduct);
            parameters.Add("StockQuantity", product.availableQuantity);
            parameters.Add("ImageUrl", newFileName, System.Data.DbType.String);
            parameters.Add("CategoryId", categoryId);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                if (result == 0)
                {
                    return APIResponse<bool>.Error("Unable to add product");
                }
                return APIResponse<bool>.Success(true, "Product Added SuccessFully");

            }
        }

        // we give a category name as input and get the ID of that as return (while adding product we need to specify teh category))
        public async Task<APIResponse<Guid>> GetIdOfCategory(string categoryName)
        {
            var query = "select CategoryId from Category where CategoryName=@categoryName";
            var parameters = new DynamicParameters();
            parameters.Add("categoryName", $"{categoryName}");

            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var result =await connection.QueryFirstOrDefaultAsync<Guid>(query, parameters);
                connection.Close() ;

                if(result == null)
                {
                    return APIResponse<Guid>.Error("Invalid category");
                }
                return APIResponse<Guid>.Success(result, "Success");
            }
        }

        //for get all products
        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            var query = "select * from Products";

            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Products>(query);
                connection.Close();
                return result;
            }
        }


        //for get all products from a category
        public async Task<IEnumerable<Products>> GetProductByCategory(string categoryName)
        {
            var query = "select p.* from products p  inner join category c on c.CategoryId = p.CategoryId where c.CategoryName like @categoryName";
            var parameters = new DynamicParameters();
            parameters.Add("categoryName", $"%{categoryName.Trim()}%");

            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Products>(query, parameters);
                connection.Close();
                return result;
            }
        }

        // for get the product  by the id
        public async Task<Products> GetProductById(Guid id)
        {
            var query = "select * from Products where ProductId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Products>(query,parameters);
                connection.Close();
                return result;
            }
        }
    }
}
