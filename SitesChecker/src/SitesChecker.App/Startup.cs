using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SitesChecker.Core;

namespace SitesChecker.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
