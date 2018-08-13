using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitesChecker.DataAccess;

namespace SitesChecker.App.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class AdminController : Controller
	{
		public AdminController()
		{
			
		}
		public IActionResult Index()
		{
			//todo get sites
			throw new NotImplementedException();
		}
		
		[Authorize]
		[HttpDelete("removesite")]
		public IActionResult RemoveSite()
		{
			throw new NotImplementedException();
		}
		[Authorize]
		[HttpPut("updatesite")]
		public IActionResult UpdateSite()
		{
			throw new NotImplementedException();
		}
		[Authorize]
		[HttpPost("addsite")]
		public IActionResult AddSite()
		{
			throw new NotImplementedException();
		}
	}
}