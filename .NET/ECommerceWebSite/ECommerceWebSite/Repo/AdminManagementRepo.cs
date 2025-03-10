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

                                        ////for update the product (THIS CODE WORKS PERFECTLY BUT A SIMPLE IMPROVISATION NEEDED)
                                        //public async Task<APIResponse<bool>> UpdateProduct(ProductForUpdationDto product, Guid product_id)
                                        //{
                                        //    var queryBuilder = new List<string>();
                                        //    var parameters = new DynamicParameters();


                                        //    string filePath = string.Empty;  //this is for image
                                        //    string newFileName = string.Empty; //this is also for image


                                        //    var mapped = new Products
                                        //    {
                                        //        ProductName = product.nameOfProduct,
                                        //        Description = product.descriptionOfProduct,
                                        //        Price = product.priceOfProduct,
                                        //        StockQuantity = product.availableQuantity,
                                        //        CategoryId = product.idOfCategory,

                                        //    };

                                        //    foreach (var prop in typeof(Products).GetProperties())  // from this for each we get all the values excluding the image(that we do seperatly)
                                        //    {
                                        //        if (prop.Name != "ProductId" && prop.Name != "ImageUrl")
                                        //        {
                                        //            var value = prop.GetValue(mapped);
                                        //            if (value != null)
                                        //            {
                                        //                queryBuilder.Add($"{prop.Name}=@{prop.Name}");
                                        //                parameters.Add($"{prop.Name}", value);
                                        //            }
                                        //        }
                                        //    }


                                        //    //image code starts here
                                        //    if (product.product_image != null)
                                        //    {
                                        //        var folderPath = Path.Combine(_env.WebRootPath, "Media", "ProductImage");


                                        //        if (!Directory.Exists(folderPath))
                                        //        {
                                        //            Directory.CreateDirectory(folderPath);
                                        //        }


                                        //        var fileExtension = Path.GetExtension(product.product_image.FileName).ToLower();

                                        //        var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };

                                        //        if (!allowedExtensions.Contains(fileExtension))
                                        //        {
                                        //            return APIResponse<bool>.Error("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                                        //        }
                                        //        newFileName = $"{product.nameOfProduct}{fileExtension}";
                                        //        filePath = Path.Combine(folderPath, newFileName);
                                        //        if (System.IO.File.Exists(filePath))
                                        //        {
                                        //            System.IO.File.Delete(filePath);
                                        //        }
                                        //        using (var stream = new FileStream(filePath, FileMode.Create))
                                        //        {
                                        //            await product.product_image.CopyToAsync(stream);

                                        //        }
                                        //        Console.WriteLine(product.product_image.FileName);
                                        //        Console.WriteLine(filePath);
                                        //        Console.WriteLine(newFileName);


                                        //        queryBuilder.Add("ImageUrl=@ImageUrl");  // from these 2 lines  we add the ImageUrl field(this fireld si in the product table),
                                        //        parameters.Add("ImageUrl", newFileName);  //and we give its value as the newly added immages name with teh file extension.
                                        //    }


                                        //    //image code ends here

                                        //    parameters.Add("id", product_id); // to update the query with this given product_id;


                                        //    if (queryBuilder.Count == 0)  //this is to make sure their is atleast one parameter is their to update
                                        //    {
                                        //        return APIResponse<bool>.Error("No fields to Update");
                                        //    }


                                        //    var query = $"update Products set {string.Join(",", queryBuilder)} where ProductId=@id";

                                        //    using (var connection = _context.CreateConnection())
                                        //    {
                                        //        connection.Open();
                                        //        var result = await connection.ExecuteAsync(query, parameters);
                                        //        connection.Close();
                                        //        if (result == 0)
                                        //        {
                                        //            return APIResponse<bool>.Error("Unable to update");
                                        //        }
                                        //        return APIResponse<bool>.Success(true, "Success");
                                        //    }

                                        //}

        //for update the product (THIS CODE WORKS PERFECTLY BUT A SIMPLE IMPROVISATION NEEDED)
        public async Task<APIResponse<bool>> UpdateProduct(Guid product_id, ProductForUpdationDto product)
        {
            var queryBuilder = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("id", product_id);

            string filePath = string.Empty;  //this is for image
            string newFileName = string.Empty; //this is also for image

            string realName = string.Empty; // this variable is used when the image is not Updated but the name that we linked with the image for make it unique ,changed. So in that case we need to change the imagename
 
            var query_TogetAlreadyData = "select * from Products where ProductId=@id";
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var existing_Data = await connection.QueryFirstOrDefaultAsync<Products>(query_TogetAlreadyData, parameters);  //here we get that already existing row
                connection.Close();

                if(existing_Data==null)
                {
                    return APIResponse<bool>.Error("invalid Id");
                }

                var binded = new Products   //here we create a ned original model copy for foreach(same as always)
                {
                    ProductName = product.nameOfProduct,
                    Description = product.descriptionOfProduct,
                    Price = product.priceOfProduct,
                    StockQuantity = product.availableQuantity,
                    CategoryId = product.idOfCategory,
                };

                foreach(var prop in typeof(Products).GetProperties())  //iterate through each
                {
                    if (prop.Name != "ProductId" && prop.Name != "ImageUrl")
                    {
                        var value = prop.GetValue(binded);

                        if(value != null)
                        {
                            queryBuilder.Add($"{prop.Name}=@{prop.Name}");
                            parameters.Add($"{prop.Name}",value);
                        }
                    }
                }

                //Image Code Starts Here

                if (product.product_image != null)  // if we gave a new image for updation
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
                    if(product.nameOfProduct != null)
                    {
                        newFileName = $"{product.nameOfProduct}{fileExtension}";
                    }
                    else
                    {
                        newFileName = $"{existing_Data.ProductName}{fileExtension}";
                    }
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

                    queryBuilder.Add("ImageUrl=@ImageUrl");
                    parameters.Add("ImageUrl", newFileName);
                }
                else  //if we dont give a new inage to updation, but if we changed the productname(that we set as unique and add .extension and save it in the imageurl eg:stacks.jpg), then the naem of the image that is imageurl also needs to be modified based on the name or any oyther field u use iyt to make that unique
                {                    
                    if(product.nameOfProduct!=null)
                    {
             //IMP           //to get teh old path name (that is with the old image name)
                        var oldFilePath = Path.Combine(_env.WebRootPath, "Media", "ProductImage", existing_Data.ImageUrl);

                        //top create the new name for teh ImgaeUrlField
                        string[] newname = existing_Data.ImageUrl.Split('.');
                        string firstname = product.nameOfProduct;
                        string extension = newname[^1];
                        realName = string.Join(".", firstname,extension);

             //IMP      //this is the new file path name(HERE WE CHANGED THE IMAEG NAME TO NEW ONE)
                        var newFilePath = Path.Combine(_env.WebRootPath, "Media", "ProductImage", realName);

                        // Check if the existing image file is present
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            // Rename the existing file to match the updated product name
                            System.IO.File.Move(oldFilePath, newFilePath);
                        }


                        queryBuilder.Add("ImageUrl=@ImageUrl");
                        parameters.Add("ImageUrl", realName);
                    }                  
                }

                //for update image code ENDS here

                //following below code is for the update query and db
                var query = $"update Products set {string.Join(",", queryBuilder)} where ProductId=@id";
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                if (rowAffected==0)
                {
                    return APIResponse<bool>.Error("Unable to update");
                }
                return APIResponse<bool>.Success(true, "Success");
            }
        }




        //to get the total number of Products
        public async Task<APIResponse<int>> GetProductsCount()
        {
            var query = "select count(*) from Products where StockQuantity>0";
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<int>(query);
                connection.Close();
                if(result==0)
                {
                    return APIResponse<int>.Error("NO products To show");
                }
                return APIResponse<int>.Success(result, "Success");
            }
        }


        //for search a product
        public async Task<APIResponse<IEnumerable<ProductToListDto>>> SearchProduct(string pname)
        {
            var query = "select * from products where ProductName like @pname";
            var parameters = new DynamicParameters();
            parameters.Add("pname", $"%{pname}%");
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Products>(query, parameters);
                connection.Close();

                if (result.Count() == 0)
                {
                    return APIResponse<IEnumerable<ProductToListDto>>.Error("No Products in this categpry");
                }
                var masked = result.Select(product => new ProductToListDto
                {
                    idOfProduct = product.ProductId,
                    nameOfProduct = product.ProductName,
                    descriptionOfProduct = product.Description,
                    priceOfProduct = product.Price,
                    availableQuantity = product.StockQuantity,
                    idOfCategory = product.CategoryId,
                    urlOfImage = product.ImageUrl,
                });
                return APIResponse<IEnumerable<ProductToListDto>>.Success(masked,"Success");
            }
        }


    }
}
