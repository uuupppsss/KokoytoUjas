using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KokoytoUjas.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KokoytoUjas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v1Controller : ControllerBase
    {
        List<User> _users = new List<User>()
        {
            new User()
            {
                Id = 1,
                Login="паша",
                Password="пашаadmin",
                Role="admin"
            }
        };

        [HttpPost("signin")]
        public ActionResult<ResponseTokenAndRole> SignIn(User user)
        {
            User? found_user = _users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
            if (found_user is null)
                return Unauthorized();
           
            string role = found_user.Role;
            string username = found_user.Login;
            int id = found_user.Id;

            var claims = new List<Claim> {
                //Id (если нужно)
                new Claim(ClaimValueTypes.Integer32, id.ToString()),
                //роль
                new Claim(ClaimTypes.Role, role),
                //юз
                new Claim(ClaimTypes.Name, username)
            };

            // создаем
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new ResponseTokenAndRole
            {
                Token = token,
                Role = role
            });
        }
    }
}
