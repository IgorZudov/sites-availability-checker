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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
			services.AddSingleton<IHostedService,MonitoringHostedService>();

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

		}
    }
}
