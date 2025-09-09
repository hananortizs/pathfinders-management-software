namespace Pms.Backend.Api.Extensions;

/// <summary>
/// Extensions for exception handling configuration
/// </summary>
public static class ExceptionHandlingExtensions
{
    /// <summary>
    /// Adds exception handling middleware to the application pipeline
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <returns>Application builder for chaining</returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();
    }

    /// <summary>
    /// Adds exception handling services to the DI container
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        // Register any exception handling related services here
        // For now, the middleware handles everything, but we might add services later
        
        return services;
    }
}
