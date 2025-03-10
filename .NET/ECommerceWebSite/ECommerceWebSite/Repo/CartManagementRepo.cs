using System.Collections.Generic;
using System.Dynamic;
using Dapper;
using ECommerceWebSite.Context;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.Models;
using ECommerceWebSite.Utilitys;
using Org.BouncyCastle.Crypto.Prng;

namespace ECommerceWebSite.Repo
{
    public class CartManagementRepo : ICartManagementRepo
    {
        private readonly DapperContext _context;

        public CartManagementRepo(DapperContext context)
        {
            _context = context;
        }

        //for Add a product into the cart
        public async Task<APIResponse<bool>> AddToCart(CartForAddDto dto, Guid userid)
        {
            
            var gettingQuery = "select * from cart where ProductId=@productid and UserId=@userid";
            var parameters = new DynamicParameters();
            parameters.Add("ProductId", dto.idOfProduct);
            parameters.Add("userid", userid);

            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var itemChecking = await connection.QueryFirstOrDefaultAsync<Cart>(gettingQuery, parameters);
                connection.Close();
                if(itemChecking != null)
                {
                    return APIResponse<bool>.Error("Item Already in the Cart");
                }



                var query = "insert into Cart (ProductId,UserId) values (@productid,@userid)";
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);  
                connection.Close();
                if(rowAffected==0)
                {
                    return APIResponse<bool>.Error("Unable to add to Cart");
                }
                return APIResponse<bool>.Success(true,"Added To Cart");
            }
        }

        //for get all the products in the cart
        public async Task<APIResponse<IEnumerable<ProductForCartDto>>> GetProductsFromCart(Guid userid)
        {
            var query = "select p.ProductId as idOfProduct,p.ProductName as nameOfProduct,p.Description as descriptionOfProduct,p.Price as priceOfProduct, p.StockQuantity as availableQuantity,p.ImageUrl  as urlOfImage,p.CategoryId  as idOfCategory,c.CategoryName  as nameOfCategory from Products p join Category c on p.CategoryId=c.CategoryId join cart on p.ProductId=cart.ProductId where cart.UserId=@userid";

            var parameters = new DynamicParameters();
            parameters.Add("userid", userid);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<ProductForCartDto>(query, parameters); 
                connection.Close();

                if(result.Count()==null )
                {
                    return APIResponse<IEnumerable<ProductForCartDto>>.Error("No items in the CART");
                }

                return APIResponse<IEnumerable<ProductForCartDto>>.Success(result, "Success");
            }
        
        }


        //for delete a product from te cart
        public async Task<APIResponse<bool>> DeleteFromCart(Guid productid, Guid userid)
        {
            var query = "delete from cart where ProductId=@pid and UserId=@uid";

            var parameters = new DynamicParameters();
            parameters.Add("pid", productid);
            parameters.Add("uid", userid);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();

                if(rowAffected==0)
                {
                    return  APIResponse<bool>.Error("No Product in this Id");
                }
                return APIResponse<bool>.Success(true,"Removed from Cart");
            }
        }


        //for get the total number of products in the cart
        public async Task<APIResponse<int>> GetCartCount(Guid userid)
        {
            var query = "select count(*) from Cart where UserId = @uid";
            var parameters = new DynamicParameters();
            parameters.Add("uid",userid);
            using(var connection = _context.CreateConnection())
            {
                connection.Open();
                var count = await connection.QueryFirstOrDefaultAsync<int>(query,parameters);
                connection.Close();
                return APIResponse<int>.Success(count, "Success");
            }
        }

    }
}
