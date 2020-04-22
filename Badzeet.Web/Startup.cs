using Badzeet.DataAccess.Book;
using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Badzeet.Domain.Budget;
using Badzeet.Domain.Budget.Interfaces;
using Badzeet.Service.User;
using Badzeet.Web.Configuration;
using Badzeet.Web.Configuration.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace Badzeet.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddHttpContextAccessor();

            services.AddTransient<Features.Account.Service>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<PaymentsService>();
            services.AddTransient<BookService>();
            services.AddTransient<RegistrationService>();
            services.AddTransient<ScheduledPaymentsService>();
            
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserBookRepository, UserBookRepository>();
            services.AddScoped<IDbConnection>(x => new SqlConnection(configuration.GetConnectionString("badzeetDb")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<ICategoryBudgetRepository, CategoryBudgetRepository>();
            services.AddScoped<IScheduledPaymentRepository, ScheduledPaymentRepository>();

            services.AddDbContext<BookDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("badzeetDb")); });

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
