using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryGoDriver.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;


namespace LaundryGoDriver.Controllers
{
	[Authorize]
	public class DriversController : Controller
    {
        private readonly LaundryGoContext _context;

        public DriversController(LaundryGoContext context)
        {
            _context = context;
        }



		public async Task SignIn(HttpContext httpContext, Driver user, bool isPersistent = false)
		{
			user.dri_google = GetHash(user.dri_google);
			var user_SPData = await _context.Service_Provider
		.SingleOrDefaultAsync(m => m.sp_mobile == user.dri_phone && m.sp_password == user.dri_google);
			//ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user_AdminData), CookieAuthenticationDefaults.AuthenticationScheme);
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);
			claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user_SPData.sp_id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user_SPData.sp_name.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Custom Identity", "http://www.w3.org/2001/XMLSchema#string"));

			ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
			//AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

			await httpContext.SignInAsync(principal);

			httpContext.User = principal;
			string aaa = httpContext.User.Identity.Name;
			string bbb = httpContext.User.Identity.GetUserName();
			string ccc = httpContext.User.Identity.GetUserId();



		}

		public async void SignOut(HttpContext httpContext)
		{
			await httpContext.SignOutAsync();
		}

		private IEnumerable<Claim> GetUserClaims(Service_Provider user)
		{
			List<Claim> claims = new List<Claim>();

			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.sp_mobile.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, user.sp_name));


			//claims.AddRange(this.GetUserRoleClaims(user));
			return claims;
		}

		private IEnumerable<Claim> GetUserRoleClaims(Service_Provider user)
		{
			List<Claim> claims = new List<Claim>();

			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.sp_mobile.ToString()));
			//claims.Add(new Claim("Role", user.user_role.ToString()));
			return claims;
		}




		// GET: Drivers
		public async Task<IActionResult> Index()
        {
            return View(await _context.Driver.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver
                .SingleOrDefaultAsync(m => m.dri_id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("dri_id,dri_name,dri_fb,dri_google,dri_car_plate,dri_IC,dri_status,dri_phone,dri_c_add_1,dri_c_add_2,dri_c_add_3,dri_c_post")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver.SingleOrDefaultAsync(m => m.dri_id == id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("dri_id,dri_name,dri_fb,dri_google,dri_car_plate,dri_IC,dri_status,dri_phone,dri_c_add_1,dri_c_add_2,dri_c_add_3,dri_c_post")] Driver driver)
        {
            if (id != driver.dri_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.dri_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver
                .SingleOrDefaultAsync(m => m.dri_id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var driver = await _context.Driver.SingleOrDefaultAsync(m => m.dri_id == id);
            _context.Driver.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(long id)
        {
            return _context.Driver.Any(e => e.dri_id == id);
        }

		private static string GetHash(string text)
		{
			// SHA512 is disposable by inheritance.  
			using (var sha256 = SHA256.Create())
			{
				// Send a sample text to hash.  
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
				// Get the hashed string.  
				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}


	}
}
