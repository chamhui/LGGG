using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaundryGo.Models;

namespace LaundryGo.Models
{
    public class LaundryGoContext : DbContext
    {
        public LaundryGoContext (DbContextOptions<LaundryGoContext> options)
            : base(options)
        {
        }

        public DbSet<LaundryGo.Models.Driver> Driver { get; set; }

        public DbSet<LaundryGo.Models.Customer> Customer { get; set; }

        public DbSet<LaundryGo.Models.Service_Provider> Service_Provider { get; set; }

        public DbSet<LaundryGo.Models.Order_Header> Order_Header { get; set; }

        public DbSet<LaundryGo.Models.Cust_Address> Cust_Address { get; set; }

        public DbSet<LaundryGo.Models.User_Admin> User_Admin { get; set; }
    }
}
