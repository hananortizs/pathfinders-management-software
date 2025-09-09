namespace Pms.Backend.Domain.Exceptions;

/// <summary>
/// Base exception class for all custom exceptions in the system
/// </summary>
public abstract class BaseException : Exception
{
    /// <summary>
    /// Error code for the exception
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Additional details about the error
    /// </summary>
    public Dictionary<string, object> Details { get; }

    /// <summary>
    /// Initializes a new instance of the BaseException
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    protected BaseException(string errorCode, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = new Dictionary<string, object>();
    }

    /// <summary>
    /// Initializes a new instance of the BaseException with details
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <param name="message">Error message</param>
    /// <param name="details">Additional details</param>
    /// <param name="innerException">Inner exception</param>
    protected BaseException(string errorCode, string message, Dictionary<string, object> details, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = details ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Adds a detail to the exception
    /// </summary>
    /// <param name="key">Detail key</param>
    /// <param name="value">Detail value</param>
    public void AddDetail(string key, object value)
    {
        Details[key] = value;
    }
}
