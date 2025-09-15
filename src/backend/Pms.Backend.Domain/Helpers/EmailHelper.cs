using System.Text.RegularExpressions;

namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Helper class for email operations and validation
/// </summary>
public static class EmailHelper
{
    /// <summary>
    /// Regular expression for email validation (RFC 5322 compliant)
    /// </summary>
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Normalizes email for database storage (lowercase, trimmed)
    /// </summary>
    /// <param name="email">Email input</param>
    /// <returns>Normalized email or null if invalid</returns>
    public static string? NormalizeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalized = email.Trim().ToLowerInvariant();
        
        if (!IsValidEmail(normalized))
            return null;

        return normalized;
    }

    /// <summary>
    /// Validates email format
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        if (email.Length > 255) // RFC 5321 limit
            return false;

        return EmailRegex.IsMatch(email);
    }

    /// <summary>
    /// Gets email validation error message
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetValidationError(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return "Email é obrigatório";

        if (email.Length > 255)
            return "Email não pode exceder 255 caracteres";

        if (!IsValidEmail(email))
            return "Email deve estar em um formato válido (exemplo: usuario@dominio.com)";

        return null;
    }

    /// <summary>
    /// Extracts domain from email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>Domain part or null if invalid</returns>
    public static string? ExtractDomain(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalized = NormalizeEmail(email);
        if (normalized == null)
            return null;

        var atIndex = normalized.IndexOf('@');
        if (atIndex == -1)
            return null;

        return normalized.Substring(atIndex + 1);
    }

    /// <summary>
    /// Extracts local part from email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>Local part or null if invalid</returns>
    public static string? ExtractLocalPart(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalized = NormalizeEmail(email);
        if (normalized == null)
            return null;

        var atIndex = normalized.IndexOf('@');
        if (atIndex == -1)
            return null;

        return normalized.Substring(0, atIndex);
    }

    /// <summary>
    /// Checks if two emails are equivalent (ignoring case and whitespace)
    /// </summary>
    /// <param name="email1">First email</param>
    /// <param name="email2">Second email</param>
    /// <returns>True if equivalent, false otherwise</returns>
    public static bool AreEquivalent(string? email1, string? email2)
    {
        if (string.IsNullOrWhiteSpace(email1) && string.IsNullOrWhiteSpace(email2))
            return true;

        if (string.IsNullOrWhiteSpace(email1) || string.IsNullOrWhiteSpace(email2))
            return false;

        var normalized1 = NormalizeEmail(email1);
        var normalized2 = NormalizeEmail(email2);

        return normalized1 == normalized2;
    }

    /// <summary>
    /// Masks email for display (e.g., u***@domain.com)
    /// </summary>
    /// <param name="email">Email to mask</param>
    /// <returns>Masked email or null if invalid</returns>
    public static string? MaskEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalized = NormalizeEmail(email);
        if (normalized == null)
            return null;

        var localPart = ExtractLocalPart(normalized);
        var domain = ExtractDomain(normalized);

        if (localPart == null || domain == null)
            return null;

        if (localPart.Length <= 2)
            return $"{localPart[0]}***@{domain}";

        return $"{localPart[0]}{new string('*', localPart.Length - 2)}{localPart[^1]}@{domain}";
    }
}

