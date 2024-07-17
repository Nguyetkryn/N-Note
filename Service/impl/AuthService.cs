using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Note.Models;

namespace Note.Service
{
    public class AuthService : IAuthService
    {
        // UserManager và SignInManager để xác thực user và tạo token nếu đăng nhập thành công
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IConfiguration _config;

        public AuthService (UserManager<Users> userManager,
                            SignInManager<Users> signInManager,
                            IConfiguration config) {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<ActionResult<string>> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded){
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) {
                    return new BadRequestObjectResult("User not found");
                }
                return GenerateJSONWebToken(user);
            } else {
                return new BadRequestObjectResult("Invalid username or password");
            }
            
        }

        public async Task<ActionResult<string>> Register(string username, string password)
        {
            var user = new Users { UserName = username};
            var result = await _userManager.CreateAsync(user,password);

            if (result.Succeeded){
                return GenerateJSONWebToken(user);
            } else {
                return new BadRequestObjectResult("Unable to create account");
            }
        }

        private string GenerateJSONWebToken(Users userInfo){
            
            if (userInfo == null) {
                throw new ArgumentNullException(nameof(userInfo));
            }

            var signingKey = _config["JWT:SigningKey"];
            if (signingKey == null) {
                throw new ArgumentNullException("Failed");
            }
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}