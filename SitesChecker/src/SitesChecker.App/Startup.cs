using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SitesChecker.Core;
using SitesChecker.DataAccess;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;
using SitesChecker.Domain.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SitesChecker.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>();
	        services.AddSingleton<IResponseDataProvider,ResponseDataProvider>();
	        services.AddSingleton<IUrlChecker,UrlChecker>();
	        services.AddScoped<IDataContext,DataContext>();
			services.AddScoped<IMonitoringService,MonitoringHostedService>();
	        services.AddHostedService<BackgroundService>();
			var loggerFactory = new LoggerFactory()
		        .AddSerilog()
		       .AddConsole(LogLevel.Trace);
			//todo add file logger
			services.AddSingleton<ILoggerFactory>(loggerFactory);

	        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		        .AddCookie(options => 
		        {
					options.ExpireTimeSpan=TimeSpan.FromMinutes(2);
			        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
				});
	        services.AddMvc();
			
		}
		

	    private void InitDatabase(IDataContext dbContext)
	    {
			dbContext.InitDatabase();
		    var admin = dbContext.Query<User>().FirstOrDefault(_=>_.Login=="admin"&&_.Password=="admin");
		    if (admin == null)
		    {
			    dbContext.Create(new User()
			    {
					Login = "admin",
				    Password = "admin",
				    Role = "admin"
			    });
			    dbContext.CommitAsync();
		    }

		    var sites = dbContext.Query<Site>();
		    if (!sites.Any())
		    {
				    dbContext.Create(new Site()
				    {
					    Name = "YOUTUBE",
					    Url = "http://youtube.com",
						UpdateDelay = 5
				    });
				    dbContext.CommitAsync();
		    }
		}
	    
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDataContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			
	        InitDatabase(dataContext);
	        app.UseDefaultFiles();
	        app.UseStaticFiles();
	        app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "api/{controller=Statistic}/{action=Index}/{id?}"
				
				);
			});
		}
    }
}
