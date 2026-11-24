using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		
	}
}
