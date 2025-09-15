namespace Pms.Backend.Domain.Helpers;

/// <summary>
/// Classe auxiliar para cálculos de idade e validações relacionadas.
/// </summary>
public static class AgeHelper
{
    /// <summary>
    /// Calcula a idade baseada na data de nascimento.
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro.</param>
    /// <param name="referenceDate">Data de referência (padrão: hoje).</param>
    /// <returns>Idade em anos completos.</returns>
    public static int CalculateAge(DateTime dateOfBirth, DateTime? referenceDate = null)
    {
        var refDate = referenceDate ?? DateTime.Today;
        var age = refDate.Year - dateOfBirth.Year;

        // Ajusta se o aniversário ainda não aconteceu este ano
        if (refDate.Month < dateOfBirth.Month ||
            (refDate.Month == dateOfBirth.Month && refDate.Day < dateOfBirth.Day))
        {
            age--;
        }

        return age;
    }

    /// <summary>
    /// Calcula a idade baseada na data de nascimento usando 1º de junho como referência.
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro.</param>
    /// <param name="year">Ano de referência (padrão: ano atual).</param>
    /// <returns>Idade em anos completos.</returns>
    public static int CalculateAgeWithJuneReference(DateTime dateOfBirth, int? year = null)
    {
        var referenceYear = year ?? DateTime.Today.Year;
        var referenceDate = new DateTime(referenceYear, 6, 1); // 1º de junho

        return CalculateAge(dateOfBirth, referenceDate);
    }

    /// <summary>
    /// Verifica se a pessoa é maior de idade (18 anos ou mais).
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro.</param>
    /// <param name="referenceDate">Data de referência (padrão: hoje).</param>
    /// <returns>True se for maior de idade, False caso contrário.</returns>
    public static bool IsAdult(DateTime dateOfBirth, DateTime? referenceDate = null)
    {
        return CalculateAge(dateOfBirth, referenceDate) >= 18;
    }

    /// <summary>
    /// Verifica se a pessoa tem idade para batismo obrigatório (16 anos ou mais).
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro.</param>
    /// <param name="year">Ano de referência (padrão: ano atual).</param>
    /// <returns>True se precisa de batismo para ativação, False caso contrário.</returns>
    public static bool RequiresBaptism(DateTime dateOfBirth, int? year = null)
    {
        return CalculateAgeWithJuneReference(dateOfBirth, year) >= 16;
    }

    /// <summary>
    /// Verifica se a pessoa é menor de idade (menos de 18 anos).
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro.</param>
    /// <param name="referenceDate">Data de referência (padrão: hoje).</param>
    /// <returns>True se for menor de idade, False caso contrário.</returns>
    public static bool IsMinor(DateTime dateOfBirth, DateTime? referenceDate = null)
    {
        return !IsAdult(dateOfBirth, referenceDate);
    }
}
