using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryGo.Models
{
    public class Cust_Address
    {
        [Key]
        [Display(Name = "ID")]
        public long cust_add_id { get; set; }
        [Display(Name = "cust_id")]
        public long cust_id { get; set; }
        [Display(Name = "Name")]
        public long cust_add_name { get; set; }
        [Display(Name = "Details")]
        public long cust_add_details { get; set; }
        [Display(Name = "Created Date")]
        public long created_date { get; set; }
        [Display(Name = "Updated Date")]
        public long updated_date { get; set; }
    }
    public class Cust_AddDBContext : DbContext
    {
        public DbSet<Cust_Address> LaundryGo { get; set; }
    }
}
