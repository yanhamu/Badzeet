using Badzeet.Web.Configuration;
using Badzeet.Web.Configuration.Filters;
using Badzeet.Web.Configuration.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Badzeet.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;

        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            this.configuration = configuration;
            this.environment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                .AddInMemoryClients(IdentityServerConfig.Clients(configuration))
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options => {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetConnectionString("badzeetDb"));
                    options.DefaultSchema = "id4";
                });

            services.AddHttpContextAccessor();
            services.AddAuthentication("Cookies")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["Authority:Url"];
                    options.TokenValidationParameters = new TokenValidationParameters() { ValidateAudience = false };

                })
                .AddCookie("Cookies");

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddTransient<IUserAccountService, UserAccountService>();

            services.RegisterBudgetDependencies(configuration);
            services.RegisterUserDependencies();
            services.RegisterSchedulerDependencies(configuration);
            services.RegisterIntegrationHandlers();
            services.RegisterServiceDependencies();

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
            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
                x.AllowAnyMethod();
            });

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