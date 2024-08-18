using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthExample.Models;
using Microsoft.Extensions.Options;

namespace JwtAuthExample.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    public readonly string _jwtSecretKey;

    public AuthController(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSecretKey = jwtSettings.Value.SecretKey == null ? "" : jwtSettings.Value.SecretKey;
    }
     [HttpPost]
     public IActionResult Login([FromBody] UserModel userModel)
     {
        // You can validate the user credentials from the database here
        if (userModel.username == "admin" && userModel.password == "admin@123")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userModel.username),
                    new Claim(ClaimTypes.Role,"Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "http://localhost:5111",
                Audience = "http://localhost:5111"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {Token = tokenString});
        }

        return Unauthorized();
     }
  }
}