using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryGoSP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;

namespace LaundryGo.Controllers
{
	[Authorize]
	public class Order_HeaderController : Controller
    {
        private readonly LaundryGoContext _context;
		
		public Order_HeaderController(LaundryGoContext context)
        {
            _context = context;
        }

		// GET: Order_Header
		//[Authorize(Policy = "Admin")]
		//
		public async Task<IActionResult> Index()
        {
			string query = "SELECT * FROM Order_Header oh left join " +
				"(select cust_name, cust_id as cid from Customer) cust on cust.cid = oh.cust_id left join " +
		"(select dri_name as pickup_dri_name, dri_id as pid from Driver) pd on pd.pid = oh.pickup_dri_id left join " +
	" (select dri_name as deliver_dri_name, dri_id as did from Driver) dd on dd.did = oh.deliver_dri_id left join " +
	" (select status_id as sid,status_name as status_name from status_master) sm on oh.status_id = sm.sid left join " +
 " (select sp_name, sp_id as spid from service_provider) sp on sp.spid = oh.sp_id where status_id=2";
            List<Order_Header> orderheader = _context.Order_Header.FromSql(query).ToList();
			return View(orderheader.ToAsyncEnumerable());
        }


		public async Task<IActionResult> CompletedList()
		{
			string query = "SELECT * FROM Order_Header oh left join " +
				"(select cust_name, cust_id as cid from Customer) cust on cust.cid = oh.cust_id left join " +
		"(select dri_name as pickup_dri_name, dri_id as pid from Driver) pd on pd.pid = oh.pickup_dri_id left join " +
	" (select dri_name as deliver_dri_name, dri_id as did from Driver) dd on dd.did = oh.deliver_dri_id left join " +
		" (select status_id as sid,status_name as status_name from status_master) sm on oh.status_id = sm.sid left join " +
 " (select sp_name, sp_id as spid from service_provider) sp on sp.spid = oh.sp_id where status_id=3";
			List<Order_Header> orderheader = _context.Order_Header.FromSql(query).ToList();
			return View(orderheader.ToAsyncEnumerable());
		}
		// GET: Order_Header/Details/5
		public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Header = await _context.Order_Header
                .SingleOrDefaultAsync(m => m.order_id == id);
            if (order_Header == null)
            {
                return NotFound();
            }

            return View(order_Header);
        }

        // GET: Order_Header/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order_Header/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("order_id,cust_id,pickup_dri_id,sp_id,status_id,order_weight,order_pickup_add,order_deliver_add,deliver_dri_id,request_time,awaiting_pickup_time,pickup_time,washing_time,washing_completed_time,delivering_time,delivered_time,order_amount,payment_method")] Order_Header order_Header)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_Header);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order_Header);
        }

        // GET: Order_Header/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Header = await _context.Order_Header.SingleOrDefaultAsync(m => m.order_id == id);
            if (order_Header == null)
            {
                return NotFound();
            }
            return View(order_Header);
        }

        // POST: Order_Header/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("order_id,cust_id,pickup_dri_id,sp_id,status_id,order_weight,order_pickup_add,order_deliver_add,deliver_dri_id,request_time,awaiting_pickup_time,pickup_time,washing_time,washing_completed_time,delivering_time,delivered_time,order_amount,payment_method")] Order_Header order_Header)
        {
            if (id != order_Header.order_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_Header);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_HeaderExists(order_Header.order_id))
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
            return View(order_Header);
        }

        // GET: Order_Header/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Header = await _context.Order_Header
                .SingleOrDefaultAsync(m => m.order_id == id);
            if (order_Header == null)
            {
                return NotFound();
            }

            return View(order_Header);
        }

		public async Task<IActionResult> Done(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			
			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,sp_id=@sp_id,washing_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@sp_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delivered(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,sp_id=@sp_id,washing_completed_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@sp_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(CompletedList));
		}
		public async Task<IActionResult> Undo(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id-1,sp_id=@sp_id,washing_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@sp_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(CompletedList));
		}

		// POST: Order_Header/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order_Header = await _context.Order_Header.SingleOrDefaultAsync(m => m.order_id == id);
            _context.Order_Header.Remove(order_Header);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_HeaderExists(long id)
        {
            return _context.Order_Header.Any(e => e.order_id == id);
        }
    }
}
