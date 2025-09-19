namespace Pms.Backend.Application.DTOs.Members
{
    /// <summary>
    /// DTO para resposta da listagem de membros
    /// </summary>
    public class MemberListResponseDto
    {
        /// <summary>
        /// Lista de membros
        /// </summary>
        public List<MemberSummaryDto> Members { get; set; } = new();

        /// <summary>
        /// Grupos hierárquicos (quando groupingStrategy = hierarchical)
        /// </summary>
        public List<MemberGroupDto> Groups { get; set; } = new();

        /// <summary>
        /// Total de membros encontrados
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Página atual
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Tamanho da página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Indica se há próxima página
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Indica se há página anterior
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Estatísticas resumidas
        /// </summary>
        public MemberListStatsDto Stats { get; set; } = new();
    }

    /// <summary>
    /// DTO para resumo de membro na listagem
    /// </summary>
    public class MemberSummaryDto
    {
        /// <summary>
        /// ID do membro
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome completo
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Nome de exibição (pode incluir nome social)
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Idade
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gênero
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Status do membro
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Nome do clube
        /// </summary>
        public string ClubName { get; set; } = string.Empty;

        /// <summary>
        /// Nome da unidade
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// Cargo atual (se houver)
        /// </summary>
        public string? CurrentRole { get; set; }

        /// <summary>
        /// Todas as roles ativas do membro, ordenadas por nível hierárquico (maior para menor)
        /// </summary>
        public string? AllRoles { get; set; }

        /// <summary>
        /// Email principal
        /// </summary>
        public string? PrimaryEmail { get; set; }

        /// <summary>
        /// Telefone principal
        /// </summary>
        public string? PrimaryPhone { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Indica se tem investidura de lenço
        /// </summary>
        public bool HasScarfInvestiture { get; set; }

        /// <summary>
        /// Indica se tem batismo válido
        /// </summary>
        public bool HasValidBaptism { get; set; }
    }

    /// <summary>
    /// DTO para grupo hierárquico de membros
    /// </summary>
    public class MemberGroupDto
    {
        /// <summary>
        /// ID do grupo
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome do grupo
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do grupo (Division, Union, Region, etc.)
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Código do grupo
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Caminho hierárquico completo
        /// </summary>
        public string CodePath { get; set; } = string.Empty;

        /// <summary>
        /// Quantidade de membros no grupo
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// Subgrupos (quando aplicável)
        /// </summary>
        public List<MemberGroupDto> SubGroups { get; set; } = new();

        /// <summary>
        /// Membros diretos do grupo (quando não há subgrupos)
        /// </summary>
        public List<MemberSummaryDto> DirectMembers { get; set; } = new();

        /// <summary>
        /// Indica se o grupo está expandido na UI
        /// </summary>
        public bool IsExpanded { get; set; } = false;
    }

    /// <summary>
    /// DTO para estatísticas da listagem
    /// </summary>
    public class MemberListStatsDto
    {
        /// <summary>
        /// Total de membros ativos
        /// </summary>
        public int ActiveCount { get; set; }

        /// <summary>
        /// Total de membros pendentes
        /// </summary>
        public int PendingCount { get; set; }

        /// <summary>
        /// Total de membros inativos
        /// </summary>
        public int InactiveCount { get; set; }

        /// <summary>
        /// Total de membros suspensos
        /// </summary>
        public int SuspendedCount { get; set; }

        /// <summary>
        /// Distribuição por gênero
        /// </summary>
        public Dictionary<string, int> GenderDistribution { get; set; } = new();

        /// <summary>
        /// Distribuição por faixa etária
        /// </summary>
        public Dictionary<string, int> AgeDistribution { get; set; } = new();

        /// <summary>
        /// Distribuição por clube
        /// </summary>
        public Dictionary<string, int> ClubDistribution { get; set; } = new();
    }
}
