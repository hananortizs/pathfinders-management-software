using Pms.Backend.Domain.Common;
using Pms.Backend.Domain.Entities.Hierarchy;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Representa um Pastor no sistema
/// Um pastor pode ser pastor de um ou mais distritos
/// </summary>
public class Pastor : BaseEntity
{
    /// <summary>
    /// Nome do pastor
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Nome completo do pastor (computed property)
    /// </summary>
    public string FullName => NameHelper.NormalizeName(Name) ?? Name;

    /// <summary>
    /// Email do pastor (deve ser único)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Telefone do pastor
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Data de ordenação do pastor
    /// </summary>
    public DateTime? OrdinationDate { get; set; }

    /// <summary>
    /// Igreja onde foi ordenado
    /// </summary>
    public string? OrdinationChurch { get; set; }

    /// <summary>
    /// Observações sobre o pastor
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Indica se o pastor está ativo
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Data de início do pastorado
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data de fim do pastorado (se aplicável)
    /// </summary>
    public DateTime? EndDate { get; set; }

    // Navegação
    /// <summary>
    /// Distritos onde o pastor atua
    /// </summary>
    public ICollection<District> Districts { get; set; } = new List<District>();

    /// <summary>
    /// Valida se o pastor está ativo em uma data específica
    /// </summary>
    /// <param name="date">Data para verificação</param>
    /// <returns>True se ativo, false caso contrário</returns>
    public bool IsActiveAt(DateTime date)
    {
        return IsActive &&
               (StartDate == null || StartDate <= date) &&
               (EndDate == null || EndDate >= date);
    }

    /// <summary>
    /// Valida se o pastor está ativo atualmente
    /// </summary>
    /// <returns>True se ativo, false caso contrário</returns>
    public bool IsCurrentlyActive()
    {
        return IsActiveAt(DateTime.UtcNow);
    }

    /// <summary>
    /// Obtém o email formatado para exibição
    /// </summary>
    public string? EmailFormatted => EmailHelper.NormalizeEmail(Email);

    /// <summary>
    /// Obtém o telefone formatado para exibição
    /// </summary>
    public string? PhoneFormatted => PhoneHelper.FormatPhoneForDisplay(Phone);

    /// <summary>
    /// Valida se os dados do pastor são válidos
    /// </summary>
    /// <returns>True se válido, false caso contrário</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(Email) &&
               EmailHelper.IsValidEmail(Email) &&
               (Phone == null || PhoneHelper.IsValidPhone(Phone)) &&
               (StartDate == null || StartDate <= DateTime.UtcNow) &&
               (EndDate == null || EndDate >= StartDate);
    }

    /// <summary>
    /// Obtém a duração do pastorado em dias
    /// </summary>
    /// <returns>Duração em dias ou null se ainda ativo</returns>
    public int? GetPastorateDurationInDays()
    {
        if (StartDate.HasValue)
        {
            var endDate = EndDate ?? DateTime.UtcNow;
            return (int)(endDate - StartDate.Value).TotalDays;
        }
        return null;
    }
}
