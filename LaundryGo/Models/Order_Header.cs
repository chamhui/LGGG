using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryGo.Models
{
    public class Order_Header
	{
		[Key]
		[Display(Name = "ID")]
		public long order_id { get; set; }
		[Display(Name = "Customer ID")]
		public long cust_id { get; set; }
		[Display(Name = "Pickup Drive ID")]
		public long pickup_dri_id { get; set; }
		[Display(Name = "Service Provider ID")]
		public long sp_id { get; set; }
		[Display(Name = "Status")]
		public int status_id { get; set; }
		[Display(Name = "Weight")]
		public decimal order_weight { get; set; }
		[Display(Name = "order_pickup_add")]
		public string order_pickup_add { get; set; }
		[Display(Name = "order_deliver_add")]
		public string order_deliver_add { get; set; }
		[Display(Name = "Deliver Driver ID")]
		public long deliver_dri_id { get; set; }
		[Display(Name = "Request Time")]
		public DateTime request_time { get; set; }
		[Display(Name = "Awaiting Pick Up Time")]
		public DateTime awaiting_pickup_time { get; set; }
		[Display(Name = "Pick Up Time")]
		public DateTime pickup_time { get; set; }
		[Display(Name = "Washing Time")]
		public DateTime washing_time { get; set; }
		[Display(Name = "Washing Completed Time")]
		public DateTime washing_completed_time { get; set; }
		[Display(Name = "Delivering Time")]
		public DateTime delivering_time { get; set; }
		[Display(Name = "Delivered Time")]
		public DateTime delivered_time { get; set; }
		[Display(Name = "Amount")]
		public decimal order_amount { get; set; }
		[Display(Name = "order_pickup_add_1")]
		public string order_pickup_add_1 { get; set; }
		[Display(Name = "order_deliver_add_1")]
		public string order_deliver_add_1 { get; set; }
		[Display(Name = "order_pickup_add_2")]
		public string order_pickup_add_2 { get; set; }
		[Display(Name = "order_deliver_add_2")]
		public string order_deliver_add_2 { get; set; }
		[Display(Name = "order_pickup_add_3")]
		public string order_pickup_add_3 { get; set; }
		[Display(Name = "order_deliver_add_3")]
		public string order_deliver_add_3 { get; set; }
		[Display(Name = "order_pickup_add_4")]
		public string order_pickup_add_4 { get; set; }
		[Display(Name = "order_deliver_add_4")]
		public string order_deliver_add_4 { get; set; }
		[Display(Name = "order_pickup_add_5")]
		public string order_pickup_add_5 { get; set; }
		[Display(Name = "order_deliver_add_5")]
		public string order_deliver_add_5 { get; set; }



		[Display(Name = "Payment Method")]
		public string payment_method { get; set; }

		[Display(Name = "Customer Name")]
		public virtual string cust_name { get; set; }
		
		[Display(Name = "Pick Up Driver Name")]
		public virtual string pickup_dri_name { get; set; }

		[Display(Name = "Deliver Driver Name")]
		public virtual string deliver_dri_name { get; set; }
	
		[Display(Name = "Service Provider Name")]
		public virtual string sp_name { get; set; }
	}
	public class Order_HeaderDBContext : DbContext
	{
		public DbSet<Order_Header> LaundryGo { get; set; }
	}
	public enum OrderStatusType : int
	{
		[Display(Name = "Request")]
		Request,
		[Display(Name = "Awaiting Pickup")]
		Awaiting_Pickup,
		[Display(Name = "Pick Up")]
		Pick_Up,
			[Display(Name = "Washing In Progress")]
		Washing_In_Progress,
			[Display(Name = "Washing Completed")]
		Washing_Completed,
			[Display(Name = "Delivering")]
		Delivering,
			[Display(Name = "Done")]
		Done
	}

}
