using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Laundry_Go_Customer_Site.Models;

namespace Laundry_Go_Customer_Site.Models
{
    public class LaundryGoContext : DbContext
    {
        public LaundryGoContext (DbContextOptions<LaundryGoContext> options)
            : base(options)
        {
			
		}

        public DbSet<Laundry_Go_Customer_Site.Models.Driver> Driver { get; set; }

        public DbSet<Laundry_Go_Customer_Site.Models.Customer> Customer { get; set; }

        public DbSet<Laundry_Go_Customer_Site.Models.Service_Provider> Service_Provider { get; set; }

        public DbSet<Laundry_Go_Customer_Site.Models.Order_Header> Order_Header { get; set; }

        public DbSet<Laundry_Go_Customer_Site.Models.Cust_Address> Cust_Address { get; set; }

    }
}
