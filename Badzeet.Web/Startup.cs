using Badzeet.Web.Configuration;
using Badzeet.Web.Configuration.Filters;
using Badzeet.Web.Configuration.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddHttpContextAccessor();

            services.AddTransient<Features.Account.Service>();
            services.AddTransient<IUserAccountService, UserAccountService>();

            services.RegisterBudgetDependencies(configuration);
            services.RegisterUserDependencies();
            services.RegisterSchedulerDependencies(configuration);

            services.AddControllersWithViews(x =>
            {
                x.Filters.Add<AccountIdFilter>();
                x.Filters.Add<BudgetIdFilter>();
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