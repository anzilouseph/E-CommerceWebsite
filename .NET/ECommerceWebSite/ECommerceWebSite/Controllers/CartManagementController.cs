using System.IdentityModel.Tokens.Jwt;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartManagementController : ControllerBase
    {
        private readonly ICartManagementRepo _repo;

        public CartManagementController(ICartManagementRepo repo)
        {
            _repo = repo;
        }


        //for add a product to teh cart
        [Authorize]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(CartForAddDto dto)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized("Unable to Generate JWT");
                }
                var apiResponse = await _repo.AddToCart(dto, userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { messgae = ex.Message });
            }

        }

        //for GET ALL the products from the cart
        [Authorize]
        [HttpGet("GetCartProducts")]
        public async Task<IActionResult> GetProductsFromCart()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized("Unable to Generate JWT");
                }
                var apiResponse = await _repo.GetProductsFromCart(userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { messgae = ex.Message });
            }

        }

        //for remove a product from the cart
        [Authorize]
        [HttpDelete("DeleteFromCart")]
        public async Task<IActionResult> DeleteFromCart(Guid productid)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized("Unable to Generate JWT");
                }
                var apiResponse = await _repo.DeleteFromCart(productid, userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { messgae = ex.Message });
            }
        }


        //for get the total number of products in the cart
        [Authorize]
        [HttpGet("GetCartCount")]
        public async Task<IActionResult> GetCartCount()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userid))
                {
                    return Unauthorized("Unable to Generate JWT");
                }
                var apiResponse = await _repo.GetCartCount(userid);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { messgae = ex.Message });
            }

        }
    }
}
