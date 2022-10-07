using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using UserOnBoarding.Data;
using UserOnBoarding.Requests;

namespace UserOnBoarding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterRequest request)
        {
            if(await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("User already exists");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSlat);

            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSlat,
                VerificationToken = CreateRandomToken(),
            };

             _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User successfully created");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if(user == null)
            {
                return NotFound("User not found");
            }

            if(!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Invalid credentials");
            }

            if(user.VerifiedAt == null)
            {
                return BadRequest("Not verified");
            }

            return Ok($"Welcome back {user.Email}! :)");
        }

        [HttpPost("verify")]
        public async Task<ActionResult> Verify(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if(user == null)
            {
                return NotFound("Invalid token");
            }

            user.VerifiedAt = DateTime.Now;
            user.VerificationToken = null;
            await _context.SaveChangesAsync();

            return Ok("User verified!");
        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
            {
                return NotFound("No user associated with this Email");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok("You may now reset your password");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if(user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return NotFound("Invalid Token");
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Your password updated successfully");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
