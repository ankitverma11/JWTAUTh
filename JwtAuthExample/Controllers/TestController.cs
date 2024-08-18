using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthExample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("This is a public endpoint.");
        }

        [HttpGet("private")]
        public IActionResult Private()
        {
            return Ok("This is a private endpoint. Only authenticated users can access this.");
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
             return Ok("This is an Admin endpoint. Only users with Admin role can access this.");
        }
    }
}