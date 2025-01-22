using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KokoytoUjas.Model;

namespace KokoytoUjas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v1Controller : ControllerBase
    {
        [HttpPost("signin")]
        public void SignIn(User user)
        {

        }
    }

}
