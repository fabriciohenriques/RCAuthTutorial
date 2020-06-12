using IdentityServer.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        readonly IIdentityServerInteractionService identityServerInteractionService;
        readonly SignInManager<IdentityUser> signInManager;
        readonly UserManager<IdentityUser> userManager;

        public AuthController(
            IIdentityServerInteractionService identityServerInteractionService,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this.identityServerInteractionService = identityServerInteractionService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            var result = await signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            if (result.Succeeded)
                return Redirect(vm.ReturnUrl);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await signInManager.SignOutAsync();
            var logoutRequest = await identityServerInteractionService.GetLogoutContextAsync(logoutId);
            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
                return RedirectToAction("Index", "Home");

            return Redirect(logoutRequest.PostLogoutRedirectUri);

        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser(vm.UserName);
                var userResult = await userManager.CreateAsync(user, vm.Password);

                if (userResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return Redirect(vm.ReturnUrl);
                }
            }

            return View(vm);
        }
    }
}
