using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using SkyCommerce.Data.Configuration;
using SkyCommerce.Data.Context;
using SkyCommerce.Site.Configure;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace SkyCommerce.Site
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews()
				.AddRazorRuntimeCompilation();
			services.AddHttpContextAccessor();

			JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
			if (Debugger.IsAttached)
				IdentityModelEventSource.ShowPII = true;
			services.AddAuthentication(o =>
			{
				o.DefaultScheme = "Cookies";
				o.DefaultChallengeScheme = "oidc";
			})
			.AddCookie("Cookies")
			.AddOpenIdConnect("oidc", options =>
			{
				options.Authority = "https://localhost:5001";
				options.ClientId = "76d81e44d2ab4a5293109d8ce77574a4";
				options.ClientSecret = "0ed41f19d13341dc9b68faf1fd642cc4";
				options.ResponseType = "code";
				options.Scope.Add("profile");
				options.Scope.Add("openid");
				//options.Scope.Add("api_frete");
				options.SaveTokens = true;
				options.GetClaimsFromUserInfoEndpoint = true;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					NameClaimType = "name",
					RoleClaimType = "role"
				};
			});
			services.AddHttpClient();
			services.ConfigureProviderForContext<SkyContext>(DetectDatabase);
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = new PathString("/conta/entrar");
			});
			services.ConfigureSkyCommerce();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			// Definindo a cultura padrão: pt-BR
			var supportedCultures = new[] { new CultureInfo("pt-BR") };
			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		/// <summary>
		/// it's just a tuple. Returns 2 parameters.
		/// Trying to improve readability at ConfigureServices
		/// </summary>
		private (DatabaseType, string) DetectDatabase => (
			Configuration.GetValue<DatabaseType>("ApplicationSettings:DatabaseType"),
			Configuration.GetConnectionString("DefaultConnection"));
	}
}
