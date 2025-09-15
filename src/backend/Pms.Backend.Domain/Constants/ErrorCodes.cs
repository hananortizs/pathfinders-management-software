namespace Pms.Backend.Domain.Constants;

/// <summary>
/// Constantes para códigos de erro do sistema
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Códigos de erro de autenticação e autorização
    /// </summary>
    public static class Auth
    {
        /// <summary>
        /// Membro com status Pending tentando operação de domínio
        /// </summary>
        public const string MemberPending = "MemberPending";

        /// <summary>
        /// Progresso sem lenço (investidura)
        /// </summary>
        public const string ScarfRequired = "ScarfRequired";

        /// <summary>
        /// Liderança sem batismo válido/lenço/sem censura
        /// </summary>
        public const string RequisitosEspirituaisNaoAtendidos = "RequisitosEspirituaisNaoAtendidos";
    }

    /// <summary>
    /// Códigos de erro de validação
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// CPF já cadastrado
        /// </summary>
        public const string CpfAlreadyExists = "CpfAlreadyExists";

        /// <summary>
        /// Email já cadastrado
        /// </summary>
        public const string EmailAlreadyExists = "EmailAlreadyExists";

        /// <summary>
        /// Idade mínima não atendida
        /// </summary>
        public const string MinimumAgeNotMet = "MinimumAgeNotMet";
    }

    /// <summary>
    /// Códigos de erro de negócio
    /// </summary>
    public static class Business
    {
        /// <summary>
        /// Clube inativo
        /// </summary>
        public const string ClubInactive = "ClubInactive";

        /// <summary>
        /// Unidade inativa
        /// </summary>
        public const string UnitInactive = "UnitInactive";

        /// <summary>
        /// Capacidade da unidade excedida
        /// </summary>
        public const string UnitCapacityExceeded = "UnitCapacityExceeded";
    }
}
