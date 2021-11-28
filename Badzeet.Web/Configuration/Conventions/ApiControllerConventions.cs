using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

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

    public class AccountIdActionMethodConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerType.Namespace.Contains("Api"))
            {
                // TODO create filters for budgetId and accountId
                //action.Filters.Add(new )
            }
        }
    }
}
