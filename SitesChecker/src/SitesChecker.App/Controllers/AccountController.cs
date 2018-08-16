using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesChecker.App.Models;
using SitesChecker.DataAccess;
using SitesChecker.Domain.Models;

namespace SitesChecker.App.Controllers
{
	[Route("account")]
	public class AccountController : Controller
	{
		private readonly IDataContext dataContext;

		public AccountController(IDataContext dbContext)
		{
			dataContext = dbContext;
		}

		[HttpGet("Login")]
		public IActionResult Login()
		{
			return View();
		}
		
		[HttpPost("Login")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (!ModelState.IsValid) return View(model);
			var user = await dataContext.Query<User>()
				.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
			if (user != null)
			{
				var returnUrl = Request.Query["ReturnUrl"];
				await Authenticate(model.Login);
				if (Url.IsLocalUrl(returnUrl))
					return Redirect(returnUrl);
				return RedirectToAction("Index", "Statistic");
			}
			ModelState.AddModelError("", "Некорректные логин и(или) пароль");

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account");
		}

		private async Task Authenticate(string login)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, login)
			};
			var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}
	}
}