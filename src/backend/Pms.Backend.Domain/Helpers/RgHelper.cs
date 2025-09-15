using System.Text.RegularExpressions;

namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Helper class for RG (Brazilian ID) operations and validation
/// </summary>
public static class RgHelper
{
    /// <summary>
    /// Regular expression for RG format validation
    /// Accepts formats: 12.345.678-9 or 123456789
    /// </summary>
    private static readonly Regex RgRegex = new(@"^\d{1,2}\.?\d{3}\.?\d{3}-?[0-9X]$", RegexOptions.Compiled);

    /// <summary>
    /// Normalizes RG for database storage (digits and X only)
    /// </summary>
    /// <param name="rg">RG input</param>
    /// <returns>Normalized RG or null if invalid</returns>
    public static string? NormalizeRg(string? rg)
    {
        if (string.IsNullOrWhiteSpace(rg))
            return null;

        // Remove dots and dashes, keep digits and X
        var normalized = Regex.Replace(rg, @"[^\dX]", "").ToUpperInvariant();

        // Check if it's a valid RG format
        if (!IsValidRg(normalized))
            return null;

        return normalized;
    }

    /// <summary>
    /// Formats RG for display (with dots and dash)
    /// </summary>
    /// <param name="rg">RG in database format</param>
    /// <returns>Formatted RG (12.345.678-9) or null if invalid</returns>
    public static string? FormatRgForDisplay(string? rg)
    {
        if (string.IsNullOrWhiteSpace(rg))
            return null;

        var normalized = NormalizeRg(rg);
        if (normalized == null)
            return null;

        // Format based on length
        if (normalized.Length == 9)
        {
            return $"{normalized[..2]}.{normalized[2..5]}.{normalized[5..8]}-{normalized[8]}";
        }
        else if (normalized.Length == 8)
        {
            return $"{normalized[..1]}.{normalized[1..4]}.{normalized[4..7]}-{normalized[7]}";
        }

        return null;
    }

    /// <summary>
    /// Validates RG format
    /// </summary>
    /// <param name="rg">RG to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidRg(string? rg)
    {
        if (string.IsNullOrWhiteSpace(rg))
            return true; // RG is optional

        // Remove dots and dashes, keep digits and X
        var normalized = Regex.Replace(rg, @"[^\dX]", "").ToUpperInvariant();

        // Must have 8 or 9 characters
        if (normalized.Length < 8 || normalized.Length > 9)
            return false;

        // Must match the pattern
        if (!RgRegex.IsMatch(rg))
            return false;

        // Last character must be digit or X
        var lastChar = normalized[^1];
        if (lastChar != 'X' && !char.IsDigit(lastChar))
            return false;

        return true;
    }

    /// <summary>
    /// Gets RG validation error message
    /// </summary>
    /// <param name="rg">RG to validate</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetValidationError(string? rg)
    {
        if (string.IsNullOrWhiteSpace(rg))
            return null; // RG is optional

        // Remove dots and dashes, keep digits and X
        var normalized = Regex.Replace(rg, @"[^\dX]", "").ToUpperInvariant();

        if (normalized.Length < 8 || normalized.Length > 9)
            return "RG deve ter entre 8 e 9 caracteres";

        if (!RgRegex.IsMatch(rg))
            return "RG deve estar no formato 12.345.678-9 ou 123456789";

        var lastChar = normalized[^1];
        if (lastChar != 'X' && !char.IsDigit(lastChar))
            return "RG deve terminar com dígito ou X";

        return null;
    }

    /// <summary>
    /// Masks RG for display (e.g., 12.***.***-9)
    /// </summary>
    /// <param name="rg">RG to mask</param>
    /// <returns>Masked RG or null if invalid</returns>
    public static string? MaskRg(string? rg)
    {
        if (string.IsNullOrWhiteSpace(rg))
            return null;

        var normalized = NormalizeRg(rg);
        if (normalized == null)
            return null;

        if (normalized.Length == 9)
        {
            return $"{normalized[..2]}.***.***-{normalized[8]}";
        }
        else if (normalized.Length == 8)
        {
            return $"{normalized[0]}.***.***-{normalized[7]}";
        }

        return null;
    }

    /// <summary>
    /// Checks if two RGs are equivalent (ignoring format)
    /// </summary>
    /// <param name="rg1">First RG</param>
    /// <param name="rg2">Second RG</param>
    /// <returns>True if equivalent, false otherwise</returns>
    public static bool AreEquivalent(string? rg1, string? rg2)
    {
        if (string.IsNullOrWhiteSpace(rg1) && string.IsNullOrWhiteSpace(rg2))
            return true;

        if (string.IsNullOrWhiteSpace(rg1) || string.IsNullOrWhiteSpace(rg2))
            return false;

        var normalized1 = NormalizeRg(rg1);
        var normalized2 = NormalizeRg(rg2);

        return normalized1 == normalized2;
    }

    /// <summary>
    /// Extracts state code from RG (first 1-2 digits)
    /// </summary>
    /// <param name="rg">RG number</param>
    /// <returns>State code or null if invalid</returns>
    public static string? ExtractStateCode(string? rg)
    {
        if (string.IsNullOrWhiteSpace(rg))
            return null;

        var normalized = NormalizeRg(rg);
        if (normalized == null)
            return null;

        // State code is usually the first 1-2 digits
        if (normalized.Length == 9)
        {
            return normalized[..2];
        }
        else if (normalized.Length == 8)
        {
            return normalized[0].ToString();
        }

        return null;
    }

    /// <summary>
    /// Validates RG state code
    /// </summary>
    /// <param name="rg">RG number</param>
    /// <returns>True if state code is valid</returns>
    public static bool IsValidStateCode(string? rg)
    {
        var stateCode = ExtractStateCode(rg);
        if (stateCode == null)
            return false;

        // Valid state codes (simplified list)
        var validStateCodes = new[]
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", // Single digit
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", // Two digits
            "20", "21", "22", "23", "24", "25", "26", "27"
        };

        return validStateCodes.Contains(stateCode);
    }
}

