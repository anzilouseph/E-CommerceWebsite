using ECommerceWebSite.Dto;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.IService
{
    public interface IAdminManagementService 
    {


        //BELOW ARE THE FUNCTIONALITIES FOR USER MANAGEMENT

        public Task<APIResponse<UserToListDto>> GetUserById(Guid id);
        public Task<APIResponse<IEnumerable<UserToListDto>>> GetAllUsers();
        public Task<APIResponse<bool>> AddUserByAdmin(UserForCreationByAdmin user); // for add user by Admin(he can give the role also)
        public Task<APIResponse<IEnumerable<UserToListDto>>> GetUsersByName(string name); // get a single user of multiple users by their name
        public Task<APIResponse<int>> GetUsersCount();
        public Task<APIResponse<bool>> DeleteUserById(Guid id);


        //BELOW ARE THE FUNCTIONALITIES FOR PRODUCT MANAGEMENT
        public Task<APIResponse<IEnumerable<ProductToListDto>>> GetAllProducts();  //for get all products
        public Task<APIResponse<IEnumerable<ProductToListDto>>> GetProductByCategory(string categoryName); //for search a category abd get all the products from that category
        public Task<APIResponse<ProductToListDto>> GetProductById(Guid id);  // for get the product  by the id

    }
}
