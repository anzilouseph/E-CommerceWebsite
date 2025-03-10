using Dapper;
using ECommerceWebSite.Context;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IService;
using ECommerceWebSite.Utilitys;

namespace ECommerceWebSite.Repo
{
    public class WatchListManagementRepo :IWatchListManagementRepo
    {
        private readonly DapperContext _dapperContext;

        public WatchListManagementRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        //for add an item to the wishlist(onl once we can add)
        public async Task<APIResponse<bool>> AddToWishlist(AddWishlistDto dto, Guid userid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("productid", dto.idOfProduct);
            parameters.Add("userid", userid);
            var query = "select * from Wishlist where UserId=@userid and ProductId=@productid";

            using (var connection = _dapperContext.CreateConnection())
            {

                var rowAffected = await connection.QueryFirstOrDefaultAsync(query, parameters);

                if (rowAffected != null)
                {
                    return APIResponse<bool>.Error("Item Already In The WishList");
                }
                else
                {
                    var query2 = "insert into Wishlist (UserId,ProductId) values(@userid,@productid)";

                    var insertRowAffected = await connection.ExecuteAsync(query2, parameters);
                    connection.Close();
                    if (insertRowAffected == 0)
                    {
                        return APIResponse<bool>.Error("Unable To Add Iten To The WishList");
                    }
                    return APIResponse<bool>.Success(true, "Added To WishList");
                }
            }
        }


        //in watchlist we only have userid and product id BUT WE NEED TO RETRIVE THE ENTUIRE PRODUCT DETAILS SPO USE INNER JOIN
        public async Task<APIResponse<IEnumerable<dynamic>>> GetProductsInWatchList(Guid userId)
        {
            var query = "select p.ProductId as idOfProduct ,p.ProductName as nameOfProduct,p.Description as descriptionOfProduct,p.price as priceOfProduct,p.StockQuantity as availableQuantity,p.ImageUrl as urlOfImage,p.CategoryId as idOfCategory from Wishlist w inner join Products p on p.productId = w.ProductId where w.UserId=@userid";
            var parameters = new DynamicParameters();
            parameters.Add("userid", userId);
            
            using(var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<dynamic>(query, parameters);
                connection.Close() ;
                if(result.Count() ==0 )
                {
                    return APIResponse<IEnumerable<dynamic>>.Error("NO PRODUCTS IN THE WISHLIST");
                }
                return APIResponse<IEnumerable<dynamic>>.Success(result, "Success");
            }
        }

        //for delete the wishlist
        public async Task<APIResponse<bool>> DeleteFromWishlist(Guid productid, Guid userid)
        {
            var query = "delete from Wishlist where UserId=@userid and ProductId=@productid";
            var parameters = new DynamicParameters();   
            parameters.Add("userid",userid);
            parameters.Add("productid",productid);

            using(var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                if(rowAffected == 0)
                {
                    return APIResponse<bool>.Error("Invalid product id");
                }
                return APIResponse<bool>.Success(true, "Removed from wishlist");
            }
        }


        //for get the total number of products in the cart
        public async Task<APIResponse<int>> GetWishlistCount(Guid userid)
        {
            var query = "select count(*) from Wishlist where UserId = @uid";
            var parameters = new DynamicParameters();
            parameters.Add("uid", userid);
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var count = await connection.QueryFirstOrDefaultAsync<int>(query, parameters);
                connection.Close();
                return APIResponse<int>.Success(count, "Success");
            }
        }


    }
}
