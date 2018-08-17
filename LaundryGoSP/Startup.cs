using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LaundryGoSP.Models;

namespace LaundryGoSP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(
		CookieAuthenticationDefaults.AuthenticationScheme
).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
		options =>
		{
			options.LoginPath = "/Home/Login";
			options.LogoutPath = "/Home/Login";
		});
			services.AddSession();

			services.AddAuthorization(options =>
			{
			});

			//services.AddAuthentication(options =>
			//{
			//	//options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//	//options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//	//options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

			//	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

			//	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

			//	options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;

			//	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

			//	options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//});
			services.AddMvc();

			services.AddDbContext<LaundryGoContext>(options =>
										options.UseSqlServer(Configuration.GetConnectionString("LaundryGoContext")));
			services.AddDistributedMemoryCache();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseSession();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
						name: "default",
						template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
