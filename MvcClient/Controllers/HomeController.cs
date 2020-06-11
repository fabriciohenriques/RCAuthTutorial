using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            var claims = User.Claims.ToList();

            ViewData["Result"] = await GetSecret(accessToken);

            return View();
        }

        public async Task<string> GetSecret(string accessToken)
        {
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(accessToken);

            var respose = await apiClient.GetAsync("https://localhost:44371/secret");
            if (respose.IsSuccessStatusCode)
                return await respose.Content.ReadAsStringAsync();
            else
                return respose.StatusCode.ToString();
        }
    }
}
