namespace Pms.Backend.Api.Extensions;

/// <summary>
/// Extension methods for middleware registration
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds the response standardization middleware to the pipeline
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UseResponseStandardization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Middleware.ResponseStandardizationMiddleware>();
    }
}
