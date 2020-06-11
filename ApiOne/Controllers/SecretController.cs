using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiOne.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public string Get()
        {
            var claims = User.Claims.ToList();
            return "Secret message from ApiOne";
        }
    }
}
