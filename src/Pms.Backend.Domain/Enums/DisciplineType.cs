namespace Pms.Backend.Domain.Enums;

/// <summary>
/// Tipos de disciplina eclesiástica
/// </summary>
public enum DisciplineType
{
    /// <summary>
    /// Censura - disciplina temporária que não invalida batismo
    /// </summary>
    Censure = 1,

    /// <summary>
    /// Remoção - disciplina que invalida batismo
    /// </summary>
    Removal = 2
}
