namespace Pms.Backend.Application.DTOs.Validation;

/// <summary>
/// Informações sobre a investidura de lenço do membro
/// </summary>
public class ScarfInvestitureInfo
{
    /// <summary>
    /// Indica se o membro possui lenço investido
    /// </summary>
    public bool IsInvested { get; set; }

    /// <summary>
    /// Data da investidura
    /// </summary>
    public DateTime? InvestedAt { get; set; }

    /// <summary>
    /// Igreja onde foi investido
    /// </summary>
    public string? Church { get; set; }

    /// <summary>
    /// Pastor que realizou a investidura
    /// </summary>
    public string? Pastor { get; set; }

    /// <summary>
    /// Data da investidura (campo adicional)
    /// </summary>
    public DateTime? Date { get; set; }
}
