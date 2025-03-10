using System.IdentityModel.Tokens.Jwt;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.IService;
using ECommerceWebSite.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    [Route("api/Watchlist")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchListManagementRepo _repo;
       
        public WatchListController(IWatchListManagementRepo repo)
        {
            _repo = repo;
        }

        //to add a product to the wishlist
        [Authorize]
        [HttpPost("AddToWishlist")]
        public async Task<IActionResult> AddToWishlist(AddWishlistDto dto)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid usersId))
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _repo.AddToWishlist(dto, usersId);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //in watchlist we only have userid and product id BUT WE NEED TO RETRIVE THE ENTUIRE PRODUCT DETAILS SPO USE INNER JOIN
        [Authorize]
        [HttpGet("WatchListProducts")]
        public async Task<IActionResult> GetProductsInWatchList()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid usersId))
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _repo.GetProductsInWatchList(usersId);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for delete from the wishlist
        [Authorize]
        [HttpDelete("DeleteFromWishlist")]
        public async Task<IActionResult> DeleteFromWishlist(Guid productid)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized("Unable to generate JWT");
                }
                var apiResponse = await _repo.DeleteFromWishlist(productid, userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //for get the total number of products in the cart
        [Authorize]
        [HttpGet("GetWishlistCount")]
        public async Task<IActionResult> GetWishlistCount()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized("Unable to Generate JWT");
                }
                var apiResponse = await _repo.GetWishlistCount(userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { messgae = ex.Message });
            }

        }

    }
}
