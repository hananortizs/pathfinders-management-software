using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Membership;

/// <summary>
/// DTO para resultado de alocação de membro
/// </summary>
public class AllocationResultDto
{
    /// <summary>
    /// Indica se a alocação foi bem-sucedida
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensagem do resultado
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Membership atualizada (se alocação automática foi bem-sucedida)
    /// </summary>
    public MembershipDto? Membership { get; set; }

    /// <summary>
    /// Lista de unidades compatíveis (se houver múltiplas opções)
    /// </summary>
    public List<CompatibleUnitDto> CompatibleUnits { get; set; } = new List<CompatibleUnitDto>();

    /// <summary>
    /// Indica se é necessário criar uma tarefa
    /// </summary>
    public bool RequiresTask { get; set; }

    /// <summary>
    /// Tipo de tarefa necessária
    /// </summary>
    public string? TaskType { get; set; }

    /// <summary>
    /// Descrição da tarefa
    /// </summary>
    public string? TaskDescription { get; set; }
}

/// <summary>
/// DTO para unidade compatível com um membro
/// </summary>
public class CompatibleUnitDto
{
    /// <summary>
    /// ID da unidade
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome da unidade
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da unidade
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gênero da unidade
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Idade mínima da unidade
    /// </summary>
    public int AgeMin { get; set; }

    /// <summary>
    /// Idade máxima da unidade
    /// </summary>
    public int AgeMax { get; set; }

    /// <summary>
    /// Capacidade da unidade (null = ilimitada)
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Número atual de membros
    /// </summary>
    public int CurrentMemberCount { get; set; }

    /// <summary>
    /// Indica se tem capacidade disponível
    /// </summary>
    public bool HasAvailableCapacity { get; set; }

    /// <summary>
    /// Percentual de ocupação (0-100)
    /// </summary>
    public double OccupancyPercentage { get; set; }

    /// <summary>
    /// Prioridade para alocação (menor = maior prioridade)
    /// </summary>
    public int Priority { get; set; }
}

/// <summary>
/// DTO para resultado de verificação de realocação por aniversário
/// </summary>
public class ReallocationCheckResultDto
{
    /// <summary>
    /// Indica se o membro precisa ser realocado
    /// </summary>
    public bool NeedsReallocation { get; set; }

    /// <summary>
    /// Motivo da necessidade de realocação
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Unidade atual do membro
    /// </summary>
    public CompatibleUnitDto? CurrentUnit { get; set; }

    /// <summary>
    /// Unidades compatíveis para realocação
    /// </summary>
    public List<CompatibleUnitDto> CompatibleUnits { get; set; } = new List<CompatibleUnitDto>();

    /// <summary>
    /// Indica se é necessário criar uma tarefa
    /// </summary>
    public bool RequiresTask { get; set; }

    /// <summary>
    /// Tipo de tarefa necessária
    /// </summary>
    public string? TaskType { get; set; }

    /// <summary>
    /// Descrição da tarefa
    /// </summary>
    public string? TaskDescription { get; set; }
}
