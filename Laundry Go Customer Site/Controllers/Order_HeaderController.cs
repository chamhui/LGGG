using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laundry_Go_Customer_Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;

namespace Laundry_Go_Customer_Site.Controllers
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
		public IActionResult PlaceOrder()
		{

			return View();
		}

		public IActionResult PlaceOrder2()
		{
			ViewData["add_1"] = HttpContext.Request.Query["add_1"];
			ViewData["add_2"] = HttpContext.Request.Query["add_2"]; 
			ViewData["add_3"] = HttpContext.Request.Query["add_3"]; 
			ViewData["add_4"] = HttpContext.Request.Query["add_4"]; 
			ViewData["add_5"] = HttpContext.Request.Query["add_5"]; 
			ViewData["add_lat"] = HttpContext.Request.Query["add_lat"]; 
			ViewData["add_long"] = HttpContext.Request.Query["add_long"]; 
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PlaceOrder2(string add_1, string add_2, string add_3, string add_4, string add_5, string add_lat, string add_long,
string deliver_add_1, string deliver_add_2, string deliver_add_3, string deliver_add_4, string deliver_add_5, string deliver_lat, string deliver_long)
		{
			string sql = "INSERT INTO Order_Header(cust_id,sp_id,status_id,order_pickup_add,order_deliver_add,request_time,order_amount,order_pickup_add_1,order_pickup_add_2,order_pickup_add_3,order_pickup_add_4,order_pickup_add_5,order_deliver_add_1,order_deliver_add_2,order_deliver_add_3,order_deliver_add_4,order_deliver_add_5,awaiting_pickup_time,pickup_time,washing_time,washing_completed_time,delivering_time,delivered_time,pickup_dri_id,deliver_dri_id,order_weight)      VALUES(@cust_id,@sp_id,@status_id,@order_pickup_add,@order_deliver_add,@request_time,@order_amount,@order_pickup_add_1,@order_pickup_add_2,@order_pickup_add_3,@order_pickup_add_4,@order_pickup_add_5,@order_deliver_add_1,@order_deliver_add_2,@order_deliver_add_3,@order_deliver_add_4,@order_deliver_add_5,getdate(),getdate(),getdate(),getdate(),getdate(),getdate(),0,0,0)";
			var rowsAffected = _context.Database.ExecuteSqlCommand(sql, new SqlParameter("@cust_id", HttpContext.User.Identity.Name), new SqlParameter("@sp_id", ClosestSP(add_long,add_lat)), new SqlParameter("@status_id", "0"), new SqlParameter("@order_pickup_add", add_lat+","+add_long), new SqlParameter("@order_deliver_add", deliver_lat+","+deliver_long), new SqlParameter("@request_time", DateTime.Now), new SqlParameter("@order_amount", "0"), new SqlParameter("@order_pickup_add_1", add_1), new SqlParameter("@order_pickup_add_2", add_2), new SqlParameter("@order_pickup_add_3", add_3), new SqlParameter("@order_pickup_add_4", add_4), new SqlParameter("@order_pickup_add_5", add_5), new SqlParameter("@order_deliver_add_1", deliver_add_1), new SqlParameter("@order_deliver_add_2", deliver_add_2), new SqlParameter("@order_deliver_add_3", deliver_add_3), new SqlParameter("@order_deliver_add_4", deliver_add_4), new SqlParameter("@order_deliver_add_5", deliver_add_5));

		
			return RedirectToAction(nameof(Index));
		}

		public long ClosestSP(string add_long,string add_lat)
		{
			long sp_id = 0;
			List<Service_Provider> lsp = new List<Service_Provider>();
			lsp = _context.Service_Provider.AsNoTracking().ToList();
			decimal currentvalue = 9999;
			foreach(Service_Provider sp in lsp)
			{
				string[] coord = sp.sp_add_details.Split(',');
				decimal temp_long = RemoveNegative(Convert.ToDecimal(add_long) - Convert.ToDecimal(coord[1]));
				decimal temp_lat = RemoveNegative(Convert.ToDecimal(add_lat) - Convert.ToDecimal(coord[0]));
				if ((temp_long + temp_lat) < currentvalue)
				{
					sp_id = sp.sp_id;
					currentvalue = temp_long + temp_lat;
				}
			}


			return sp_id;
		}

		public decimal RemoveNegative(decimal tempvalue)
		{
			if (tempvalue < 0)
			{
				tempvalue = 0 - tempvalue;
			}

			return tempvalue;
		}

		[HttpPost]
		public IActionResult PlaceOrder(string add_1, string add_2, string add_3, string add_4, string add_5, string add_lat, string add_long)
		{
			return RedirectToAction ("PlaceOrder2","Order_Header",new { add_1, add_2, add_3, add_4, add_5, add_lat, add_long });
		}
			public async Task<IActionResult> Index()
        {
			string query = "SELECT * FROM Order_Header oh left join " +
				"(select cust_name, cust_id as cid from Customer) cust on cust.cid = oh.cust_id left join " +
		"(select dri_name as pickup_dri_name, dri_id as pid from Driver) pd on pd.pid = oh.pickup_dri_id left join " +
	" (select dri_name as deliver_dri_name, dri_id as did from Driver) dd on dd.did = oh.deliver_dri_id left join " +
	" (select status_id as sid,status_name as status_name from status_master) sm on oh.status_id = sm.sid left join " +
 " (select sp_name, sp_id as spid from service_provider) sp on sp.spid = oh.sp_id where status_id=0 and cust_id="+HttpContext.User.Identity.GetUserId() ;
            List<Order_Header> orderheader = _context.Order_Header.FromSql(query).ToList();
			return View(orderheader.ToAsyncEnumerable());
        }
		public async Task<IActionResult> AwaitPickupList()
		{
			string query = "SELECT * FROM Order_Header oh left join " +
				"(select cust_name, cust_id as cid from Customer) cust on cust.cid = oh.cust_id left join " +
		"(select dri_name as pickup_dri_name, dri_id as pid from Driver) pd on pd.pid = oh.pickup_dri_id left join " +
	" (select dri_name as deliver_dri_name, dri_id as did from Driver) dd on dd.did = oh.deliver_dri_id left join " +
	" (select status_id as sid,status_name as status_name from status_master) sm on oh.status_id = sm.sid left join " +
 " (select sp_name, sp_id as spid from service_provider) sp on sp.spid = oh.sp_id where status_id=1";
			List<Order_Header> orderheader = _context.Order_Header.FromSql(query).ToList();
			return View(orderheader.ToAsyncEnumerable());
		}


		public async Task<IActionResult> PickupList()
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


		public async Task<IActionResult> WashingInList()
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

		public async Task<IActionResult> WashingCompletedList()
		{
			string query = "SELECT * FROM Order_Header oh left join " +
				"(select cust_name, cust_id as cid from Customer) cust on cust.cid = oh.cust_id left join " +
		"(select dri_name as pickup_dri_name, dri_id as pid from Driver) pd on pd.pid = oh.pickup_dri_id left join " +
	" (select dri_name as deliver_dri_name, dri_id as did from Driver) dd on dd.did = oh.deliver_dri_id left join " +
		" (select status_id as sid,status_name as status_name from status_master) sm on oh.status_id = sm.sid left join " +
 " (select sp_name, sp_id as spid from service_provider) sp on sp.spid = oh.sp_id where status_id=4";
			List<Order_Header> orderheader = _context.Order_Header.FromSql(query).ToList();
			return View(orderheader.ToAsyncEnumerable());
		}

		public async Task<IActionResult> DeliveringList()
		{
			string query = "SELECT * FROM Order_Header oh left join " +
				"(select cust_name, cust_id as cid from Customer) cust on cust.cid = oh.cust_id left join " +
		"(select dri_name as pickup_dri_name, dri_id as pid from Driver) pd on pd.pid = oh.pickup_dri_id left join " +
	" (select dri_name as deliver_dri_name, dri_id as did from Driver) dd on dd.did = oh.deliver_dri_id left join " +
		" (select status_id as sid,status_name as status_name from status_master) sm on oh.status_id = sm.sid left join " +
 " (select sp_name, sp_id as spid from service_provider) sp on sp.spid = oh.sp_id where status_id=5";
			List<Order_Header> orderheader = _context.Order_Header.FromSql(query).ToList();
			return View(orderheader.ToAsyncEnumerable());
		}

		public async Task<IActionResult> DoneList()
		{
			string query = "SELECT * FROM Order_Header oh left join " +
				"(select cust_name, cust_id as cid from Customer) cust on cust.cid = oh.cust_id left join " +
		"(select dri_name as pickup_dri_name, dri_id as pid from Driver) pd on pd.pid = oh.pickup_dri_id left join " +
	" (select dri_name as deliver_dri_name, dri_id as did from Driver) dd on dd.did = oh.deliver_dri_id left join " +
		" (select status_id as sid,status_name as status_name from status_master) sm on oh.status_id = sm.sid left join " +
 " (select sp_name, sp_id as spid from service_provider) sp on sp.spid = oh.sp_id where status_id=6  and cust_id=" + HttpContext.User.Identity.GetUserId();
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

		public async Task<IActionResult> Answer(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,pickup_dri_id=@dri_id,awaiting_pickup_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(DeliveringList));
		}

		public async Task<IActionResult> BackRequest(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id-1 where order_id =@order_id", new SqlParameter("@order_id", id));
			return RedirectToAction(nameof(AwaitPickupList));
		}

		public async Task<IActionResult> ToAwaitPickUpList(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			
			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,pickup_dri_id=@dri_id,awaiting_pickup_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> BackAwaitPickUpList(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id-1 where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(PickupList));
		}

		public async Task<IActionResult> ToPickUp(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,pickup_dri_id=@dri_id,pickup_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(AwaitPickupList));
		}

		public async Task<IActionResult> ToWashing(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,deliver_dri_id=@dri_id,washing_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(PickupList));
		}

		public async Task<IActionResult> BackPickUp(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,pickup_dri_id=@dri_id  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(WashingInList));
		}

		public async Task<IActionResult> ToDelivering(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,deliver_dri_id=@dri_id,delivering_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(WashingCompletedList));
		}
		public async Task<IActionResult> BackWashCompleted(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id-1 where order_id =@order_id", new SqlParameter("@order_id", id));
			return RedirectToAction(nameof(DeliveringList));
		}

		public async Task<IActionResult> ToDone(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,deliver_dri_id=@dri_id,delivered_time= GETDATE()  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(DeliveringList));
		}

		public async Task<IActionResult> BackDelivering(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Order_Header set status_id= status_id+1,deliver_dri_id=@dri_id  where order_id =@order_id", new SqlParameter("@order_id", id), new SqlParameter("@dri_id", HttpContext.User.Identity.Name));
			return RedirectToAction(nameof(DoneList));
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
