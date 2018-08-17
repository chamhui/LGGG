using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryGoSP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace LaundryGoSP.Controllers
{
	[Authorize]
	public class Service_ProviderController : Controller
    {
        private readonly LaundryGoContext _context;

        public Service_ProviderController(LaundryGoContext context)
        {
            _context = context;
        }


		public async Task SignIn(HttpContext httpContext, Service_Provider user, bool isPersistent = false)
		{
			user.sp_password = GetHash(user.sp_password);
			var user_SPData = await _context.Service_Provider
		.SingleOrDefaultAsync(m => m.sp_mobile == user.sp_mobile && m.sp_password == user.sp_password);
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




		// GET: Service_Provider
		public async Task<IActionResult> Index()
        {
            return View(await _context.Service_Provider.ToListAsync());
        }

        // GET: Service_Provider/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service_Provider = await _context.Service_Provider
                .SingleOrDefaultAsync(m => m.sp_id == id);
            if (service_Provider == null)
            {
                return NotFound();
            }

            return View(service_Provider);
        }

        // GET: Service_Provider/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Service_Provider/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("sp_id,sp_name,sp_add_details,sp_registration_no,sp_fb,sp_google,sp_phone,sp_mobile,sp_password,sp_status,sp_add_1,sp_add_2,sp_add_3,sp_post")] Service_Provider service_Provider)
        {
            if (ModelState.IsValid)
            {
				service_Provider.created_date = DateTime.Now;
				service_Provider.update_date = DateTime.Now;
                _context.Add(service_Provider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service_Provider);
        }

        // GET: Service_Provider/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service_Provider = await _context.Service_Provider.SingleOrDefaultAsync(m => m.sp_id == id);
            if (service_Provider == null)
            {
                return NotFound();
            }
            return View(service_Provider);
        }

        // POST: Service_Provider/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("sp_id,sp_name,sp_add_details,sp_registration_no,sp_fb,sp_google,sp_phone,sp_mobile,sp_password,sp_status,sp_add_1,sp_add_2,sp_add_3,sp_post,created_date")] Service_Provider service_Provider)
        {
            if (id != service_Provider.sp_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					service_Provider.sp_password = GetHash(service_Provider.sp_password);
					service_Provider.update_date = DateTime.Now;
                    _context.Update(service_Provider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Service_ProviderExists(service_Provider.sp_id))
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
            return View(service_Provider);
        }

        // GET: Service_Provider/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service_Provider = await _context.Service_Provider
                .SingleOrDefaultAsync(m => m.sp_id == id);
            if (service_Provider == null)
            {
                return NotFound();
            }

            return View(service_Provider);
        }

        // POST: Service_Provider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var service_Provider = await _context.Service_Provider.SingleOrDefaultAsync(m => m.sp_id == id);
            _context.Service_Provider.Remove(service_Provider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Service_ProviderExists(long id)
        {
            return _context.Service_Provider.Any(e => e.sp_id == id);
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
