using BarcodeAPI.Models;
using BarcodeAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarcodeAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("add")]
        public ActionResult AddUser([FromBody] AddUserBodyRequest body)
        {
            _accountService.AddNewUser(body);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto body)
        {
            var token = _accountService.GenerateJWT(body);
            return Ok(token);
        }
    }
}