using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace LaundryGoSP.Models
{
	public class Customer
    {
		[Key]
		[Display(Name = "ID")]
		public long cust_id { get; set; }
		[Display(Name = "Name")]
		public string cust_name { get; set; }
		[Display(Name = "Facebook")]
		public string cust_fb { get; set; }
		[Display(Name = "Google")]
		public string cust_google { get; set; }
		[Display(Name = "Joined Date")]
		public DateTime created_date { get; set; }
		[Display(Name = "Last Login")]
		public DateTime last_login { get; set; }
		[Display(Name = "Status")]
		public int cust_status { get; set; }
		[Display(Name = "Phone")]
		public string cust_phone { get; set; }

	}
	public class CustDBContext : DbContext
	{
		public DbSet<Customer> LaundryGo { get; set; }
	}
}
