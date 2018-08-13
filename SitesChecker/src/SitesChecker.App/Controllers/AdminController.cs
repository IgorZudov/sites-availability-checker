using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SitesChecker.App.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			//todo get sites
			throw new NotImplementedException();
		}
		[Authorize]
		[Route("removesite")]
		public IActionResult RemoveSite()
		{
			throw new NotImplementedException();
		}
		[Authorize]
		[Route("updatesite")]
		public IActionResult UpdateSite()
		{
			throw new NotImplementedException();
		}
		[Authorize]
		[Route("addsite")]
		public IActionResult AddSite()
		{
			throw new NotImplementedException();
		}
	}
}