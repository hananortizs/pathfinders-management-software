using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Pms.Backend.Api.Configuration;

namespace Pms.Backend.Api.Infrastructure;

/// <summary>
/// Convention that applies a global route prefix to all controllers
/// </summary>
public class GlobalRoutePrefixConvention : IApplicationModelConvention
{
    private readonly string _routePrefix;

    /// <summary>
    /// Initializes a new instance of the GlobalRoutePrefixConvention class
    /// </summary>
    /// <param name="routePrefix">The route prefix to apply globally</param>
    public GlobalRoutePrefixConvention(string routePrefix)
    {
        _routePrefix = routePrefix;
    }

    /// <summary>
    /// Applies the global route prefix to all controllers
    /// </summary>
    /// <param name="application">The application model</param>
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            // Skip controllers that already have a custom route prefix
            if (HasCustomRoutePrefix(controller))
                continue;

            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    // Apply the global prefix to existing routes
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                        new AttributeRouteModel(new RouteAttribute(_routePrefix)),
                        selector.AttributeRouteModel
                    );
                }
                else
                {
                    // Create a new route model with the prefix
                    selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"{_routePrefix}/[controller]"));
                }
            }
        }
    }

    /// <summary>
    /// Checks if a controller already has a custom route prefix
    /// </summary>
    /// <param name="controller">The controller model</param>
    /// <returns>True if the controller has a custom route prefix</returns>
    private static bool HasCustomRoutePrefix(ControllerModel controller)
    {
        return controller.Selectors.Any(selector => 
            selector.AttributeRouteModel?.Template?.StartsWith("pms") == true ||
            selector.AttributeRouteModel?.Template?.StartsWith("api") == true ||
            selector.AttributeRouteModel?.Template?.StartsWith("pms-loc") == true ||
            selector.AttributeRouteModel?.Template?.StartsWith("pms-prod") == true);
    }
}
