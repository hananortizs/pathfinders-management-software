using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Helper class for date of birth validation and operations
/// </summary>
public static class DateOfBirthHelper
{
    /// <summary>
    /// Minimum age required for membership (10 years old by June 1st of current year)
    /// </summary>
    private const int MinimumAge = 10;

    /// <summary>
    /// Maximum age allowed for membership (120 years old)
    /// </summary>
    private const int MaximumAge = 120;

    /// <summary>
    /// Validates if the date of birth meets the minimum age requirement
    /// Member must be at least 10 years old by June 1st of the current year
    /// </summary>
    /// <param name="dateOfBirth">Date of birth to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidDateOfBirth(DateTime dateOfBirth)
    {
        var currentYear = DateTime.Now.Year;
        var juneFirst = new DateTime(currentYear, 6, 1);
        var minimumBirthDate = juneFirst.AddYears(-MinimumAge);
        var maximumBirthDate = juneFirst.AddYears(-MaximumAge);

        return dateOfBirth >= maximumBirthDate && dateOfBirth <= minimumBirthDate;
    }

    /// <summary>
    /// Gets the validation error message for date of birth
    /// </summary>
    /// <param name="dateOfBirth">Date of birth to validate</param>
    /// <returns>Error message or null if valid</returns>
    public static string? GetValidationError(DateTime dateOfBirth)
    {
        var currentYear = DateTime.Now.Year;
        var juneFirst = new DateTime(currentYear, 6, 1);
        var minimumBirthDate = juneFirst.AddYears(-MinimumAge);
        var maximumBirthDate = juneFirst.AddYears(-MaximumAge);

        if (dateOfBirth > minimumBirthDate)
        {
            return $"Membro deve ter pelo menos {MinimumAge} anos completos até 1º de junho de {currentYear}";
        }

        if (dateOfBirth < maximumBirthDate)
        {
            return $"Data de nascimento não pode ser anterior a {maximumBirthDate:dd/MM/yyyy}";
        }

        return null;
    }

    /// <summary>
    /// Calculates the age as of June 1st of the current year
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns>Age as of June 1st of current year</returns>
    public static int CalculateAge(DateTime dateOfBirth)
    {
        var currentYear = DateTime.Now.Year;
        var juneFirst = new DateTime(currentYear, 6, 1);
        
        var age = juneFirst.Year - dateOfBirth.Year;
        
        if (juneFirst.DayOfYear < dateOfBirth.DayOfYear)
        {
            age--;
        }
        
        return age;
    }

    /// <summary>
    /// Calculates the age as of a specific date
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <param name="asOfDate">Date to calculate age as of</param>
    /// <returns>Age as of the specific date</returns>
    public static int CalculateAge(DateTime dateOfBirth, DateTime asOfDate)
    {
        var age = asOfDate.Year - dateOfBirth.Year;
        
        if (asOfDate.DayOfYear < dateOfBirth.DayOfYear)
        {
            age--;
        }
        
        return age;
    }

    /// <summary>
    /// Gets the minimum birth date for membership eligibility
    /// </summary>
    /// <returns>Minimum birth date</returns>
    public static DateTime GetMinimumBirthDate()
    {
        var currentYear = DateTime.Now.Year;
        var juneFirst = new DateTime(currentYear, 6, 1);
        return juneFirst.AddYears(-MinimumAge);
    }

    /// <summary>
    /// Gets the maximum birth date for membership eligibility
    /// </summary>
    /// <returns>Maximum birth date</returns>
    public static DateTime GetMaximumBirthDate()
    {
        var currentYear = DateTime.Now.Year;
        var juneFirst = new DateTime(currentYear, 6, 1);
        return juneFirst.AddYears(-MaximumAge);
    }

    /// <summary>
    /// Checks if the member will be eligible for membership by June 1st of current year
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns>True if will be eligible, false otherwise</returns>
    public static bool WillBeEligibleByJuneFirst(DateTime dateOfBirth)
    {
        var currentYear = DateTime.Now.Year;
        var juneFirst = new DateTime(currentYear, 6, 1);
        var minimumBirthDate = juneFirst.AddYears(-MinimumAge);
        
        return dateOfBirth <= minimumBirthDate;
    }

    /// <summary>
    /// Gets the next eligibility date for a member who is not yet eligible
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns>Next eligibility date or null if already eligible</returns>
    public static DateTime? GetNextEligibilityDate(DateTime dateOfBirth)
    {
        if (IsValidDateOfBirth(dateOfBirth))
            return null;

        var currentYear = DateTime.Now.Year;
        var juneFirst = new DateTime(currentYear, 6, 1);
        var minimumBirthDate = juneFirst.AddYears(-MinimumAge);

        if (dateOfBirth > minimumBirthDate)
        {
            // Member will be eligible on June 1st of the year they turn 10
            var eligibilityYear = dateOfBirth.Year + MinimumAge;
            return new DateTime(eligibilityYear, 6, 1);
        }

        return null;
    }
}
