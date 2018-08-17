using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace LaundryGo.Controllers
{
    public class User_AdminController : Controller
    {
        private readonly LaundryGoContext _context;

        public User_AdminController(LaundryGoContext context)
        {
            _context = context;
        }
		//public User_AdminController()
		//{
		//}

		public async Task SignIn(HttpContext httpContext, User_Admin user, bool isPersistent = false)
		{
			user.user_password = GetHash(user.user_password);
			var user_AdminData = await _context.User_Admin
		.SingleOrDefaultAsync(m => m.email_address == user.email_address && m.user_password== user.user_password);
			//ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user_AdminData), CookieAuthenticationDefaults.AuthenticationScheme);
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);
			claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user_AdminData.user_id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user_AdminData.user_name.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Custom Identity", "http://www.w3.org/2001/XMLSchema#string"));

			ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
			//AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

			await httpContext.SignInAsync(principal);
		
			httpContext.User = principal;


		}

			public async void SignOut(HttpContext httpContext)
		{
			await httpContext.SignOutAsync();
		}

		private IEnumerable<Claim> GetUserClaims(User_Admin user)
		{
			List<Claim> claims = new List<Claim>();

			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, user.user_name));
			claims.Add(new Claim(ClaimTypes.Email, user.email_address));
			claims.Add(new Claim("Role", user.user_role));

			//claims.AddRange(this.GetUserRoleClaims(user));
			return claims;
		}

		private IEnumerable<Claim> GetUserRoleClaims(User_Admin user)
		{
			List<Claim> claims = new List<Claim>();

			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()));
			claims.Add(new Claim("Role", user.user_role.ToString()));
			return claims;
		}




		// GET: User_Admin
		[Authorize]
		public async Task<IActionResult> Index()
        {
            return View(await _context.User_Admin.ToListAsync());
        }

        // GET: User_Admin/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user_Admin = await _context.User_Admin
                .SingleOrDefaultAsync(m => m.user_id == id);
            if (user_Admin == null)
            {
                return NotFound();
            }

            return View(user_Admin);
        }

		// GET: User_Admin/Create
		[Authorize]
		public IActionResult Create()
        {
            return View();
        }

        // POST: User_Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("user_id,user_name,email_address,user_password")] User_Admin user_Admin)
        {
            if (ModelState.IsValid)
            {
				user_Admin.user_password = GetHash(user_Admin.user_password);
                _context.Add(user_Admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user_Admin);
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



		// GET: User_Admin/Edit/5
		public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user_Admin = await _context.User_Admin.SingleOrDefaultAsync(m => m.user_id == id);
            if (user_Admin == null)
            {
                return NotFound();
            }
			user_Admin.user_password = "";
            return View(user_Admin);
        }

        // POST: User_Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("user_id,user_name,email_address,user_password")] User_Admin user_Admin)
        {
            if (id != user_Admin.user_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					user_Admin.user_password = GetHash(user_Admin.user_password);
					_context.Update(user_Admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!User_AdminExists(user_Admin.user_id))
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
            return View(user_Admin);
        }

        // GET: User_Admin/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user_Admin = await _context.User_Admin
                .SingleOrDefaultAsync(m => m.user_id == id);
            if (user_Admin == null)
            {
                return NotFound();
            }

            return View(user_Admin);
        }

        // POST: User_Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user_Admin = await _context.User_Admin.SingleOrDefaultAsync(m => m.user_id == id);
            _context.User_Admin.Remove(user_Admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool User_AdminExists(long id)
        {
            return _context.User_Admin.Any(e => e.user_id == id);
        }
    }
}
