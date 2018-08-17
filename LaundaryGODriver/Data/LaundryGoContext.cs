using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaundryGoDriver.Models;

namespace LaundryGoDriver.Models
{
    public class LaundryGoContext : DbContext
    {
        public LaundryGoContext (DbContextOptions<LaundryGoContext> options)
            : base(options)
        {
        }

        public DbSet<LaundryGoDriver.Models.Driver> Driver { get; set; }

        public DbSet<LaundryGoDriver.Models.Customer> Customer { get; set; }

        public DbSet<LaundryGoDriver.Models.Service_Provider> Service_Provider { get; set; }

        public DbSet<LaundryGoDriver.Models.Order_Header> Order_Header { get; set; }


     
    }
}
