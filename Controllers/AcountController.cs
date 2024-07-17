using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Note.Service;

namespace Note.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AcountController : Controller
    {
        private readonly IAuthService _authService;

        public AcountController(IAuthService authService){
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password){
            var token = await _authService.Register(username, password);

            if (token != null) {
                return Ok(new{ token });
            }

            return BadRequest("Registration failed!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password) {
            var token = await _authService.Login(username, password);

            if (token != null){
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}