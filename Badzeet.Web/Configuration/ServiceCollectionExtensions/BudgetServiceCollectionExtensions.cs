using Badzeet.Budget.DataAccess;
using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.DataAccess.Budget;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions
{
    public static class BudgetServiceCollectionExtensions
    {
        public static void RegisterBudgetDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<PaymentsService>();
            services.AddTransient<BudgetService>();
            services.AddTransient<RegistrationService>();

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IDbConnection>(x => new SqlConnection(configuration.GetConnectionString("badzeetDb")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<IBudgetCategoryRepository, BudgetCategoryRepository>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();

            services.AddDbContext<BudgetDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("badzeetDb")); });
        }
    }
}
