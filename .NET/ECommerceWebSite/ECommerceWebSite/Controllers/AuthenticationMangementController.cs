using ECommerceWebSite.Dto;
using ECommerceWebSite.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationMangementController : ControllerBase
    {
        private readonly IAuthenticationManagementService _service;

        public AuthenticationMangementController(IAuthenticationManagementService service)
        {
            _service = service;
        }

        //for Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto log)
        {
            var apiResponse = await _service.Login(log);
            return Ok(apiResponse);
        }
    }
}
