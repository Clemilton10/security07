using IdentityExpress.Identity;
using IdentityExpress.Manager.Api;
using IdentityServer4;
using IdentityServer4.Configuration;
using is4.Data;
using is4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace is4
{
	public class Startup
	{
		public IWebHostEnvironment Environment { get; }
		public IConfiguration Configuration { get; }

		public Startup(IWebHostEnvironment environment, IConfiguration configuration)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.Configure<IISOptions>(iis =>
			{
				iis.AuthenticationDisplayName = "Windows";
				iis.AutomaticAuthentication = false;
			});

			services.Configure<IISServerOptions>(iis =>
			{
				iis.AuthenticationDisplayName = "Windows";
				iis.AutomaticAuthentication = false;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlite(Configuration.GetConnectionString("Users")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			var connectionString = Configuration.GetConnectionString("Configuration");
			var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

			var builder = services.AddIdentityServer(options =>
				{
					options.Events.RaiseErrorEvents = true;
					options.Events.RaiseInformationEvents = true;
					options.Events.RaiseFailureEvents = true;
					options.Events.RaiseSuccessEvents = true;

					options.UserInteraction = new UserInteractionOptions
					{
						LogoutUrl = "/Account/Logout",
						LoginUrl = "/Account/Login",
						LoginReturnUrlParameter = "returnUrl"
					};
				})
				.AddAspNetIdentity<ApplicationUser>()
				.AddConfigurationStore(options =>
				{
					options.ConfigureDbContext = db =>
						db.UseSqlite(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));
				})
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = db =>
						db.UseSqlite(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));

					options.EnableTokenCleanup = true;
				});

			builder.AddDeveloperSigningCredential();

			services.AddAuthentication()
				.AddGoogle(options =>
				{
					options.ClientId = "copy client ID from Google here";
					options.ClientSecret = "copy client secret from Google here";
				});

			services.UseAdminUI();
			services.AddScoped<IdentityExpressDbContext, SqliteIdentityDbContext>();
		}

		public void Configure(IApplicationBuilder app)
		{
			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseIdentityServer();
			app.UseAuthorization();

			app.UseAdminUI();

			app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
		}
	}
}
