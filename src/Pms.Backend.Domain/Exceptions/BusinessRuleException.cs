namespace Pms.Backend.Domain.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated
/// </summary>
public class BusinessRuleException : BaseException
{
    /// <summary>
    /// Business rule that was violated
    /// </summary>
    public string BusinessRule { get; }

    /// <summary>
    /// Initializes a new instance of the BusinessRuleException
    /// </summary>
    /// <param name="businessRule">Business rule that was violated</param>
    /// <param name="message">Error message</param>
    /// <param name="details">Additional details</param>
    /// <param name="innerException">Inner exception</param>
    public BusinessRuleException(string businessRule, string message, Dictionary<string, object>? details = null, Exception? innerException = null)
        : base("BUSINESS_RULE_VIOLATION", message, details ?? new Dictionary<string, object>(), innerException)
    {
        BusinessRule = businessRule;
        AddDetail("BusinessRule", BusinessRule);
    }

    /// <summary>
    /// Initializes a new instance of the BusinessRuleException
    /// </summary>
    /// <param name="businessRule">Business rule that was violated</param>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    public BusinessRuleException(string businessRule, string message, Exception? innerException = null)
        : this(businessRule, message, null, innerException)
    {
    }
}
