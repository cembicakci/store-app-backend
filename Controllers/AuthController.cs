using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.Dtos;
using StoreApp.Models;
using StoreApp.Services;

namespace StoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly TokenService _tokenService;

        public AuthController(StoreContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email already exists");
            }

            if (await _context.Users.AnyAsync(x => x.Username == registerDto.Username))
            {
                return BadRequest("Username already exists");
            }

            using var hmac = new HMACSHA512();
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)));
            var passwordSalt = Convert.ToBase64String(hmac.Key);

            var user = new User
            {
                Email = registerDto.Email.ToLower(),
                Username = registerDto.Username.ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Username = user.Username,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null) return Unauthorized("Invalid email");

            var saltBytes = Convert.FromBase64String(user.PasswordSalt);
            using var hmac = new HMACSHA512(saltBytes);
            var computedHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password))
            );

            if (computedHash != user.PasswordHash)
                return Unauthorized("Invalid password");

            return new AuthResponseDto
            {
                Username = user.Username,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
