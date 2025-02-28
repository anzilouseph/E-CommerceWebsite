using ECommerceWebSite.Dto;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.IRepo
{
    public interface IAdminManagementRepo
    {


        //BELOW ARE THE FUNCTIONALITIES FOR USER MANAGEMENT
        public Task<Users> GetUserById(Guid id);
        public Task<IEnumerable<Users>> GetAllUsers();
        public Task<int> AddUserByAdmin(UserForCreationByAdmin user, string salt);  //AddUser By Admin
        public Task<IEnumerable<Users>> GetUsersByName(string name);
        public Task<int> GetUsersCount();
        public Task<int> DeleteUserById(Guid id);



        //BELOW ARE THE FUNCTIONALITIES FOR PRODUCT MANAGEMENT
        public Task<APIResponse<bool>> AddProduct(ProductForCreationDto product); //to add a product
        public Task<APIResponse<Guid>> GetIdOfCategory(string categoryName); // we give a category name as input and get the ID of that as return (while adding product we need to specify teh category))
        public Task<IEnumerable<Products>> GetAllProducts();  //for get all products
        public Task<IEnumerable<Products>> GetProductByCategory(string categoryName); //for search a category abd get all the products from that category
        public Task<Products> GetProductById(Guid id);  // for get the product  by the id
        public Task<APIResponse<bool>> UpdateProduct(Guid product_id ,ProductForUpdationDto product); //for update the product
        public Task<APIResponse<int>> GetProductsCount(); //for get the count of products
    }
}
