using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SitesChecker.Core;
using SitesChecker.DataAccess;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SitesChecker.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddSingleton<IResponseDataProvider,ResponseDataProvider>();
	        services.AddSingleton<IUrlChecker,UrlChecker>();
	        services.AddSingleton<IMonitoringResultsComparer, MonitoringResultsComparer>();
	        services.AddSingleton<IDataContext,DataContext>();
			services.AddSingleton<IMonitoringService,MonitoringHostedService>();
	        services.AddHostedService<BackgroundService>();
			var loggerFactory = new LoggerFactory()
		        .AddSerilog();
		       // .AddConsole(LogLevel.Trace);
			//todo add file logger
			services.AddSingleton<ILoggerFactory>(loggerFactory);
			
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		        .AddJwtBearer(options =>
		        {
			        options.RequireHttpsMetadata = false;
			        options.TokenValidationParameters = new TokenValidationParameters
			        {
				        ValidateIssuer = true,
				        ValidIssuer = AuthOptions.Issuer,
						ValidateAudience = true,
				        ValidAudience = AuthOptions.Audience,
						ValidateLifetime = true,
						IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
				        ValidateIssuerSigningKey = true,
			        };
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

		    var sites = dbContext.Query<SiteAvailability>();
		    if (!sites.Any())
		    {
				    dbContext.Create(new SiteAvailability()
				    {
					    Name = "YOUTUBE",
					    Url = "http://youtube.com"
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
					template: "api/{controller}/{action?}/{id?}",
					defaults: new {controller = "Statistic"}
				);
			});
		}
    }
}
