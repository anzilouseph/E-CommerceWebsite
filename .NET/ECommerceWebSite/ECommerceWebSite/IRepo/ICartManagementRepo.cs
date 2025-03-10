using ECommerceWebSite.Dto;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.IRepo
{
    public interface ICartManagementRepo
    {
        public Task<APIResponse<bool>> AddToCart(CartForAddDto dto,Guid userid);  //for add a product to the cart
        public Task<APIResponse<IEnumerable<ProductForCartDto>>> GetProductsFromCart(Guid userid);  //for get all the products in the cart
        public Task<APIResponse<bool>> DeleteFromCart(Guid productid, Guid userid); //for delete a product from te cart
        public Task<APIResponse<int>> GetCartCount(Guid userid); //for get the total number of products in the cart
    }
}
