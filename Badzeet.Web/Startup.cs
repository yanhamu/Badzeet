using Badzeet.Web.Configuration;
using Badzeet.Web.Configuration.Conventions;
using Badzeet.Web.Configuration.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Badzeet.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddHttpContextAccessor();
            services.AddAuthentication("Cookies").AddCookie("Cookies");

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddTransient<IUserAccountService, UserAccountService>();

            services.RegisterBudgetDependencies(configuration);
            services.RegisterUserDependencies();
            services.RegisterSchedulerDependencies(configuration);
            services.RegisterIntegrationHandlers();
            services.RegisterServiceDependencies();
            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new WebControllerConventions());
                x.Conventions.Add(new ApiControllerConventions());
                x.Conventions.Add(new ApiActionMethodConvention());
            }).AddJsonOptions(x =>
            {
                x.UseDateOnlyTimeOnlyStringConverters();
            });

            services.AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(x =>
                {
                    x.AllowAnyOrigin();
                    x.AllowAnyHeader();
                    x.AllowAnyMethod();
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<DefaultAccountMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}