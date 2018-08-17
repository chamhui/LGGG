using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryGo.Models
{
    public class Service_Provider
    {
		[Key]
		[Display(Name = "ID")]
		public long sp_id { get; set; }
		[Display(Name = "Name")]
		public string sp_name { get; set; }
		[Display(Name = "Address")]
		public string sp_add_details { get; set; }
		[Display(Name = "Registration No")]
		public string sp_registration_no { get; set; }
		[Display(Name = "Facebook")]
		public string sp_fb { get; set; }
		[Display(Name = "Google")]
		public string sp_google { get; set; }
		[Display(Name = "Phone")]
		public string sp_phone { get; set; }
		[Display(Name = "Mobile")]
		public string sp_mobile { get; set; }
		[Display(Name = "Password")]
		public string sp_password { get; set; }
		[Display(Name = "Status")]
		public int sp_status { get; set; }

	
		[Display(Name = "Address 1")]
		public string sp_add_1 { get; set; }
		[Display(Name = "Address 2")]
		public string sp_add_2 { get; set; }
		[Display(Name = "Address 3")]
		public string sp_add_3 { get; set; }
		[Display(Name = "Post Code")]
		public string sp_post { get; set; }
		[Display(Name = "Joined Date")]
		public DateTime created_date { get; set; }
		[Display(Name = "Last Update")]
		public DateTime update_date { get; set; }
	}
}
