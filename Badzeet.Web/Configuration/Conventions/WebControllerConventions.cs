using Badzeet.Web.Configuration.Filters;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Badzeet.Web.Configuration.Conventions;

public class WebControllerConventions : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.Namespace?.Contains("Api") != false) 
            return;
        
        controller.Filters.Add(new AccountIdFilter());
        controller.Filters.Add(new UserIdFilter());
    }
}