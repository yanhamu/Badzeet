using Badzeet.Web.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Linq;

namespace Badzeet.Web.Configuration.Conventions
{
    public class ApiControllerConventions : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.Namespace.Contains("Api"))
            {
                var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();
                controller.Filters.Add(new AuthorizeFilter(policy));
            }
        }
    }

    public class ApiActionMethodConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerType.Namespace.Contains("Api"))
            {
                if (action.Parameters.Any(p => p.Name == "accountId"))
                {
                    action.Filters.Add(new AccountIdFilter());
                }

                if (action.Parameters.Any(p => p.Name == "budgetId"))
                {
                    action.Filters.Add(new BudgetIdFilter());
                }
            }
        }
    }
}
