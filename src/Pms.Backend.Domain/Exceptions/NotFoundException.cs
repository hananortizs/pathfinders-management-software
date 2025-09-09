namespace Pms.Backend.Domain.Exceptions;

/// <summary>
/// Exception thrown when a resource is not found
/// </summary>
public class NotFoundException : BaseException
{
    /// <summary>
    /// Type of the resource that was not found
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// ID of the resource that was not found
    /// </summary>
    public object? ResourceId { get; }

    /// <summary>
    /// Initializes a new instance of the NotFoundException
    /// </summary>
    /// <param name="resourceType">Type of the resource</param>
    /// <param name="resourceId">ID of the resource</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    public NotFoundException(string resourceType, object? resourceId = null, string? message = null, Exception? innerException = null)
        : base("NOT_FOUND", message ?? $"Resource '{resourceType}' not found", innerException)
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
        AddDetail("ResourceType", ResourceType);
        if (ResourceId != null)
            AddDetail("ResourceId", ResourceId);
    }

    /// <summary>
    /// Creates a NotFoundException for a specific entity
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="id">Entity ID</param>
    /// <param name="message">Custom message</param>
    /// <returns>NotFoundException instance</returns>
    public static NotFoundException ForEntity<T>(object id, string? message = null)
    {
        var entityName = typeof(T).Name;
        return new NotFoundException(entityName, id, message);
    }
}
