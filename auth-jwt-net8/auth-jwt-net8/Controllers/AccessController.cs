using auth_jwt_net8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using auth_jwt_net8.Custom;
using auth_jwt_net8.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace auth_jwt_net8.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly DbauthContext _context;
        private readonly Utilities _utilities;
        public AccessController(DbauthContext context, Utilities utilities)
        {
            _context = context;
            _utilities = utilities;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserDTO user)
        {
            try
            {
                var userModel = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = _utilities.encryptSHA256(user.Password)
                };

                await _context.Users.AddAsync(userModel);
                await _context.SaveChangesAsync();

                if (userModel.UserId != 0)
                {
                    return Ok(new { message = "User registered successfully" });
                }
                else
                {
                    return BadRequest(new { message = "User registration failed" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            try
            {
                var findUser = await _context.Users
                        .Where(u => u.Email == user.Email && u.Password == _utilities.encryptSHA256(user.Password)
                        ).FirstOrDefaultAsync(); 

                if (findUser == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }
                else
                {
                    var token = _utilities.generateJWT(findUser);
                    return Ok(new { token = token, message = "Login successful" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
