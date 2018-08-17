using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaundryGoSP.Models;

namespace LaundryGoSP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		private readonly LaundryGoContext _context;
		public HomeController(LaundryGoContext context)
		{
			_context = context;
		}

		//private static readonly LaundryGoContext _context = new LaundryGoContext(new Microsoft.EntityFrameworkCore.DbContextOptions<LaundryGoContext>());


		public ActionResult Login()
		{
			return View();
		}

		public ActionResult Logout()
		{

			Service_ProviderController _userManager = new Service_ProviderController(_context);

			_userManager.SignOut(this.HttpContext);
			return RedirectToAction("Login", "Home", null);

		}

		//private static readonly LaundryGoContext _context = new LaundryGoContext();

		[HttpPost]
		public async Task<IActionResult> Login(string email, string password)
		{
			Service_ProviderController _userManager = new Service_ProviderController(_context);
			if (!ModelState.IsValid)
				return View();
			try
			{
				//authenticate
				var userdata = new Service_Provider()
				{
					sp_mobile = email,
					sp_password = password
				};
				//var result = new User_AdminController(_context).SignIn(HttpContext, userdata);
				//RedirectToAction("../{User_AdminController}/SignIn", new { httpContext = this.HttpContext, user= userdata });
				await _userManager.SignIn(this.HttpContext, userdata);
				if (!this.HttpContext.User.Identity.IsAuthenticated)
				{

				}
				return RedirectToAction("Index", "Order_Header", null);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("summary", ex.Message);
				return View();
			}
		}

		public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
