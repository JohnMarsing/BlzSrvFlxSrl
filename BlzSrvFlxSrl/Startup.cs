using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.Extensions.Options;
using Serilog;
using Blazored.Toast;
using BlzSrvFlxSrl.Settings;

using Blazored.LocalStorage;

namespace BlzSrvFlxSrl;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRazorPages();
		services.AddOptions();

		services.AddServerSideBlazor()
				.AddCircuitOptions(options =>
				{
							//can toggle detailed errors on or off from app settings
							options.DetailedErrors = System.Convert.ToBoolean(Configuration["DetailedErrors"]);
				});

		services.AddDataStores();
		services.AddSession();
		services.AddBlazoredToast();
		services.AddBlazoredModal();
		services.AddBlazoredLocalStorage();
		services.AddCustomAuthentication(Configuration);
		services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));

		services.AddFluxor(o =>
		{
			o.ScanAssemblies(typeof(Startup).Assembly);
#if DEBUG
			o.UseReduxDevTools();
#endif
		});

	}


	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseSerilogRequestLogging();
		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapRazorPages();
			endpoints.MapBlazorHub();
			endpoints.MapFallbackToPage("/_Host");
		});
	}
}
