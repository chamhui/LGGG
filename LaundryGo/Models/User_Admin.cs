using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryGo.Models
{
    public class User_Admin
    {
		[Key]
		[Display(Name = "ID")]
		public long user_id { get; set; }
		[Display(Name = "Name")]
		public string user_name { get; set; }
		[Display(Name = "Email Address")]
		public string email_address { get; set; }
		[Display(Name = "Password")]
		public string user_password { get; set; }
		[Display(Name = "Role")]
		public string user_role { get; set; }
	}
}
