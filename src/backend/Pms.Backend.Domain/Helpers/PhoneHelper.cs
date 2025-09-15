using System.Text.RegularExpressions;

namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Helper class for phone number operations and validation
/// </summary>
public static class PhoneHelper
{
    /// <summary>
    /// Regular expression for international phone validation
    /// Accepts formats: +1 (555) 123-4567, +44 20 7946 0958, +55 11 99999-9999
    /// </summary>
    private static readonly Regex InternationalPhoneRegex = new(
        @"^\+?[1-9]\d{1,14}$",
        RegexOptions.Compiled);

    /// <summary>
    /// Regular expression for Brazilian phone validation (legacy support)
    /// Accepts formats: (11) 99999-9999, 11999999999, +55 11 99999-9999
    /// </summary>
    private static readonly Regex BrazilianPhoneRegex = new(
        @"^(\+55\s?)?(\(?[1-9]{2}\)?)?\s?([9]?[0-9]{4})-?([0-9]{4})$",
        RegexOptions.Compiled);

    /// <summary>
    /// Normalizes phone for database storage (digits only with country code)
    /// </summary>
    /// <param name="phone">Phone input</param>
    /// <returns>Normalized phone (15551234567, 442079460958, 5511999999999) or null if invalid</returns>
    public static string? NormalizePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        // Remove all non-digit characters except +
        var cleaned = phone.Trim();

        // Handle international format (+1, +44, +55, etc.)
        if (cleaned.StartsWith("+"))
        {
            var internationalDigits = Regex.Replace(cleaned, @"\D", "");
            if (internationalDigits.Length >= 7 && internationalDigits.Length <= 15)
                return internationalDigits;
        }

        // Handle Brazilian formats (legacy support)
        var digitsOnly = Regex.Replace(phone, @"\D", "");

        // Brazilian formats
        if (digitsOnly.Length == 10) // (11) 9999-9999
        {
            return $"55{digitsOnly}";
        }
        else if (digitsOnly.Length == 11) // (11) 99999-9999 or 11999999999
        {
            return $"55{digitsOnly}";
        }
        else if (digitsOnly.Length == 13 && digitsOnly.StartsWith("55")) // +55 11 99999-9999
        {
            return digitsOnly;
        }

        // International format without +
        if (digitsOnly.Length >= 7 && digitsOnly.Length <= 15)
        {
            return digitsOnly;
        }

        return null;
    }

    /// <summary>
    /// Formats phone for display (international format)
    /// </summary>
    /// <param name="phone">Phone in database format (15551234567, 442079460958, 5511999999999)</param>
    /// <returns>Formatted phone (+1 (555) 123-4567, +44 20 7946 0958, (11) 99999-9999) or null if invalid</returns>
    public static string? FormatPhoneForDisplay(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        // Remove all non-digit characters
        var digitsOnly = Regex.Replace(phone, @"\D", "");

        // Brazilian format (legacy support)
        if (digitsOnly.StartsWith("55") && digitsOnly.Length == 13)
        {
            var areaCode = digitsOnly.Substring(2, 2);
            var number = digitsOnly.Substring(4);

            if (number.Length == 9)
            {
                return $"({areaCode}) {number.Substring(0, 5)}-{number.Substring(5)}";
            }
            else if (number.Length == 8)
            {
                return $"({areaCode}) {number.Substring(0, 4)}-{number.Substring(4)}";
            }
        }

        // International format
        if (digitsOnly.Length >= 7 && digitsOnly.Length <= 15)
        {
            return $"+{digitsOnly}";
        }

        return null;
    }

    /// <summary>
    /// Validates phone format (international)
    /// </summary>
    /// <param name="phone">Phone to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return true; // Phone is optional

        // Try international format first
        if (InternationalPhoneRegex.IsMatch(phone))
            return true;

        // Try Brazilian format (legacy support)
        if (BrazilianPhoneRegex.IsMatch(phone))
            return true;

        return false;
    }

    /// <summary>
    /// Gets phone validation error message
    /// </summary>
    /// <param name="phone">Phone to validate</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetValidationError(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null; // Phone is optional

        if (!IsValidPhone(phone))
            return "Telefone deve estar no formato internacional (+1 555 123-4567) ou brasileiro ((11) 99999-9999)";

        return null;
    }

    /// <summary>
    /// Extracts country code from phone
    /// </summary>
    /// <param name="phone">Phone number</param>
    /// <returns>Country code or null if invalid</returns>
    public static string? ExtractCountryCode(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        var normalized = NormalizePhone(phone);
        if (normalized == null)
            return null;

        // Brazilian format (legacy support)
        if (normalized.StartsWith("55") && normalized.Length == 13)
            return "55";

        // International format - extract first 1-3 digits as country code
        if (normalized.Length >= 7 && normalized.Length <= 15)
        {
            // Common country codes: 1 (US/CA), 44 (UK), 33 (FR), 49 (DE), etc.
            if (normalized.StartsWith("1") && normalized.Length >= 10)
                return "1";
            else if (normalized.StartsWith("44") && normalized.Length >= 10)
                return "44";
            else if (normalized.StartsWith("33") && normalized.Length >= 10)
                return "33";
            else if (normalized.StartsWith("49") && normalized.Length >= 10)
                return "49";
            else if (normalized.StartsWith("55") && normalized.Length >= 10)
                return "55";
            // For other countries, try to extract first 1-3 digits
            else if (normalized.Length >= 8)
                return normalized.Substring(0, 1);
        }

        return null;
    }

    /// <summary>
    /// Extracts area code from phone (Brazilian format only)
    /// </summary>
    /// <param name="phone">Phone number</param>
    /// <returns>Area code or null if invalid</returns>
    public static string? ExtractAreaCode(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        var normalized = NormalizePhone(phone);
        if (normalized == null)
            return null;

        // Should be 5511XXXXXXXXX format
        if (normalized.Length != 13 || !normalized.StartsWith("55"))
            return null;

        return normalized.Substring(2, 2);
    }

    /// <summary>
    /// Extracts number part from phone (without country code)
    /// </summary>
    /// <param name="phone">Phone number</param>
    /// <returns>Number part or null if invalid</returns>
    public static string? ExtractNumber(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        var normalized = NormalizePhone(phone);
        if (normalized == null)
            return null;

        // Brazilian format (legacy support)
        if (normalized.Length == 13 && normalized.StartsWith("55"))
            return normalized.Substring(4);

        // International format - extract number without country code
        var countryCode = ExtractCountryCode(phone);
        if (countryCode != null)
        {
            return normalized.Substring(countryCode.Length);
        }

        return null;
    }

    /// <summary>
    /// Checks if two phones are equivalent (ignoring format)
    /// </summary>
    /// <param name="phone1">First phone</param>
    /// <param name="phone2">Second phone</param>
    /// <returns>True if equivalent, false otherwise</returns>
    public static bool AreEquivalent(string? phone1, string? phone2)
    {
        if (string.IsNullOrWhiteSpace(phone1) && string.IsNullOrWhiteSpace(phone2))
            return true;

        if (string.IsNullOrWhiteSpace(phone1) || string.IsNullOrWhiteSpace(phone2))
            return false;

        var normalized1 = NormalizePhone(phone1);
        var normalized2 = NormalizePhone(phone2);

        return normalized1 == normalized2;
    }

    /// <summary>
    /// Masks phone for display (e.g., (11) 9****-9999)
    /// </summary>
    /// <param name="phone">Phone to mask</param>
    /// <returns>Masked phone or null if invalid</returns>
    public static string? MaskPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        var normalized = NormalizePhone(phone);
        if (normalized == null)
            return null;

        var areaCode = ExtractAreaCode(normalized);
        var number = ExtractNumber(normalized);

        if (areaCode == null || number == null)
            return null;

        if (number.Length == 9)
        {
            return $"({areaCode}) {number[0]}****-{number.Substring(5)}";
        }
        else if (number.Length == 8)
        {
            return $"({areaCode}) {number[0]}***-{number.Substring(4)}";
        }

        return null;
    }

    /// <summary>
    /// Gets phone type (mobile or landline) based on number
    /// </summary>
    /// <param name="phone">Phone number</param>
    /// <returns>Phone type or null if invalid</returns>
    public static string? GetPhoneType(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        var normalized = NormalizePhone(phone);
        if (normalized == null)
            return null;

        // Brazilian format (legacy support)
        if (normalized.StartsWith("55") && normalized.Length == 13)
        {
            var number = normalized.Substring(4);

            // Mobile numbers start with 9 and have 9 digits
            if (number.Length == 9 && number.StartsWith("9"))
                return "Mobile";

            // Landline numbers have 8 digits
            if (number.Length == 8)
                return "Landline";
        }

        // International format - basic detection
        var countryCode = ExtractCountryCode(phone);
        if (countryCode != null)
        {
            var number = ExtractNumber(phone);
            if (number != null)
            {
                // US/Canada mobile detection
                if (countryCode == "1" && number.Length == 10)
                {
                    var areaCode = number.Substring(0, 3);
                    // US mobile area codes typically start with 2-9
                    if (areaCode[0] >= '2' && areaCode[0] <= '9')
                        return "Mobile";
                    return "Landline";
                }

                // UK mobile detection
                if (countryCode == "44" && number.Length >= 10)
                {
                    if (number.StartsWith("7"))
                        return "Mobile";
                    return "Landline";
                }
            }
        }

        return "Unknown";
    }
}

