using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using IdentityServer3.Core.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace superheroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();


        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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


            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);


            return Ok(token);
        }



        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            //lấy mã token ở cooki
            var refreshToken = Request.Cookies["refreshToken"];

            //kiểm tra như đã thấy
            if (!user.Refreshtoken.Equals(refreshToken))
            {
                return Unauthorized("Mã không đúng");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Hết hạn rồi làm mới đi");
            }


            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private Refreshtoken GenerateRefreshToken()
        {
            var refreshToken = new Refreshtoken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(3),
                Created = DateTime.Now
            };
            
            return refreshToken;
        }

        private void SetRefreshToken(Refreshtoken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            //ghi vào cooki máy người dùng
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            //ghi vào csdl mà chưa làm csdl
            user.Refreshtoken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        [HttpPost, Authorize]
        public ActionResult<object> Getme()
        {
            var userName = User?.Identity?.Name;
            var userName2 = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new { userName, userName2, role });
        }

        private string CreateToken(User user)
        {
            //Payload
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),

                //Role admin cho người tạo mới,
                new Claim(ClaimTypes.Role, "Admin")
            };

            //lớp khóa đối xứng lấy value ở appsettings token
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            byte[] keyForHmacSha256 = new byte[64];

            var randomGen =RandomNumberGenerator.Create();
            randomGen.GetBytes(keyForHmacSha256);

            //var key = new SymmetricSecurityKey(keyForHmacSha256);
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);        //header
        
            
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1), //thời gian tồn tại 1 ngày
                signingCredentials: creds);
        
            var jwt = new JwtSecurityTokenHandler().WriteToken(token); //tạo mã
        
            return jwt;
        }

        //tạo mã mật khẩu --thấy ko cần thiết lắm
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
