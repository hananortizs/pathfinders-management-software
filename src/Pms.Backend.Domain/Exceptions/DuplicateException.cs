namespace Pms.Backend.Domain.Exceptions;

/// <summary>
/// Exception thrown when trying to create a duplicate resource
/// </summary>
public class DuplicateException : BaseException
{
    /// <summary>
    /// Type of the resource that already exists
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// Field that caused the duplicate
    /// </summary>
    public string FieldName { get; }

    /// <summary>
    /// Value that caused the duplicate
    /// </summary>
    public object? FieldValue { get; }

    /// <summary>
    /// Initializes a new instance of the DuplicateException
    /// </summary>
    /// <param name="resourceType">Type of the resource</param>
    /// <param name="fieldName">Field that caused the duplicate</param>
    /// <param name="fieldValue">Value that caused the duplicate</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    public DuplicateException(string resourceType, string fieldName, object? fieldValue = null, string? message = null, Exception? innerException = null)
        : base("DUPLICATE_RESOURCE", message ?? $"Resource '{resourceType}' already exists with {fieldName}", innerException)
    {
        ResourceType = resourceType;
        FieldName = fieldName;
        FieldValue = fieldValue;
        AddDetail("ResourceType", ResourceType);
        AddDetail("FieldName", FieldName);
        if (FieldValue != null)
            AddDetail("FieldValue", FieldValue);
    }

    /// <summary>
    /// Creates a DuplicateException for a specific field
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="fieldName">Field name</param>
    /// <param name="fieldValue">Field value</param>
    /// <param name="message">Custom message</param>
    /// <returns>DuplicateException instance</returns>
    public static DuplicateException ForField<T>(string fieldName, object fieldValue, string? message = null)
    {
        var entityName = typeof(T).Name;
        return new DuplicateException(entityName, fieldName, fieldValue, message);
    }
}
