using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryGo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace LaundryGo.Controllers
{
	[Authorize]
	public class Service_ProviderController : Controller
    {
        private readonly LaundryGoContext _context;
		private readonly LaundryGoContext _tempcontext;

		public Service_ProviderController(LaundryGoContext context)
        {
            _context = context;

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
				service_Provider.sp_password = GetHash(service_Provider.sp_password);
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

			var temp_sp = _context.Service_Provider.AsNoTracking().FirstOrDefault(m => m.sp_id == id);
			if (ModelState.IsValid)
            {
                try
                {
				
					service_Provider.sp_password = temp_sp.sp_password;
					service_Provider.created_date = temp_sp.created_date;
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
