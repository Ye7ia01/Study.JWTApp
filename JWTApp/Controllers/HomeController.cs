using JWTApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace JWTApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        JwtService jwtService;
        AppDBContext _db;
        public HomeController(IConfiguration config, AppDBContext db)
        {
            jwtService = new JwtService(config);
            _db = db;
        }
        [HttpPost("Register")]
        public IActionResult Register(UserDto dto)
        {
            User user = new User()
            {
                UserName = dto.UserName,
                RoleId = dto.RoleId,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)

            };
            _db.Add(user);
            _db.SaveChanges();
            return Ok(user);
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLogin dto)
        {
            var user = _db.Users.FirstOrDefault(m => m.UserName == dto.UserName);
            if (user is null)
            {
                return NotFound("Invalid User ");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!verified)
            {
                return NotFound("wrong password ");
            }
            var token = jwtService.GenerateJSONWebToken(user.UserName, user.RoleId);
            return Ok(token);
        }
        [HttpGet("GetData")]
        [Authorize]
        public IEnumerable<string> GetName()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("GetDataRole1")]
        [Authorize(Roles = "1")]
        public IEnumerable<string> GetRole()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
