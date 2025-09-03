using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Api.Dtos;
using TaskManagement.Api.Models.Entities;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger; 

        public AuthController(AppDbContext context, IConfiguration config, ILogger<AuthController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Register attempt with null payload");
                return BadRequest("Invalid data");
            }

            if (string.IsNullOrWhiteSpace(dto.UserName))
                return BadRequest("Username is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required");

            if (!dto.Email.Contains("@"))
                return BadRequest("Invalid email format");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters");

            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                _logger.LogWarning("Register attempt failed: Email {Email} already exists", dto.Email);
                return Conflict("Email already exists");
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            _logger.LogInformation("User {Email} registered successfully with ID {UserId}", user.Email, user.Id);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.CreatedAt
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                _logger.LogWarning("Login attempt with missing credentials");
                return BadRequest("Email and Password are required");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: No user found with Email {Email}", login.Email);
                return Unauthorized("Invalid credentials");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);
            if (!verified)
            {
                _logger.LogWarning("Login failed: Invalid password for Email {Email}", login.Email);
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(user);

            _logger.LogInformation("User {Email} logged in successfully", login.Email);

            return Ok(new { Token = token });
        }

        [HttpGet("profile")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Profile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                _logger.LogWarning("Profile requested but user with Email {Email} not found", email);
                return NotFound();
            }

            _logger.LogInformation("Profile accessed for user {Email}", email);

            return Ok(new { user.Id, user.UserName, user.Email, user.CreatedAt });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtConfig");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
