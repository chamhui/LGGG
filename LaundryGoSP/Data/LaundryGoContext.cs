using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaundryGoSP.Models;

namespace LaundryGoSP.Models
{
    public class LaundryGoContext : DbContext
    {
        public LaundryGoContext (DbContextOptions<LaundryGoContext> options)
            : base(options)
        {
        }

        public DbSet<LaundryGoSP.Models.Driver> Driver { get; set; }

        public DbSet<LaundryGoSP.Models.Customer> Customer { get; set; }

        public DbSet<LaundryGoSP.Models.Service_Provider> Service_Provider { get; set; }

        public DbSet<LaundryGoSP.Models.Order_Header> Order_Header { get; set; }


    }
}
