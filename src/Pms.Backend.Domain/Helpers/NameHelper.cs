using System.Text.RegularExpressions;

namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Helper class for name validation and normalization
/// </summary>
public static class NameHelper
{
    /// <summary>
    /// Regular expression for valid name characters (letters, spaces, hyphens, apostrophes)
    /// </summary>
    private static readonly Regex ValidNameRegex = new(
        @"^[a-zA-ZÀ-ÿ\s\-']+$",
        RegexOptions.Compiled);

    /// <summary>
    /// Regular expression for multiple spaces
    /// </summary>
    private static readonly Regex MultipleSpacesRegex = new(
        @"\s+",
        RegexOptions.Compiled);

    /// <summary>
    /// Minimum length for a valid name
    /// </summary>
    private const int MinimumNameLength = 2;

    /// <summary>
    /// Maximum length for a valid name
    /// </summary>
    private const int MaximumNameLength = 100;

    /// <summary>
    /// Normalizes a name by trimming, removing extra spaces, and proper case formatting
    /// </summary>
    /// <param name="name">Name to normalize</param>
    /// <returns>Normalized name or null if invalid</returns>
    public static string? NormalizeName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        // Trim and remove extra spaces
        var normalized = MultipleSpacesRegex.Replace(name.Trim(), " ");

        // Check if valid after normalization
        if (!IsValidName(normalized))
            return null;

        // Apply proper case formatting
        return FormatProperCase(normalized);
    }

    /// <summary>
    /// Validates if a name is valid
    /// </summary>
    /// <param name="name">Name to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var trimmed = name.Trim();

        if (trimmed.Length < MinimumNameLength || trimmed.Length > MaximumNameLength)
            return false;

        if (!ValidNameRegex.IsMatch(trimmed))
            return false;

        // Check for invalid patterns
        if (IsInvalidNamePattern(trimmed))
            return false;

        return true;
    }

    /// <summary>
    /// Gets the validation error message for a name
    /// </summary>
    /// <param name="name">Name to validate</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetValidationError(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Nome é obrigatório";

        var trimmed = name.Trim();

        if (trimmed.Length < MinimumNameLength)
            return $"Nome deve ter pelo menos {MinimumNameLength} caracteres";

        if (trimmed.Length > MaximumNameLength)
            return $"Nome não pode exceder {MaximumNameLength} caracteres";

        if (!ValidNameRegex.IsMatch(trimmed))
            return "Nome deve conter apenas letras, espaços, hífens e apostrofes";

        if (IsInvalidNamePattern(trimmed))
            return "Nome contém padrões inválidos (apenas espaços, hífens ou apostrofes)";

        return null;
    }

    /// <summary>
    /// Formats a name with proper case (first letter of each word capitalized)
    /// </summary>
    /// <param name="name">Name to format</param>
    /// <returns>Formatted name</returns>
    public static string FormatProperCase(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var formattedWords = new string[words.Length];

        for (int i = 0; i < words.Length; i++)
        {
            var word = words[i].ToLowerInvariant();

            // Handle special cases for hyphens and apostrophes
            if (word.Contains('-'))
            {
                var parts = word.Split('-');
                for (int j = 0; j < parts.Length; j++)
                {
                    if (!string.IsNullOrEmpty(parts[j]))
                    {
                        parts[j] = CapitalizeFirstLetter(parts[j]);
                    }
                }
                formattedWords[i] = string.Join("-", parts);
            }
            else if (word.Contains("'"))
            {
                var parts = word.Split('\'');
                for (int j = 0; j < parts.Length; j++)
                {
                    if (!string.IsNullOrEmpty(parts[j]))
                    {
                        parts[j] = CapitalizeFirstLetter(parts[j]);
                    }
                }
                formattedWords[i] = string.Join("'", parts);
            }
            else
            {
                formattedWords[i] = CapitalizeFirstLetter(word);
            }
        }

        return string.Join(" ", formattedWords);
    }

    /// <summary>
    /// Capitalizes the first letter of a word
    /// </summary>
    /// <param name="word">Word to capitalize</param>
    /// <returns>Word with first letter capitalized</returns>
    private static string CapitalizeFirstLetter(string word)
    {
        if (string.IsNullOrEmpty(word))
            return word;

        return char.ToUpperInvariant(word[0]) + word.Substring(1);
    }

    /// <summary>
    /// Checks if a name has invalid patterns
    /// </summary>
    /// <param name="name">Name to check</param>
    /// <returns>True if has invalid patterns, false otherwise</returns>
    private static bool IsInvalidNamePattern(string name)
    {
        // Check for names that are only spaces, hyphens, or apostrophes
        var withoutSpaces = name.Replace(" ", "");
        var withoutHyphens = withoutSpaces.Replace("-", "");
        var withoutApostrophes = withoutHyphens.Replace("'", "");

        return string.IsNullOrEmpty(withoutApostrophes);
    }

    /// <summary>
    /// Validates a full name (first name + middle names + last name)
    /// </summary>
    /// <param name="firstName">First name</param>
    /// <param name="middleNames">Middle names (optional)</param>
    /// <param name="lastName">Last name</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidFullName(string firstName, string? middleNames, string lastName)
    {
        if (!IsValidName(firstName) || !IsValidName(lastName))
            return false;

        if (!string.IsNullOrWhiteSpace(middleNames) && !IsValidName(middleNames))
            return false;

        return true;
    }

    /// <summary>
    /// Gets the validation error for a full name
    /// </summary>
    /// <param name="firstName">First name</param>
    /// <param name="middleNames">Middle names (optional)</param>
    /// <param name="lastName">Last name</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetFullNameValidationError(string firstName, string? middleNames, string lastName)
    {
        var firstNameError = GetValidationError(firstName);
        if (firstNameError != null)
            return $"Nome: {firstNameError}";

        var lastNameError = GetValidationError(lastName);
        if (lastNameError != null)
            return $"Sobrenome: {lastNameError}";

        if (!string.IsNullOrWhiteSpace(middleNames))
        {
            var middleNamesError = GetValidationError(middleNames);
            if (middleNamesError != null)
                return $"Sobrenomes do meio: {middleNamesError}";
        }

        return null;
    }

    /// <summary>
    /// Combines first name, middle names, and last name into a full name
    /// </summary>
    /// <param name="firstName">First name</param>
    /// <param name="middleNames">Middle names (optional)</param>
    /// <param name="lastName">Last name</param>
    /// <returns>Full name</returns>
    public static string CombineFullName(string firstName, string? middleNames, string lastName)
    {
        var parts = new List<string> { firstName.Trim() };

        if (!string.IsNullOrWhiteSpace(middleNames))
        {
            parts.Add(middleNames.Trim());
        }

        parts.Add(lastName.Trim());

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Extracts first name from a full name
    /// </summary>
    /// <param name="fullName">Full name</param>
    /// <returns>First name</returns>
    public static string ExtractFirstName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[0] : string.Empty;
    }

    /// <summary>
    /// Extracts last name from a full name
    /// </summary>
    /// <param name="fullName">Full name</param>
    /// <returns>Last name</returns>
    public static string ExtractLastName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 1 ? parts[^1] : string.Empty;
    }

    /// <summary>
    /// Extracts middle name from a full name
    /// </summary>
    /// <param name="fullName">Full name</param>
    /// <returns>Middle name or empty string if none</returns>
    public static string ExtractMiddleName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length <= 2)
            return string.Empty;

        var middleParts = parts.Skip(1).Take(parts.Length - 2);
        return string.Join(" ", middleParts);
    }
}
