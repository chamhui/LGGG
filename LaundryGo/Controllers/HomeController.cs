﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaundryGo.Models;
using Microsoft.AspNetCore.Authorization;

namespace LaundryGo.Controllers
{
		
    public class HomeController : Controller
    {
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

			User_AdminController _userManager = new User_AdminController(_context);

			_userManager.SignOut(this.HttpContext);
			return RedirectToAction("Login", "Home", null);
			
		}

		//private static readonly LaundryGoContext _context = new LaundryGoContext();

		[HttpPost]
		public async Task<IActionResult> Login(string email,string password)
		{
			User_AdminController _userManager = new User_AdminController(_context);
			if (!ModelState.IsValid)
				return View();
			try
			{
				//authenticate
				var userdata = new User_Admin()
				{
					email_address = email,
					user_password = password
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

		[Authorize]
		public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
		[Authorize]
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
