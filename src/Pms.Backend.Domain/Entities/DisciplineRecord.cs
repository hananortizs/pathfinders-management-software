using Pms.Backend.Domain.Common;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Representa um registro de disciplina eclesiástica de um membro
/// </summary>
public class DisciplineRecord : BaseEntity
{
    /// <summary>
    /// ID do membro associado
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Tipo de disciplina (Censure, Removal)
    /// </summary>
    public DisciplineType Type { get; set; }

    /// <summary>
    /// Data de início da disciplina
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Data de fim da disciplina (opcional para Removal)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// ID da igreja que aplicou a disciplina (opcional)
    /// </summary>
    public Guid? ChurchId { get; set; }

    /// <summary>
    /// Local da disciplina em texto livre (quando ChurchId não disponível)
    /// </summary>
    public string? PlaceText { get; set; }

    /// <summary>
    /// Observações sobre a disciplina
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Membro relacionado ao registro de disciplina
    /// </summary>
    public Member Member { get; set; } = null!;

    /// <summary>
    /// Igreja onde ocorreu a disciplina (opcional)
    /// </summary>
    public Church? Church { get; set; }

    /// <summary>
    /// Verifica se a disciplina está ativa em uma data específica
    /// </summary>
    /// <param name="date">Data para verificação</param>
    /// <returns>True se ativa, false caso contrário</returns>
    public bool IsActiveAt(DateTime date)
    {
        return !IsDeleted &&
               StartDate <= date &&
               (EndDate == null || EndDate >= date);
    }

    /// <summary>
    /// Verifica se a disciplina está ativa atualmente
    /// </summary>
    /// <returns>True se ativa, false caso contrário</returns>
    public bool IsCurrentlyActive()
    {
        return IsActiveAt(DateTime.UtcNow);
    }

    /// <summary>
    /// Valida se o registro de disciplina é válido
    /// </summary>
    /// <returns>True se válido, false caso contrário</returns>
    public bool IsValid()
    {
        return StartDate <= DateTime.UtcNow &&
               (EndDate == null || EndDate >= StartDate) &&
               (ChurchId.HasValue || !string.IsNullOrWhiteSpace(PlaceText));
    }

    /// <summary>
    /// Obtém o local da disciplina (igreja ou texto)
    /// </summary>
    /// <returns>Nome da igreja ou texto do local</returns>
    public string GetDisciplinePlace()
    {
        return Church?.Name ?? PlaceText ?? "Local não informado";
    }

    /// <summary>
    /// Obtém a duração da disciplina
    /// </summary>
    /// <returns>Duração em dias ou null se ainda ativa</returns>
    public int? GetDurationInDays()
    {
        if (EndDate.HasValue)
        {
            return (int)(EndDate.Value - StartDate).TotalDays;
        }
        return null;
    }
}
