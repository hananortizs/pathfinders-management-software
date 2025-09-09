using System.Text.RegularExpressions;

namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Helper class for CPF (Brazilian tax ID) operations and validation
/// </summary>
public static class CpfHelper
{
    /// <summary>
    /// Regular expression for CPF format validation
    /// Accepts formats: 123.456.789-10 or 12345678910
    /// </summary>
    private static readonly Regex CpfRegex = new(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$", RegexOptions.Compiled);

    /// <summary>
    /// Normalizes CPF for database storage (digits only)
    /// </summary>
    /// <param name="cpf">CPF input</param>
    /// <returns>Normalized CPF (11 digits) or null if invalid</returns>
    public static string? NormalizeCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null;

        // Remove any non-digit characters
        var digitsOnly = Regex.Replace(cpf, @"\D", "");

        // Check if we have exactly 11 digits
        if (digitsOnly.Length != 11)
            return null;

        // Check if it's a valid CPF
        if (!IsValidCpf(digitsOnly))
            return null;

        return digitsOnly;
    }

    /// <summary>
    /// Formats CPF for display (with dots and dash)
    /// </summary>
    /// <param name="cpf">CPF in database format (11 digits)</param>
    /// <returns>Formatted CPF (123.456.789-10) or null if invalid</returns>
    public static string? FormatCpfForDisplay(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null;

        // Remove any non-digit characters
        var digitsOnly = Regex.Replace(cpf, @"\D", "");

        // Check if we have exactly 11 digits
        if (digitsOnly.Length != 11)
            return null;

        // Format as 123.456.789-10
        return $"{digitsOnly[..3]}.{digitsOnly[3..6]}.{digitsOnly[6..9]}-{digitsOnly[9..]}";
    }

    /// <summary>
    /// Validates CPF format and check digits
    /// </summary>
    /// <param name="cpf">CPF to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return true; // CPF is optional

        // Remove any non-digit characters
        var digitsOnly = Regex.Replace(cpf, @"\D", "");

        // Must have exactly 11 digits
        if (digitsOnly.Length != 11)
            return false;

        // Check for invalid patterns (all same digits)
        if (IsAllSameDigits(digitsOnly))
            return false;

        // Validate check digits
        return ValidateCheckDigits(digitsOnly);
    }

    /// <summary>
    /// Gets CPF validation error message
    /// </summary>
    /// <param name="cpf">CPF to validate</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetValidationError(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null; // CPF is optional

        // Remove any non-digit characters
        var digitsOnly = Regex.Replace(cpf, @"\D", "");

        if (digitsOnly.Length != 11)
            return "CPF deve ter 11 dígitos";

        if (IsAllSameDigits(digitsOnly))
            return "CPF não pode ter todos os dígitos iguais";

        if (!ValidateCheckDigits(digitsOnly))
            return "CPF inválido (dígitos verificadores incorretos)";

        return null;
    }

    /// <summary>
    /// Checks if all digits are the same
    /// </summary>
    /// <param name="cpf">CPF digits</param>
    /// <returns>True if all digits are the same</returns>
    private static bool IsAllSameDigits(string cpf)
    {
        return cpf.All(c => c == cpf[0]);
    }

    /// <summary>
    /// Validates CPF check digits
    /// </summary>
    /// <param name="cpf">CPF digits (11 characters)</param>
    /// <returns>True if check digits are valid</returns>
    private static bool ValidateCheckDigits(string cpf)
    {
        // Calculate first check digit
        var sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        }
        var remainder = sum % 11;
        var firstCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != firstCheckDigit)
            return false;

        // Calculate second check digit
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        }
        remainder = sum % 11;
        var secondCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == secondCheckDigit;
    }

    /// <summary>
    /// Masks CPF for display (e.g., 123.***.***-10)
    /// </summary>
    /// <param name="cpf">CPF to mask</param>
    /// <returns>Masked CPF or null if invalid</returns>
    public static string? MaskCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null;

        var normalized = NormalizeCpf(cpf);
        if (normalized == null)
            return null;

        return $"{normalized[..3]}.***.***-{normalized[9..]}";
    }

    /// <summary>
    /// Checks if two CPFs are equivalent (ignoring format)
    /// </summary>
    /// <param name="cpf1">First CPF</param>
    /// <param name="cpf2">Second CPF</param>
    /// <returns>True if equivalent, false otherwise</returns>
    public static bool AreEquivalent(string? cpf1, string? cpf2)
    {
        if (string.IsNullOrWhiteSpace(cpf1) && string.IsNullOrWhiteSpace(cpf2))
            return true;

        if (string.IsNullOrWhiteSpace(cpf1) || string.IsNullOrWhiteSpace(cpf2))
            return false;

        var normalized1 = NormalizeCpf(cpf1);
        var normalized2 = NormalizeCpf(cpf2);

        return normalized1 == normalized2;
    }

    /// <summary>
    /// Generates a random valid CPF for testing purposes
    /// </summary>
    /// <returns>Random valid CPF</returns>
    public static string GenerateRandomCpf()
    {
        var random = new Random();
        var cpf = new int[11];

        // Generate first 9 digits
        for (int i = 0; i < 9; i++)
        {
            cpf[i] = random.Next(0, 10);
        }

        // Calculate first check digit
        var sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += cpf[i] * (10 - i);
        }
        var remainder = sum % 11;
        cpf[9] = remainder < 2 ? 0 : 11 - remainder;

        // Calculate second check digit
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += cpf[i] * (11 - i);
        }
        remainder = sum % 11;
        cpf[10] = remainder < 2 ? 0 : 11 - remainder;

        return string.Join("", cpf);
    }
}

