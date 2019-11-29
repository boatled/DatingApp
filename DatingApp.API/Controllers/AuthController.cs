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
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

            // Placeholder for Validation
            // if(!ModelState.IsValid)
            //     return BadRequest(ModelState);

            // Convert the username to lowecase for consistency.
            // Username cannot be duplicated.
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            // Check if the user exists.
            // If it doesn't return the message.
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            // Add the username to UserToCreate object of type User
            var UserToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            // Register the user.
            var createdUser = await _repo.Register(UserToCreate, userForRegisterDto.Password);

            // Return the user created status code.
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // Check if the login is successful
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            // If the user doesn't exist return Unauthorized.
            if (userFromRepo == null)
                return Unauthorized();

            // Create a variable to store the claims
            var claims = new[]
            {
                // Using the NameIdentifier type of claim
                // as this co-relates with the ID
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),

                // Using the Name type of claim
                // as it co-relates with the Name
                new Claim(ClaimTypes.Name, userFromRepo.Username)

            };

            // Store a token in the configuration file 
            // This token is stored in the appsettings.json file of the application
            var key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_config.GetSection("AppSettings:Token").Value));        
            
            // Sign the key with HMACSHA512 Signature
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create a token descriptor by adding 
            // Subject, Expires and SignatureCredentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Create a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Pass the tokenDescriptor as the parameter for tokenHandler
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the token
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}