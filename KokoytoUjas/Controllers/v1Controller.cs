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

        List<Document> _documents = new List<Document>()
        {
            new Document()
            {
                id= 1,
                title=" oao mmm",
                date_created=new DateTime(2024/9/20),
                date_updated=new DateTime(2024/9/25),
                category="хз",
                has_comments=true,

            }
        };

        List<Comment> _comments = new List<Comment>()
        {
            new Comment()
            {
                id=1,
                document_id=1,
                text="Hello",
                date_created=new DateTime(2024/9/25),
                date_updated=new DateTime(2024/9/30),
                author=new Author()
                    {
                        name="паша",
                        position="admin"
                    } 
            }
        };

        [HttpPost("signin")]
        public ActionResult<ResponseTokenAndRole> SignIn(User user)
        {
            if (user == null) return BadRequest(new Error()
            {
                timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                message = "Неправильно сформирован запроса",
                errorCode = 1400
            });
            User? found_user = _users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
            if (found_user is null)
                return Forbid("Неверные авторизационные данные");
           
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

        [HttpGet("Documents")]
        public ActionResult<List<Document>> Documents()
        {
            List<Document> docs = [.. _documents];
            return Ok(docs);
        }

        [HttpGet("Document/{documentId}/Comment")]
        public ActionResult Comments(int documentId)
        {
            List<Comment> comments = [.. _comments];
            List<Comment> comments_to_doc=comments.Where(c=>c.document_id==documentId).ToList();
            if (comments_to_doc == null) return NotFound(new Error()
            {
                timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                message = "Не найдены комментарии для документа",
                errorCode = 1404
            });
            return Ok(comments_to_doc);
        }

        [HttpGet("Document/{documentId}/Comment")]
        public ActionResult Comment(Comment comment)
        {
            if (comment == null) return BadRequest(new Error()
            {
                timestamp= DateTimeOffset.Now.ToUnixTimeSeconds(),
                message= "Неправильно сформирован запроса",
                errorCode = 1401
            });
            _comments.Add(comment);
            return Ok();
        }
    }
}
