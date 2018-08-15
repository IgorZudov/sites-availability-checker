using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
	    private IConfiguration Configuration;
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddSingleton<IResponseDataProvider,ResponseDataProvider>();
	        services.AddSingleton<IUrlChecker,UrlChecker>();
	        services.AddSingleton<IMonitoringResultsComparer, MonitoringResultsComparer>();
	        services.AddSingleton<IDataContext,DataContext>();
			services.AddSingleton<IMonitoringService,MonitoringHostedService>();
			services.AddSingleton<IHostedService,MonitoringHostedService>();
	        var loggerFactory = new LoggerFactory()
		        .AddSerilog();
		       // .AddConsole(LogLevel.Trace);
			//todo add file logger
			services.AddSingleton<ILoggerFactory>(loggerFactory);

	        services.Configure<RequestsDelay>(Configuration.GetSection("RequestsDelay"));

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

	    public Startup(IHostingEnvironment env)
	    {
		    var builder = new ConfigurationBuilder()
			    .SetBasePath(env.ContentRootPath)
			    .AddJsonFile($"config/delayConfig.json", optional: false, reloadOnChange: true)
			    .AddEnvironmentVariables();
		    Configuration = builder.Build();
		}

	    private void InitDatabase(IDataContext dbContext)
	    {
		    var admin = dbContext.Query<User>().FirstOrDefault(_=>_.Login=="admin"&&_.Password=="admin");
		    if (admin == null)
		    {
			    dbContext.Create(new User()
			    {
					Login = "admin",
				    Password = "admin",
				    Role = "admin"
			    });
		    }

	    }
	    
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDataContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
	        var routeBuilder = new RouteBuilder(app);
	        routeBuilder.MapRoute("default", "api/statistic");
	        app.UseDefaultFiles();
	        app.UseStaticFiles();
	        app.UseAuthentication();
	        app.UseMvc();
	        app.Run(async (context) =>
	        {
		        await context.Response.WriteAsync("Hello World!");
	        });
		}
    }
}
