using System.Text.RegularExpressions;

namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Helper class for CEP (Brazilian postal code) operations
/// </summary>
public static class CepHelper
{
    /// <summary>
    /// Regular expression for CEP validation
    /// Accepts both formats: 12345-678 or 12345678
    /// </summary>
    private static readonly Regex CepRegex = new(@"^\d{5}-?\d{3}$", RegexOptions.Compiled);

    /// <summary>
    /// Normalizes CEP to database format (digits only, with leading zeros)
    /// </summary>
    /// <param name="cep">CEP input (can be with or without dash)</param>
    /// <returns>Normalized CEP (8 digits) or null if invalid</returns>
    public static string? NormalizeCep(string? cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return null;

        // Remove any non-digit characters
        var digitsOnly = Regex.Replace(cep, @"\D", "");

        // Check if we have exactly 8 digits
        if (digitsOnly.Length != 8)
            return null;

        // Ensure leading zeros are preserved
        return digitsOnly.PadLeft(8, '0');
    }

    /// <summary>
    /// Formats CEP for display (with dash)
    /// </summary>
    /// <param name="cep">CEP in database format (8 digits)</param>
    /// <returns>Formatted CEP (12345-678) or null if invalid</returns>
    public static string? FormatCepForDisplay(string? cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return null;

        // Remove any non-digit characters
        var digitsOnly = Regex.Replace(cep, @"\D", "");

        // Check if we have exactly 8 digits
        if (digitsOnly.Length != 8)
            return null;

        // Format as 12345-678
        return $"{digitsOnly[..5]}-{digitsOnly[5..]}";
    }

    /// <summary>
    /// Validates CEP format
    /// </summary>
    /// <param name="cep">CEP to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidCep(string? cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return true; // CEP is optional

        return CepRegex.IsMatch(cep);
    }

    /// <summary>
    /// Gets CEP validation error message
    /// </summary>
    /// <param name="cep">CEP to validate</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetValidationError(string? cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return null; // CEP is optional

        if (!IsValidCep(cep))
            return "CEP deve estar no formato 12345-678 ou 12345678";

        return null;
    }

    /// <summary>
    /// Extracts only digits from CEP
    /// </summary>
    /// <param name="cep">CEP input</param>
    /// <returns>Digits only or null if invalid</returns>
    public static string? ExtractDigits(string? cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return null;

        var digitsOnly = Regex.Replace(cep, @"\D", "");
        return digitsOnly.Length == 8 ? digitsOnly : null;
    }

    /// <summary>
    /// Checks if two CEPs are equivalent (ignoring format)
    /// </summary>
    /// <param name="cep1">First CEP</param>
    /// <param name="cep2">Second CEP</param>
    /// <returns>True if equivalent, false otherwise</returns>
    public static bool AreEquivalent(string? cep1, string? cep2)
    {
        if (string.IsNullOrWhiteSpace(cep1) && string.IsNullOrWhiteSpace(cep2))
            return true;

        if (string.IsNullOrWhiteSpace(cep1) || string.IsNullOrWhiteSpace(cep2))
            return false;

        var normalized1 = NormalizeCep(cep1);
        var normalized2 = NormalizeCep(cep2);

        return normalized1 == normalized2;
    }
}
