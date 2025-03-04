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
    [Route("api/")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _service;
        private readonly IUserManagementRepo _repo;

        public UserManagementController(IUserManagementService service,IUserManagementRepo repo)
        {
            _service = service;
            _repo = repo;
        }


        //for User Registration
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserForCreationDto user)
        {
            try
            {
                
                var apiResponse = await _service.AddUser(user);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new { message = ex.Message });
            }
        }



        //for Get User By Own Profile
        [Authorize]
        [HttpGet("GetUserOwnProfile")]
        public async Task<IActionResult> GetUserById()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _service.GetOwnProfile(userId);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        //for get teh name of all categories
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var apiResponse = await _repo.GetCategories();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            
        }

    }
}
