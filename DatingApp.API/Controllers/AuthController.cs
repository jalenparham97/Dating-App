using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private readonly IAuthRepo _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepo repo, IConfiguration config) 
        {
            this._config = config;
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto incomingUser)
        {
            var username = incomingUser.Username.ToLower();
            var password = incomingUser.Password;
            if (await _repo.UserExists(username)) return BadRequest("Username already exists");
            var user = new User { Username = username };
            var newUser = await _repo.Register(user, password);
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto incomingUser)
        {
            var username = incomingUser.Username.ToLower();
            var password = incomingUser.Password;
            var user = await _repo.Login(username, password);
            if (user == null) return Unauthorized();

            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescipter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescipter);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }
}