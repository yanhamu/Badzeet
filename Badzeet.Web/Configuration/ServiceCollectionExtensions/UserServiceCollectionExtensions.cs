using Badzeet.User.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions
{
    public static class UserServiceCollectionExtensions
    {
        public static void RegisterUserDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }
    }
}
