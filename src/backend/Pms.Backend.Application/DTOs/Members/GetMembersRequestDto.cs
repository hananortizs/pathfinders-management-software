using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Members
{
    /// <summary>
    /// DTO para requisição de listagem de membros
    /// </summary>
    public class GetMembersRequestDto
    {
        /// <summary>
        /// Nível do usuário (Admin, Director, Secretary, etc.)
        /// </summary>
        [Required]
        public string UserLevel { get; set; } = string.Empty;

        /// <summary>
        /// Estratégia de agrupamento (hierarchical, flat, by_club, by_unit)
        /// </summary>
        [Required]
        public string GroupingStrategy { get; set; } = "hierarchical";

        /// <summary>
        /// Página atual (baseado em 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Tamanho da página (máximo 100)
        /// </summary>
        [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Campo para ordenação (name, age, role, created_at, club_name)
        /// </summary>
        public string SortBy { get; set; } = "name";

        /// <summary>
        /// Direção da ordenação (asc, desc)
        /// </summary>
        public string SortOrder { get; set; } = "asc";

        /// <summary>
        /// Filtros aplicados
        /// </summary>
        public MemberFiltersDto? Filters { get; set; }
    }

    /// <summary>
    /// DTO para filtros de membros
    /// </summary>
    public class MemberFiltersDto
    {
        /// <summary>
        /// Busca textual (nome, CPF, email)
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Status do membro
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gênero
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// Idade mínima
        /// </summary>
        public int? MinAge { get; set; }

        /// <summary>
        /// Idade máxima
        /// </summary>
        public int? MaxAge { get; set; }

        /// <summary>
        /// ID da divisão
        /// </summary>
        public Guid? DivisionId { get; set; }

        /// <summary>
        /// ID da união
        /// </summary>
        public Guid? UnionId { get; set; }

        /// <summary>
        /// ID da região
        /// </summary>
        public Guid? RegionId { get; set; }

        /// <summary>
        /// ID da associação
        /// </summary>
        public Guid? AssociationId { get; set; }

        /// <summary>
        /// ID do distrito
        /// </summary>
        public Guid? DistrictId { get; set; }

        /// <summary>
        /// ID do clube
        /// </summary>
        public Guid? ClubId { get; set; }

        /// <summary>
        /// ID da unidade
        /// </summary>
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Cargo específico
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Data de criação inicial
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Data de criação final
        /// </summary>
        public DateTime? CreatedTo { get; set; }
    }
}
