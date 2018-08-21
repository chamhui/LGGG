using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laundry_Go_Customer_Site.Models;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Laundry_Go_Customer_Site.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Laundry_Go_Customer_Site.Controllers
{
    public class HomeController : Controller
    {

		protected string googleplus_client_id = "675457524590-g9st6oc3c5j4d9th2t968s0v9as25mju.apps.googleusercontent.com";    // Replace this with your Client ID
		protected string googleplus_client_sceret = "ZaRLF8GwCEUhRYVBqjVqiD7K";                                                // Replace this with your Client Secret
		protected string googleplus_redirect_url = "http://localhost:53050/Home/Index";                                         // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
		protected string Parameters;
		private readonly LaundryGoContext _context;
		public HomeController(LaundryGoContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
        {
			Customer ct =	await Load();
			
				string temp = ct.cust_google;
			if(temp is null)
			{
				return View();
				
			}

			return await Login(ct);




		}
		public ActionResult Logout()
		{

			CustomersController _userManager = new CustomersController(_context);

			_userManager.SignOut(this.HttpContext);
			return RedirectToAction("Index", "Home", null);

		}

		private async Task<Customer> Load()
		{
			Customer ct = new Customer();
			if ((HttpContext.Session.GetString("loginWith") != null) && (HttpContext.Session.GetString("loginWith").ToString() == "google"))
			{
				try
				{
					var url = Request.QueryString.ToString();
					if (url != "")
					{
						string queryString = url.ToString();
						char[] delimiterChars = { '=' };
						string[] words = queryString.Split(delimiterChars);
						string code = words[1];

						if (code != null)
						{
							//get the access token 
							HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
							webRequest.Method = "POST";
							Parameters = "code=" + code + "&client_id=" + googleplus_client_id + "&client_secret=" + googleplus_client_sceret + "&redirect_uri=" + googleplus_redirect_url + "&grant_type=authorization_code";
							byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);
							webRequest.ContentType = "application/x-www-form-urlencoded";
							webRequest.ContentLength = byteArray.Length;
							Stream postStream = webRequest.GetRequestStream();
							// Add the post data to the web request
							postStream.Write(byteArray, 0, byteArray.Length);
							postStream.Close();

							WebResponse response = webRequest.GetResponse();
							postStream = response.GetResponseStream();
							StreamReader reader = new StreamReader(postStream);
							string responseFromServer = reader.ReadToEnd();

							GooglePlusAccessToken serStatus = JsonConvert.DeserializeObject<GooglePlusAccessToken>(responseFromServer);

							if (serStatus != null)
							{
								string accessToken = string.Empty;
								accessToken = serStatus.access_token;

								if (!string.IsNullOrEmpty(accessToken))
								{
									// This is where you want to add the code if login is successful.
									
									ct = await getgoogleplususerdataSer(accessToken);
									//Response.Redirect("Contact");
									
								}
							}

						}
					}
				}
				catch (Exception ex)
				{
					//throw new Exception(ex.Message, ex);
					//Response.Redirect(".aspx");
					//RedirectToAction("Index", "Home", null);
				}


			}
			return ct;
			
		}
		public void GoogleClick()
		{
			var Googleurl = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri=" + googleplus_redirect_url + "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id=" + googleplus_client_id;
			HttpContext.Session.SetString("loginWith","google");
			Response.Redirect(Googleurl);
			
		}
		public async Task<IActionResult> Login(Customer ct)
		{
			try
			{
				var userdata = new Customer()
				{
					cust_google = ct.cust_google,
					cust_name =ct.cust_name,
								cust_email = ct.cust_email

				};
				CustomersController _userManager = new CustomersController(this._context);
				await _userManager.SignIn(this.HttpContext, userdata);

			}
			catch (Exception err)
			{
				var userdata = new Customer()
				{
					cust_google = ct.cust_google,
					cust_name = ct.cust_name,
					cust_email= ct.cust_email

				};
				userdata.created_date = DateTime.Now;
				userdata.cust_status = 1;
				CustomersController _userManager = new CustomersController(_context);
				_userManager.RegisterNewCustomer(userdata);
				await _userManager.SignIn(this.HttpContext, userdata);
			}
			if(HttpContext.Session.GetString("user_phone")== "required")
			{
				return RedirectToAction("PhoneVerification", "Home", null);
			}

			return RedirectToAction("Index", "Order_Header", null);
		}


		public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
		public IActionResult PhoneVerification()
		{
			//ViewData["Message"] = "Your contact page.";

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> PhoneVerification(string user_phone)
		{
			var rowsAffected = _context.Database.ExecuteSqlCommand("Update Customer set cust_phone=@user_phone  where cust_id =@cust_id", new SqlParameter("@user_phone", user_phone), new SqlParameter("@cust_id", HttpContext.User.Identity.Name));
			return RedirectToAction("Index", "Order_Header", null);
		}

		[HttpPost]
		public async Task<IActionResult> Index(string user_phone,string user_email)
		{
			CustomersController _userManager = new CustomersController(this._context);
			var userdata = new Customer()
			{
				cust_email = user_email,
				cust_phone = user_phone
			};
			if (!ModelState.IsValid)
				return View();
			try
			{
			
				await _userManager.EmailSignIn(this.HttpContext, userdata);
			}
			catch { 
			if(HttpContext.User.Identity.IsAuthenticated ==false && CheckPhone(user_phone,user_email))
			{
					userdata.cust_name = user_email;
					_userManager.RegisterNewCustomer(userdata);
			
				await _userManager.EmailSignIn(this.HttpContext, userdata);
			}
			}
			return RedirectToAction("Index", "Order_Header", null);
		}

		public Boolean CheckPhone(string user_phone,string user_email)
		{
			//var rowsAffected = _context.Database.ExecuteSqlCommand("SELECT * FROM Customer where cust_phone=@user_phone", new SqlParameter("@user_phone", user_phone));

			int count = _context.Customer.Where(m => m.cust_phone == user_phone || m.cust_email == user_email).Count();

			if (count > 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		private async Task<Customer> getgoogleplususerdataSer(string access_token)
		{
			Customer userdata = new Customer();
			try
			{
				HttpClient client = new HttpClient();
				var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;

				client.CancelPendingRequests();
				HttpResponseMessage output = await client.GetAsync(urlProfile);

				if (output.IsSuccessStatusCode)
				{
					string outputData = await output.Content.ReadAsStringAsync();
					GoogleUserOutputData serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

					if (serStatus != null)
					{
						//HttpContext.Session.SetString("userid", serStatus.id);
						//HttpContext.Session.SetString("useremail", serStatus.email);
						//HttpContext.Session.SetString("usergiven_name", serStatus.given_name);
						//HttpContext.Session.SetString("username", serStatus.name);
						string id = serStatus.id;
						string email = serStatus.email;
						string given_name = serStatus.given_name;
						string name = serStatus.name;

						userdata.cust_google = id;
						userdata.cust_name = name;
						userdata.cust_email = email;

						//var result = new User_AdminController(_context).SignIn(HttpContext, userdata);
						//RedirectToAction("../{User_AdminController}/SignIn", new { httpContext = this.HttpContext, user= userdata });
						//try {
						//	var userdata = new Customer()
						//	{
						//		cust_google = id,
						//		cust_name = name

						//	};
						//	CustomersController _userManager = new CustomersController(this._context);
						//	 _userManager.SignIn(this.HttpContext, userdata);

						//}
						//catch(Exception err)
						//{
						//	var userdata = new Customer()
						//	{
						//		cust_google = id,
						//		cust_name = name

						//	};
						//	userdata.created_date = DateTime.Now;
						//	userdata.cust_status = 1;
						//	CustomersController _userManager = new CustomersController(_context);
						//	_userManager.RegisterNewCustomer(userdata);
						//}
						// You will get the user information here.

					}
					//return RedirectToAction("Index", "Order_Header", null);
				}
			}
			catch (Exception ex)
			{
				//return RedirectToAction("Index", "Home", null);
				//catching the exception
			}
			return userdata;
			//return RedirectToAction("Index", "Home", null);
		}
	}
	public class GooglePlusAccessToken
	{
		public string access_token { get; set; }
		public string token_type { get; set; }
		public int expires_in { get; set; }
		public string refresh_token { get; set; }
	}
	public class GoogleUserOutputData
	{
		public string id { get; set; }
		public string name { get; set; }
		public string given_name { get; set; }
		public string email { get; set; }
		public string picture { get; set; }
	}


}

