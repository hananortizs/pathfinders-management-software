namespace Pms.Backend.Application.DTOs;

/// <summary>
/// DTO para relatório de membros do clube
/// </summary>
public class ClubMembersReportDto
{
    /// <summary>
    /// ID do clube
    /// </summary>
    public Guid ClubId { get; set; }
    
    /// <summary>
    /// Nome do clube
    /// </summary>
    public string ClubName { get; set; } = string.Empty;
    
    /// <summary>
    /// Data e hora de geração do relatório
    /// </summary>
    public DateTime GeneratedAt { get; set; }
    
    /// <summary>
    /// Total de membros do clube
    /// </summary>
    public int TotalMembers { get; set; }
    
    /// <summary>
    /// Número de membros ativos
    /// </summary>
    public int ActiveMembers { get; set; }
    
    /// <summary>
    /// Número de membros inativos
    /// </summary>
    public int InactiveMembers { get; set; }
    
    /// <summary>
    /// Número de membros do sexo masculino
    /// </summary>
    public int MaleMembers { get; set; }
    
    /// <summary>
    /// Número de membros do sexo feminino
    /// </summary>
    public int FemaleMembers { get; set; }
    
    /// <summary>
    /// Distribuição de membros por faixa etária
    /// </summary>
    public Dictionary<string, int> AgeGroups { get; set; } = new();
    
    /// <summary>
    /// Lista detalhada de membros
    /// </summary>
    public List<MemberReportItemDto> Members { get; set; } = new();
}

/// <summary>
/// DTO para relatório de capacidade do clube
/// </summary>
public class ClubCapacityReportDto
{
    /// <summary>
    /// ID do clube
    /// </summary>
    public Guid ClubId { get; set; }
    
    /// <summary>
    /// Nome do clube
    /// </summary>
    public string ClubName { get; set; } = string.Empty;
    
    /// <summary>
    /// Data e hora de geração do relatório
    /// </summary>
    public DateTime GeneratedAt { get; set; }
    
    /// <summary>
    /// Total de unidades do clube
    /// </summary>
    public int TotalUnits { get; set; }
    
    /// <summary>
    /// Capacidade total de todas as unidades
    /// </summary>
    public int TotalCapacity { get; set; }
    
    /// <summary>
    /// Total de membros alocados
    /// </summary>
    public int TotalMembers { get; set; }
    
    /// <summary>
    /// Total de vagas disponíveis
    /// </summary>
    public int TotalAvailableSlots { get; set; }
    
    /// <summary>
    /// Percentual médio de utilização das unidades
    /// </summary>
    public double AverageUtilization { get; set; }
    
    /// <summary>
    /// Lista de unidades com suas capacidades
    /// </summary>
    public List<UnitCapacityReportDto> Units { get; set; } = new();
}

/// <summary>
/// DTO para relatório de faixas etárias
/// </summary>
public class AgeGroupReportDto
{
    /// <summary>
    /// ID do clube
    /// </summary>
    public Guid ClubId { get; set; }
    
    /// <summary>
    /// Nome do clube
    /// </summary>
    public string ClubName { get; set; } = string.Empty;
    
    /// <summary>
    /// Data e hora de geração do relatório
    /// </summary>
    public DateTime GeneratedAt { get; set; }
    
    /// <summary>
    /// Total de membros do clube
    /// </summary>
    public int TotalMembers { get; set; }
    
    /// <summary>
    /// Distribuição de membros por faixa etária
    /// </summary>
    public Dictionary<string, AgeGroupDetailDto> AgeGroups { get; set; } = new();
}

/// <summary>
/// DTO para relatório de status dos membros
/// </summary>
public class MemberStatusReportDto
{
    /// <summary>
    /// ID do clube
    /// </summary>
    public Guid ClubId { get; set; }
    
    /// <summary>
    /// Nome do clube
    /// </summary>
    public string ClubName { get; set; } = string.Empty;
    
    /// <summary>
    /// Data e hora de geração do relatório
    /// </summary>
    public DateTime GeneratedAt { get; set; }
    
    /// <summary>
    /// Total de membros do clube
    /// </summary>
    public int TotalMembers { get; set; }
    
    /// <summary>
    /// Número de membros ativos
    /// </summary>
    public int ActiveMembers { get; set; }
    
    /// <summary>
    /// Número de membros inativos
    /// </summary>
    public int InactiveMembers { get; set; }
    
    /// <summary>
    /// Percentual de membros ativos
    /// </summary>
    public double ActivePercentage { get; set; }
    
    /// <summary>
    /// Percentual de membros inativos
    /// </summary>
    public double InactivePercentage { get; set; }
    
    /// <summary>
    /// Lista de membros ativos
    /// </summary>
    public List<MemberReportItemDto> ActiveMembersList { get; set; } = new();
    
    /// <summary>
    /// Lista de membros inativos
    /// </summary>
    public List<MemberReportItemDto> InactiveMembersList { get; set; } = new();
}

/// <summary>
/// DTO para item de membro em relatórios
/// </summary>
public class MemberReportItemDto
{
    /// <summary>
    /// ID único do membro
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Primeiro nome do membro
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Sobrenome do membro
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome social do membro (opcional)
    /// </summary>
    public string? SocialName { get; set; }
    
    /// <summary>
    /// Data de nascimento do membro
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// Gênero do membro
    /// </summary>
    public string Gender { get; set; } = string.Empty;
    
    /// <summary>
    /// Status do membro (Ativo/Inativo)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// CPF do membro
    /// </summary>
    public string? Cpf { get; set; }
    
    /// <summary>
    /// RG do membro
    /// </summary>
    public string? Rg { get; set; }
    
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO para capacidade de unidade em relatórios
/// </summary>
public class UnitCapacityReportDto
{
    /// <summary>
    /// ID da unidade
    /// </summary>
    public Guid UnitId { get; set; }
    
    /// <summary>
    /// Nome da unidade
    /// </summary>
    public string UnitName { get; set; } = string.Empty;
    
    /// <summary>
    /// Tipo da unidade
    /// </summary>
    public string UnitType { get; set; } = string.Empty;
    
    /// <summary>
    /// Capacidade total da unidade
    /// </summary>
    public int Capacity { get; set; }
    
    /// <summary>
    /// Número atual de membros na unidade
    /// </summary>
    public int CurrentMembers { get; set; }
    
    /// <summary>
    /// Número de vagas disponíveis
    /// </summary>
    public int AvailableSlots { get; set; }
    
    /// <summary>
    /// Percentual de utilização da unidade
    /// </summary>
    public double UtilizationPercentage { get; set; }
    
    /// <summary>
    /// Indica se a unidade está lotada
    /// </summary>
    public bool IsFull { get; set; }
    
    /// <summary>
    /// Indica se a unidade está próxima da capacidade máxima
    /// </summary>
    public bool IsNearCapacity { get; set; }
}

/// <summary>
/// DTO para detalhes de faixa etária
/// </summary>
public class AgeGroupDetailDto
{
    /// <summary>
    /// Idade mínima da faixa etária
    /// </summary>
    public int MinAge { get; set; }
    
    /// <summary>
    /// Idade máxima da faixa etária
    /// </summary>
    public int MaxAge { get; set; }
    
    /// <summary>
    /// Total de membros nesta faixa etária
    /// </summary>
    public int Count { get; set; }
    
    /// <summary>
    /// Número de membros do sexo masculino nesta faixa
    /// </summary>
    public int MaleCount { get; set; }
    
    /// <summary>
    /// Número de membros do sexo feminino nesta faixa
    /// </summary>
    public int FemaleCount { get; set; }
    
    /// <summary>
    /// Número de membros ativos nesta faixa
    /// </summary>
    public int ActiveCount { get; set; }
    
    /// <summary>
    /// Número de membros inativos nesta faixa
    /// </summary>
    public int InactiveCount { get; set; }
    
    /// <summary>
    /// Lista de membros desta faixa etária
    /// </summary>
    public List<MemberReportItemDto> Members { get; set; } = new();
}
