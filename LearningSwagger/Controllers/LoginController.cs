using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LearningSwagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("generateJWT")]
        public async Task<string> Login()
        {
            // generate Base64 Key - start
            //https://generate.plus/en/base64
            //    using System.Security.Cryptography;
            //    var randomBytes = RandomNumberGenerator.GetBytes(64);
            //    var secret = Convert.ToBase64String(randomBytes); Console.WriteLine(secret);
            // generate Base64 Key - End

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
                new Claim(JwtRegisteredClaimNames.Email, "ajoshi0100@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var secret = "GAMhFKT+VqHmzgCwI5v0NfucWoJnUCXP2GrK8EQZiZsCfsOVXE/tgPqpnU3UyxxoHm8htIfmJpzu2y9LBjFc1Q==";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TheAvinashJoshiDotCom",
                audience: "TheAvinashJoshiDotComUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
