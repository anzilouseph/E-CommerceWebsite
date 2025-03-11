using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.IService;
using ECommerceWebSite.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ECommerceWebSite.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _service;
        private readonly IUserManagementRepo _repo;
        private readonly IWebHostEnvironment _env;

        public UserManagementController(IUserManagementService service, IUserManagementRepo repo, IWebHostEnvironment env)
        {
            _service = service;
            _repo = repo;
            _env = env;
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
                return StatusCode(500, new { message = ex.Message });
            }
        }



        //for Get User By Own Profile
        [Authorize]
        [HttpGet("GetUserOwnProfile")]
        public async Task<IActionResult> GetOwnProfile()
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

        //User registration with Profile Image
        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistartion(UserRegistrationDto user)
        {
            try
            {

                var apiResponse = await _repo.UserRegistration(user);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //for retrive the image
        [HttpGet("User_Image")]
        public IActionResult GetProductImage(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return BadRequest("Filename is not provided.");
                var filePath = Path.Combine(_env.WebRootPath, "Media/UserImages", fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }
                var fileExtension = Path.GetExtension(fileName).ToLower();

                string contentType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "application/octet-stream",
                };

                var bytes = System.IO.File.ReadAllBytes(filePath);

                return File(bytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }


        //for update the Details of the User Excluding te Image
        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateDetails(UserDetailsForUpdation user)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _repo.UpdateUserDetails(user, userId);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //Here we only changing te Profile Image of the User
        [Authorize]
        [HttpPut("UpdateProfileImage")]
        public async Task<IActionResult> UpdateUserProfileImage([FromForm]UserProfileImageForUpdation user)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _repo.UpdateUserProfileImage(user, userId);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }


        //for remove the profile image of the user
        [Authorize]
        [HttpDelete("RemoveProfileImage")]
        public async Task<IActionResult> DeleteProfileImage()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Unable to generate JWT"));
                }
                var apiResponse = await _repo.DeleteProfileImage(userId);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

}
