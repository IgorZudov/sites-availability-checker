using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SitesChecker.Core;
using SitesChecker.DataAccess.Models;

namespace SitesChecker.App.Controllers
{
	//todo need add IdentityServer4
	public class AccountController : Controller
	{
		//todo change to domain model or DTO
		private readonly List<User> people = new List<User>
		{
			new User {Login="admin@gmail.com", Password="12345", Role = "admin" },
			new User { Login="qwerty", Password="55555", Role = "user" }
		};

		[HttpPost("/token")]
		public async Task Token()
		{
			var username = Request.Form["username"];
			var password = Request.Form["password"];

			var identity = GetIdentity(username, password);
			if (identity == null)
			{
				Response.StatusCode = 400;
				await Response.WriteAsync("Invalid username or password.");
				return;
			}

			var now = DateTime.UtcNow;
			
			var jwt = new JwtSecurityToken(
					issuer: AuthOptions.Issuer,
					audience: AuthOptions.Audience,
					notBefore: now,
					claims: identity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
					signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = new
			{
				access_token = encodedJwt,
				username = identity.Name
			};
			
			Response.ContentType = "application/json";
			await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
		}

		private ClaimsIdentity GetIdentity(string username, string password)
		{
			var user = people.FirstOrDefault(x => x.Login == username && x.Password == password);
			if (user != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
					new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
				};
				var claimsIdentity =
				new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
					ClaimsIdentity.DefaultRoleClaimType);
				return claimsIdentity;
			}
			return null;
		}
	}
}