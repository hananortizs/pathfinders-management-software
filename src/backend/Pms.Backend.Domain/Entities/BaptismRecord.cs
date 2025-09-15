using Pms.Backend.Domain.Common;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Representa um registro de batismo ou rebatismo de um membro
/// </summary>
public class BaptismRecord : BaseEntity
{
    /// <summary>
    /// ID do membro associado
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Tipo de batismo (Baptism, ReBaptism)
    /// </summary>
    public BaptismType Type { get; set; }

    /// <summary>
    /// Data do batismo
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// ID da igreja onde ocorreu o batismo (opcional)
    /// </summary>
    public Guid? ChurchId { get; set; }

    /// <summary>
    /// Local do batismo em texto livre (quando ChurchId não disponível)
    /// </summary>
    public string? PlaceText { get; set; }

    /// <summary>
    /// Nome do pastor que realizou o batismo (opcional)
    /// </summary>
    public string? PastorText { get; set; }

    /// <summary>
    /// URL de evidência do batismo (opcional)
    /// </summary>
    public string? EvidenceUrl { get; set; }

    /// <summary>
    /// Membro relacionado ao registro de batismo
    /// </summary>
    public Member Member { get; set; } = null!;

    /// <summary>
    /// Igreja onde ocorreu o batismo (opcional)
    /// </summary>
    public Church? Church { get; set; }

    /// <summary>
    /// Valida se o registro de batismo é válido
    /// </summary>
    /// <returns>True se válido, false caso contrário</returns>
    public bool IsValid()
    {
        return Date <= DateTime.UtcNow &&
               (ChurchId.HasValue || !string.IsNullOrWhiteSpace(PlaceText));
    }

    /// <summary>
    /// Obtém o local do batismo (igreja ou texto)
    /// </summary>
    /// <returns>Nome da igreja ou texto do local</returns>
    public string GetBaptismPlace()
    {
        return Church?.Name ?? PlaceText ?? "Local não informado";
    }

    /// <summary>
    /// Obtém informações do pastor
    /// </summary>
    /// <returns>Nome do pastor ou texto informado</returns>
    public string GetPastorInfo()
    {
        return PastorText ?? "Pastor não informado";
    }
}
