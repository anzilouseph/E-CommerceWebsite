using System.IdentityModel.Tokens.Jwt;
using ECommerceWebSite.Dto;
using ECommerceWebSite.IRepo;
using ECommerceWebSite.IService;
using ECommerceWebSite.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Crypto.Prng;

namespace ECommerceWebSite.Controllers
{
    [Route("api/AdminManagement")]
    [ApiController]
    public class AdminManagementController : ControllerBase
    {
        private readonly IAdminManagementService _service;
        private readonly IAdminManagementRepo _repo;
        private readonly IWebHostEnvironment _env;

        public AdminManagementController(IAdminManagementService service, IAdminManagementRepo repo, IWebHostEnvironment env)
        {
            _service = service;
            _repo = repo;
            _env = env;
        }


        //BELOW ARE THE FUNCTIONALITIES FOR USER MANAGEMENT

        //for Get User By his Id (only admin can access it so first it need to specify that is the Admin)
        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<UserToListDto>.Error("Only Admin can Access this"));
                }

                var apiResponse = await _service.GetUserById(id);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for Get ALL Users (This can only be accessed by Admin)
        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }
                var apiResponse = await _service.GetAllUsers();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for Add User by The Admin He Can also adjust the role
        [Authorize]
        [HttpPost("AddUserByAdmin")]
        public async Task<IActionResult> AddUser(UserForCreationByAdmin user)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userIdToCheck))
                {
                    return Unauthorized(APIResponse<IEnumerable<bool>>.Error("Unable to generate JWT"));
                }
                var checkingAdminOrNot = await _service.GetUserById(userIdToCheck); // now we get the details of the logined user cuz the user id is claimed and qwe call the getById using this claimed id
                if (checkingAdminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }

                var apiResponse = await _service.AddUserByAdmin(user);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for Get a Single User or list of users by the name (This can only be accessed by Admin)
        [Authorize]
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetUsersByName(string name)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }
                var apiResponse = await _service.GetUsersByName(name);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //for get the count of the user ,only admin can access it
        [Authorize]
        [HttpGet("UsersCount")]
        public async Task<IActionResult> GetUsersCount()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }
                var apiResponse = await _service.GetUsersCount();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        //for delete a user By Admin
        [Authorize]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }
                var apiResponse = await _service.DeleteUserById(id);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }




        //BELOW ARE THE FUNCTIONALITIES FOR PRODUCT MANAGEMENT


        //to add a product
        [Authorize]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductForCreationDto product)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }
                var apiResponse = await _repo.AddProduct(product);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }


        //for retrive the image
        [HttpGet("Product_image")]
        public IActionResult GetProductImage(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return BadRequest("Filename is not provided.");
                var filePath = Path.Combine(_env.WebRootPath, "Media/ProductImage", fileName);
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

        //To get teh category by the Name of that category
        [HttpGet("GetCategoryId")]
        public async Task<IActionResult> GetIdOfCategory(string categoryName)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userIdCheck))
                {
                    return BadRequest(APIResponse<string>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userIdCheck);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }
                var result = await _repo.GetIdOfCategory(categoryName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for get all products
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                //var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                //if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userIdCheck))
                //{
                //    return BadRequest(APIResponse<string>.Error("Unable to generate JWT"));
                //}
                //var adminCheck = await _service.GetUserById(userIdCheck);
                //if (adminCheck.data.roleOfUser != "Admin")
                //{
                //    return Unauthorized(APIResponse<ProductToListDto>.Error("U are Not an Admin"));
                //}
                var apiResponse = await _service.GetAllProducts();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        //for get all products from a category
        [HttpGet("GetProductByCategory")]
        public async Task<IActionResult> GetProductByCategory(string categoryName)
        {
            try
            {
                var apiResponse = await _service.GetProductByCategory(categoryName);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for get the product by the id
        
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var apiResponse = await _service.GetProductById(id);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }


        //for update the product by the id
        [Authorize]
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Guid product_id,ProductForUpdationDto product)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<IEnumerable<UserToListDto>>.Error("Only Admin can Access this"));
                }
                var apiResponse = await _repo.UpdateProduct(product_id,product);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //for getting the total count of products
        [Authorize]
        [HttpGet("GetProductCount")]
        public async Task<IActionResult> GetProductCount()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userCheckingId))
                {
                    return Unauthorized(APIResponse<int>.Error("Unable to generate JWT"));
                }
                var adminOrNot = await _service.GetUserById(userCheckingId);
                if (adminOrNot.data.roleOfUser != "Admin")
                {
                    return Unauthorized(APIResponse<int>.Error("Only Admin can Access this"));
                }
                var apiResponse = await _repo.GetProductsCount();
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //for search a Product
        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string pname)
        {
            try
            {
                var apiResposne = await _repo.SearchProduct(pname);
                return Ok(apiResposne);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
