using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using IdentityServer3.Core.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


namespace superheroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        //private readonly IConfiguration _configuration;
        //private readonly IUserService _userService;
        //public AuthController(IConfiguration configuration, IUserService userService)
        //{
        //    _configuration = configuration;
        //    _userService = userService;
        //}


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Userdto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);
        }



        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(Userdto request)
        {
            if(user.Username != request.Username)
            {
                return BadRequest("ko tim thay");
            }

            if(!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }


        private string CreateToken(User user)
        {
            //Payload
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            //    _configuration.GetSection("AppSettings:Token").Value));
            
            byte[] keyForHmacSha256 = new byte[64];

            var randomGen =RandomNumberGenerator.Create();
            randomGen.GetBytes(keyForHmacSha256);

            var key = new SymmetricSecurityKey(keyForHmacSha256);
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);        //header
        
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
        
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
            return jwt;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
