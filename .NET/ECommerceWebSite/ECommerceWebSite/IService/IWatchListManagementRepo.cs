using ECommerceWebSite.Dto;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.IService
{
    public interface IWatchListManagementRepo
    {
        public Task<APIResponse<bool>> AddToWishlist(AddWishlistDto dto, Guid userid); //for add an item to the wishlist(only once we can add)
        public Task<APIResponse<IEnumerable<dynamic>>> GetProductsInWatchList(Guid userId); //in watchlist we only have userid and product id BUT WE NEED TO RETRIVE THE ENTUIRE PRODUCT DETAILS SPO USE INNER JOIN
        public Task<APIResponse<bool>> DeleteFromWishlist(Guid productid,Guid userid); //for delete the wishlist
        public Task<APIResponse<int>> GetWishlistCount(Guid userid); //for get the total number of products in the cart
    }
}
