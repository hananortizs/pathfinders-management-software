namespace Pms.Backend.Api.Configuration;

/// <summary>
/// Configuration settings for the API
/// </summary>
public class ApiSettings
{
    /// <summary>
    /// Global route prefix for all API endpoints
    /// Examples:
    /// - Development: "pms-loc" (localhost)
    /// - Staging: "pms-hml" (homologação)
    /// - Production: "pms-prod" (produção)
    /// - Testing: "" (empty for tests)
    /// </summary>
    public string RoutePrefix { get; set; } = "pms";

    /// <summary>
    /// API version for documentation and headers
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// API title for documentation
    /// </summary>
    public string Title { get; set; } = "PMS Backend API";

    /// <summary>
    /// API description for documentation
    /// </summary>
    public string Description { get; set; } = "Pathfinder Management System Backend API";
}
