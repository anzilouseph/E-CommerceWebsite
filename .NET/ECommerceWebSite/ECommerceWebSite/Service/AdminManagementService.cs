using System.Collections.Generic;
using System.ComponentModel;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.IService;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.Service
{
    public class AdminManagementService : IAdminManagementService
    {
        private readonly IAdminManagementRepo _repo;

        public AdminManagementService(IAdminManagementRepo repo)
        {
            _repo = repo;
        }


        //BELOW ARE THE FUNCTIONALITIES FOR USER MANAGEMENT

        //for GetUser By Id (Only Admin can access this)
        public async Task<APIResponse<UserToListDto>> GetUserById(Guid id)
        {
            var result = await _repo.GetUserById(id);
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


        //for get All users Only admin can take this
        public async Task<APIResponse<IEnumerable<UserToListDto>>> GetAllUsers()
        {
            var result = await _repo.GetAllUsers();

            if (result.Count() == 0)
            {
                return APIResponse<IEnumerable<UserToListDto>>.Error("No Users To show");
            }

            var masked = result.Select(user => new UserToListDto()
            {
                idOfUser = user.UserId,
                nameOfUser = user.FullName,
                phoneOfUser = user.Phone,
                emailOfUser = user.Email,
                roleOfUser = user.Role,
            });
            return APIResponse<IEnumerable<UserToListDto>>.Success(masked, "Success");
        }


        //for Add User
        public async Task<APIResponse<bool>> AddUserByAdmin(UserForCreationByAdmin user)
        {
            user.passwordOfUser = forPasswordHasing.HashPassword(user.passwordOfUser, out string salt);

            var rowAffected = await _repo.AddUserByAdmin(user, salt);

            if (rowAffected == 0)
            {
                return APIResponse<bool>.Error("Unable to add user");
            }

            if (user.roleOfUser == "Admin")
            {
                return APIResponse<bool>.Success(true, "User Added Successfully and he is a Admin");
            }
            return APIResponse<bool>.Success(true, "User Added Successfully");
        }

        // get a single user of multiple users by their name
        public async Task<APIResponse<IEnumerable<UserToListDto>>> GetUsersByName(string name)
        {
            var result = await _repo.GetUsersByName(name);
            if (result.Count() == 0)
            {
                return APIResponse<IEnumerable<UserToListDto>>.Error("No Users To show");
            }
            var masked = result.Select(user => new UserToListDto
            {
                idOfUser = user.UserId,
                nameOfUser = user.FullName,
                phoneOfUser = user.Phone,
                emailOfUser = user.Email,
                roleOfUser = user.Role,
            });
            return APIResponse<IEnumerable<UserToListDto>>.Success(masked, "Sucess");

        }

        //to get the total users
        public async Task<APIResponse<int>> GetUsersCount()
        {
            var result = await _repo.GetUsersCount();
            if (result.GetType() != typeof(int))
            {
                return APIResponse<int>.Error("Unable to Laod this");
            }
            else
            {
                return APIResponse<int>.Success(result, "Success");
            }
        }

        //for delete a user By Admin
        public async Task<APIResponse<bool>> DeleteUserById(Guid id)
        {
            var result = await _repo.DeleteUserById(id);
            if (result == 0)
            {
                return APIResponse<bool>.Error("Unable to Delete");
            }
            return APIResponse<bool>.Success(true, "Success");

        }


        //BELOW ARE THE FUNCTIONALITIES FOR PRODUCT MANAGEMENT
        public async Task<APIResponse<IEnumerable<ProductToListDto>>> GetAllProducts()  //for get all products
        {
            var result = await _repo.GetAllProducts();
            if(result.Count()==0)
            {
                return APIResponse<IEnumerable<ProductToListDto>>.Error("No Products Available");
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

            return APIResponse<IEnumerable<ProductToListDto>>.Success(masked, "Success");
        }

        //for get all products from a category
        public async Task<APIResponse<IEnumerable<ProductToListDto>>> GetProductByCategory(string categoryName)
        {
            var result = await _repo.GetProductByCategory(categoryName);
            if(result.Count()==0)
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
                urlOfImage =  product.ImageUrl,
            });

            return APIResponse<IEnumerable<ProductToListDto>>.Success(masked, "Success");
        }

        // for get the product  by the id
        public async Task<APIResponse<ProductToListDto>> GetProductById(Guid id)
        {
            var result = await  _repo.GetProductById(id);
            if(result == null)
            {
                return APIResponse<ProductToListDto>.Error("No products in this id");
            }
            var masked = new ProductToListDto
            {
                idOfProduct = result.ProductId,
                nameOfProduct = result.ProductName,
                descriptionOfProduct = result.Description,
                priceOfProduct = result.Price,
                availableQuantity = result.StockQuantity,
                idOfCategory = result.CategoryId,
                urlOfImage = result.ImageUrl,
            };
            return APIResponse<ProductToListDto>.Success(masked, "Success");
        }

    }
}
