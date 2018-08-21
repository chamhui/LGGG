using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laundry_Go_Customer_Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Security.Cryptography;

namespace Laundry_Go_Customer_Site.Controllers
{

	public class CustomersController : Controller
    {
        private readonly LaundryGoContext _context;
        public CustomersController(LaundryGoContext context)
        {
            _context = context;
        }

		public async Task SignIn(HttpContext httpContext, Customer user, bool isPersistent = false)
		{
			//user.dri_google = GetHash(user.dri_google);
			var user_SPData = _context.Customer.AsNoTracking().FirstOrDefault(m => m.cust_google == user.cust_google);
			//ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user_AdminData), CookieAuthenticationDefaults.AuthenticationScheme);
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);
			claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user_SPData.cust_id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user_SPData.cust_name.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Custom Identity", "http://www.w3.org/2001/XMLSchema#string"));
			//claimsIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone, user_SPData.cust_phone.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
			//AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

			await httpContext.SignInAsync(principal);

			httpContext.User = principal;
			string aaa = httpContext.User.Identity.Name;
			string bbb = httpContext.User.Identity.GetUserName();
			if(user_SPData.cust_phone is null)
			{
				httpContext.Session.SetString("user_phone","required");
			}
			else
			{
				httpContext.Session.SetString("user_phone", "existed");
			}


		}

		public async Task EmailSignIn(HttpContext httpContext, Customer user, bool isPersistent = false)
		{
			//user.dri_google = GetHash(user.dri_google);
			var user_SPData = _context.Customer.AsNoTracking().FirstOrDefault(m => m.cust_email == user.cust_email && m.cust_phone == user.cust_phone);
			//ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user_AdminData), CookieAuthenticationDefaults.AuthenticationScheme);
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);
			claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user_SPData.cust_id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user_SPData.cust_name.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Custom Identity", "http://www.w3.org/2001/XMLSchema#string"));
			//claimsIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone, user_SPData.cust_phone.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
			ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
			//AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

			await httpContext.SignInAsync(principal);

			httpContext.User = principal;
			string aaa = httpContext.User.Identity.Name;
			string bbb = httpContext.User.Identity.GetUserName();


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


		// GET: Customers

		public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .SingleOrDefaultAsync(m => m.cust_id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("cust_id,cust_name,cust_fb,cust_google,created_date,last_login,cust_status,cust_phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

		public async void RegisterNewCustomer(Customer customer)
		{
			if (ModelState.IsValid)
			{
				
				_context.Add(customer);
				_context.SaveChanges();
				//return RedirectToAction(nameof(Index));
			}
			//return View(customer);
		}

		// GET: Customers/Edit/5
		public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.cust_id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("cust_id,cust_name,cust_fb,cust_google,created_date,last_login,cust_status,cust_phone")] Customer customer)
        {
            if (id != customer.cust_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.cust_id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .SingleOrDefaultAsync(m => m.cust_id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.cust_id == id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(long id)
        {
            return _context.Customer.Any(e => e.cust_id == id);
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
