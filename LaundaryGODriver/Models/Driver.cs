using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LaundryGoDriver.Models
{
	public class Driver
	{
		[Key]
		[Display(Name = "ID")]
		public long dri_id { get; set; }
		[Display(Name = "Name")]
		public string dri_name { get; set; }
		[Display(Name = "Facebook")]
		public string dri_fb { get; set; }
		[Display(Name = "Google")]
		public string dri_google { get; set; }
		[Display(Name = "Car Plate")]
		public string dri_car_plate { get; set; }
		[Display(Name = "IC")]
		public string dri_IC { get; set; }
		[Display(Name = "Status")]
		public int dri_status { get; set; }
		[Display(Name = "Phone")]
		public string dri_phone { get; set; }
		[Display(Name = "Address 1")]
		public string dri_c_add_1 { get; set; }
		[Display(Name = "Address 2")]
		public string dri_c_add_2 { get; set; }
		[Display(Name = "Address 3")]
		public string dri_c_add_3 { get; set; }
		[Display(Name = "Post Code")]
		public string dri_c_post { get; set; }
		[NotMapped]
		public StatusType DriverStatus { get; set; }
	}

	public class DriverDBContext : DbContext
	{
		public DbSet<Driver> LaundryGo { get; set; }
	}
	public enum StatusType : int
	{
		[Display(Name = "Active")]
		Active,
		[Display(Name = "Inactive")]
		Inactive,
		[Display(Name = "Banned")]
		Banned
	}

}
