using System.Text.Json.Serialization;
using Pms.Backend.Domain.Enums;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Representa um contato no sistema
/// Pode ser associado a qualquer entidade (Member, Church, etc.) através de relacionamentos polimórficos
/// </summary>
public class Contact : BaseEntity, IComparable<Contact>
{
    /// <summary>
    /// Tipo do contato (telefone, email, rede social, etc.)
    /// </summary>
    public ContactType Type { get; set; }

    /// <summary>
    /// Categoria do contato (pessoal, emergência, igreja, etc.)
    /// </summary>
    public ContactCategory Category { get; set; }

    /// <summary>
    /// Valor do contato (número de telefone, email, URL, etc.)
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Nome ou descrição do contato (ex: "Pastor João", "Mãe", "WhatsApp pessoal")
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Indica se este é o contato principal para esta categoria
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Indica se o contato está ativo
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Notas adicionais sobre o contato
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ID da entidade que possui este contato (relacionamento polimórfico)
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Tipo da entidade que possui este contato (relacionamento polimórfico)
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Ordem de prioridade do contato (para ordenação)
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Data da última verificação/atualização do contato
    /// </summary>
    public DateTime? LastVerifiedAt { get; set; }

    /// <summary>
    /// Indica se o contato foi verificado como válido
    /// </summary>
    public bool IsVerified { get; set; } = false;

    /// <summary>
    /// Obtém o valor formatado para exibição
    /// </summary>
    public string FormattedValue
    {
        get
        {
            return Type switch
            {
                ContactType.Mobile or ContactType.Landline or ContactType.WhatsApp
                    => PhoneHelper.FormatPhoneForDisplay(Value) ?? Value,
                ContactType.Email
                    => EmailHelper.NormalizeEmail(Value) ?? Value,
                ContactType.Website
                    => FormatWebsiteUrl(Value),
                _ => Value
            };
        }
    }

    /// <summary>
    /// Obtém o valor normalizado para armazenamento no banco
    /// </summary>
    [JsonIgnore]
    public string NormalizedValue
    {
        get
        {
            return Type switch
            {
                ContactType.Mobile or ContactType.Landline or ContactType.WhatsApp
                    => PhoneHelper.NormalizePhone(Value) ?? Value,
                ContactType.Email
                    => EmailHelper.NormalizeEmail(Value) ?? Value,
                ContactType.Website
                    => NormalizeWebsiteUrl(Value),
                _ => Value
            };
        }
    }

    /// <summary>
    /// Obtém o nome de exibição do tipo de contato
    /// </summary>
    public string TypeDisplayName => ContactTypeHelper.GetDisplayName(Type);

    /// <summary>
    /// Obtém o nome de exibição da categoria
    /// </summary>
    public string CategoryDisplayName => ContactTypeHelper.GetDisplayName(Category);

    /// <summary>
    /// Obtém o nome completo do contato (Label + Type)
    /// </summary>
    public string FullDisplayName
    {
        get
        {
            if (!string.IsNullOrEmpty(Label))
                return $"{Label} ({TypeDisplayName})";
            return TypeDisplayName;
        }
    }

    /// <summary>
    /// Verifica se o contato é uma rede social
    /// </summary>
    public bool IsSocialMedia => ContactTypeHelper.IsSocialMedia(Type);

    /// <summary>
    /// Verifica se o contato é telefônico
    /// </summary>
    public bool IsPhone => ContactTypeHelper.IsPhone(Type);

    /// <summary>
    /// Verifica se o contato é digital
    /// </summary>
    public bool IsDigital => ContactTypeHelper.IsDigital(Type);

    /// <summary>
    /// Verifica se o contato é de emergência
    /// </summary>
    public bool IsEmergency => Category == ContactCategory.Emergency;

    /// <summary>
    /// Verifica se o contato é pessoal
    /// </summary>
    public bool IsPersonal => Category == ContactCategory.Personal;

    /// <summary>
    /// Verifica se o contato é da igreja
    /// </summary>
    public bool IsChurch => Category == ContactCategory.Church;

    /// <summary>
    /// Verifica se o contato é de responsável legal
    /// </summary>
    public bool IsLegalGuardian => Category == ContactCategory.LegalGuardian;

    /// <summary>
    /// Formata URL de website para exibição
    /// </summary>
    /// <param name="url">URL a ser formatada</param>
    /// <returns>URL formatada</returns>
    private static string FormatWebsiteUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;

        // Remove protocolo se não tiver
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return $"https://{url}";
        }

        return url;
    }

    /// <summary>
    /// Normaliza URL de website para armazenamento
    /// </summary>
    /// <param name="url">URL a ser normalizada</param>
    /// <returns>URL normalizada</returns>
    private static string NormalizeWebsiteUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;

        // Remove protocolo para normalização
        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            return url.Substring(7);

        if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return url.Substring(8);

        return url;
    }

    #region Validation Methods

    /// <summary>
    /// Valida se o contato está em um formato válido
    /// </summary>
    /// <returns>True se válido, false caso contrário</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Value) &&
               !string.IsNullOrWhiteSpace(EntityType) &&
               EntityId != Guid.Empty &&
               ValidateValue();
    }

    /// <summary>
    /// Valida o valor do contato baseado no tipo
    /// </summary>
    /// <returns>True se válido, false caso contrário</returns>
    public bool ValidateValue()
    {
        return Type switch
        {
            ContactType.Mobile or ContactType.Landline or ContactType.WhatsApp
                => PhoneHelper.IsValidPhone(Value),
            ContactType.Email
                => EmailHelper.IsValidEmail(Value),
            ContactType.Website
                => IsValidWebsite(Value),
            ContactType.Facebook or ContactType.Instagram or ContactType.YouTube or
            ContactType.TikTok or ContactType.LinkedIn or ContactType.Twitter
                => IsValidSocialMedia(Value),
            _ => !string.IsNullOrWhiteSpace(Value)
        };
    }

    /// <summary>
    /// Obtém mensagens de validação para o contato
    /// </summary>
    /// <returns>Lista de mensagens de erro</returns>
    public List<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Value))
            errors.Add("Valor do contato é obrigatório");

        if (string.IsNullOrWhiteSpace(EntityType))
            errors.Add("Tipo da entidade é obrigatório");

        if (EntityId == Guid.Empty)
            errors.Add("ID da entidade é obrigatório");

        if (!ValidateValue())
        {
            var valueError = GetValueValidationError();
            if (!string.IsNullOrEmpty(valueError))
                errors.Add(valueError);
        }

        return errors;
    }

    /// <summary>
    /// Obtém mensagem de erro específica para o valor do contato
    /// </summary>
    /// <returns>Mensagem de erro ou null se válido</returns>
    public string? GetValueValidationError()
    {
        return Type switch
        {
            ContactType.Mobile or ContactType.Landline or ContactType.WhatsApp
                => PhoneHelper.GetValidationError(Value),
            ContactType.Email
                => EmailHelper.GetValidationError(Value),
            ContactType.Website
                => IsValidWebsite(Value) ? null : "URL do website deve estar em formato válido",
            ContactType.Facebook or ContactType.Instagram or ContactType.YouTube or
            ContactType.TikTok or ContactType.LinkedIn or ContactType.Twitter
                => IsValidSocialMedia(Value) ? null : "URL da rede social deve estar em formato válido",
            _ => null
        };
    }

    /// <summary>
    /// Valida se a URL do website está em formato válido
    /// </summary>
    /// <param name="url">URL a ser validada</param>
    /// <returns>True se válida, false caso contrário</returns>
    private static bool IsValidWebsite(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url.StartsWith("http") ? url : $"https://{url}",
            UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    /// <summary>
    /// Valida se a URL da rede social está em formato válido
    /// </summary>
    /// <param name="url">URL a ser validada</param>
    /// <returns>True se válida, false caso contrário</returns>
    private static bool IsValidSocialMedia(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url.StartsWith("http") ? url : $"https://{url}",
            UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Cria um novo contato de telefone celular
    /// </summary>
    /// <param name="phone">Número do telefone</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="label">Rótulo do contato (opcional)</param>
    /// <param name="category">Categoria do contato</param>
    /// <returns>Nova instância de Contact</returns>
    public static Contact CreateMobile(string phone, Guid entityId, string entityType,
        string? label = null, ContactCategory category = ContactCategory.Personal)
    {
        return new Contact
        {
            Type = ContactType.Mobile,
            Value = phone,
            EntityId = entityId,
            EntityType = entityType,
            Label = label,
            Category = category,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Cria um novo contato de email
    /// </summary>
    /// <param name="email">Endereço de email</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="label">Rótulo do contato (opcional)</param>
    /// <param name="category">Categoria do contato</param>
    /// <returns>Nova instância de Contact</returns>
    public static Contact CreateEmail(string email, Guid entityId, string entityType,
        string? label = null, ContactCategory category = ContactCategory.Personal)
    {
        return new Contact
        {
            Type = ContactType.Email,
            Value = email,
            EntityId = entityId,
            EntityType = entityType,
            Label = label,
            Category = category,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Cria um novo contato de WhatsApp
    /// </summary>
    /// <param name="phone">Número do telefone</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="label">Rótulo do contato (opcional)</param>
    /// <param name="category">Categoria do contato</param>
    /// <returns>Nova instância de Contact</returns>
    public static Contact CreateWhatsApp(string phone, Guid entityId, string entityType,
        string? label = null, ContactCategory category = ContactCategory.Personal)
    {
        return new Contact
        {
            Type = ContactType.WhatsApp,
            Value = phone,
            EntityId = entityId,
            EntityType = entityType,
            Label = label,
            Category = category,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Cria um novo contato de rede social
    /// </summary>
    /// <param name="socialType">Tipo da rede social</param>
    /// <param name="url">URL da rede social</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="label">Rótulo do contato (opcional)</param>
    /// <param name="category">Categoria do contato</param>
    /// <returns>Nova instância de Contact</returns>
    public static Contact CreateSocialMedia(ContactType socialType, string url, Guid entityId,
        string entityType, string? label = null, ContactCategory category = ContactCategory.Personal)
    {
        if (!ContactTypeHelper.IsSocialMedia(socialType))
            throw new ArgumentException("Tipo deve ser uma rede social", nameof(socialType));

        return new Contact
        {
            Type = socialType,
            Value = url,
            EntityId = entityId,
            EntityType = entityType,
            Label = label,
            Category = category,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Cria um novo contato de emergência
    /// </summary>
    /// <param name="contactType">Tipo do contato</param>
    /// <param name="value">Valor do contato</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="label">Rótulo do contato (opcional)</param>
    /// <returns>Nova instância de Contact</returns>
    public static Contact CreateEmergency(ContactType contactType, string value, Guid entityId,
        string entityType, string? label = null)
    {
        return new Contact
        {
            Type = contactType,
            Value = value,
            EntityId = entityId,
            EntityType = entityType,
            Label = label,
            Category = ContactCategory.Emergency,
            IsPrimary = true, // Contatos de emergência são sempre primários
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    #endregion

    #region Business Rules

    /// <summary>
    /// Marca o contato como verificado
    /// </summary>
    /// <param name="verifiedAt">Data da verificação (opcional, usa UTC agora se não informado)</param>
    public void MarkAsVerified(DateTime? verifiedAt = null)
    {
        IsVerified = true;
        LastVerifiedAt = verifiedAt ?? DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca o contato como não verificado
    /// </summary>
    public void MarkAsUnverified()
    {
        IsVerified = false;
        LastVerifiedAt = null;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Define o contato como principal para sua categoria
    /// </summary>
    public void SetAsPrimary()
    {
        IsPrimary = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Remove o status de principal do contato
    /// </summary>
    public void RemovePrimary()
    {
        IsPrimary = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Ativa o contato
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Desativa o contato
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o valor do contato e normaliza automaticamente
    /// </summary>
    /// <param name="newValue">Novo valor do contato</param>
    public void UpdateValue(string newValue)
    {
        Value = newValue;
        UpdatedAtUtc = DateTime.UtcNow;

        // Marca como não verificado quando o valor muda
        MarkAsUnverified();
    }

    /// <summary>
    /// Atualiza a prioridade do contato
    /// </summary>
    /// <param name="priority">Nova prioridade</param>
    public void UpdatePriority(int priority)
    {
        Priority = priority;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica se o contato pode ser definido como principal
    /// </summary>
    /// <returns>True se pode ser principal, false caso contrário</returns>
    public bool CanBePrimary()
    {
        return IsActive && IsValid() && !IsDeleted;
    }

    /// <summary>
    /// Verifica se o contato está pronto para uso
    /// </summary>
    /// <returns>True se está pronto, false caso contrário</returns>
    public bool IsReadyForUse()
    {
        return IsActive && !IsDeleted && IsValid();
    }

    #endregion

    #region Equality and Comparison

    /// <summary>
    /// Verifica se dois contatos são iguais
    /// </summary>
    /// <param name="obj">Objeto a ser comparado</param>
    /// <returns>True se iguais, false caso contrário</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Contact other)
            return false;

        return Id == other.Id ||
               (EntityId == other.EntityId &&
                EntityType == other.EntityType &&
                Type == other.Type &&
                NormalizedValue == other.NormalizedValue);
    }

    /// <summary>
    /// Obtém o hash code do contato
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, EntityId, EntityType, Type, NormalizedValue);
    }

    /// <summary>
    /// Operador de igualdade
    /// </summary>
    /// <param name="left">Primeiro contato</param>
    /// <param name="right">Segundo contato</param>
    /// <returns>True se iguais, false caso contrário</returns>
    public static bool operator ==(Contact? left, Contact? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Operador de diferença
    /// </summary>
    /// <param name="left">Primeiro contato</param>
    /// <param name="right">Segundo contato</param>
    /// <returns>True se diferentes, false caso contrário</returns>
    public static bool operator !=(Contact? left, Contact? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Compara dois contatos por prioridade e tipo
    /// </summary>
    /// <param name="other">Outro contato</param>
    /// <returns>Resultado da comparação</returns>
    public int CompareTo(Contact? other)
    {
        if (other is null)
            return 1;

        // Primeiro por prioridade (maior primeiro)
        var priorityComparison = other.Priority.CompareTo(Priority);
        if (priorityComparison != 0)
            return priorityComparison;

        // Depois por tipo (ordem alfabética)
        var typeComparison = Type.CompareTo(other.Type);
        if (typeComparison != 0)
            return typeComparison;

        // Por último por data de criação (mais recente primeiro)
        return other.CreatedAtUtc.CompareTo(CreatedAtUtc);
    }

    #endregion

    #region ToString and Serialization

    /// <summary>
    /// Representação em string do contato
    /// </summary>
    /// <returns>String representando o contato</returns>
    public override string ToString()
    {
        return $"{FullDisplayName}: {FormattedValue}";
    }

    /// <summary>
    /// Obtém uma representação resumida do contato
    /// </summary>
    /// <returns>String resumida</returns>
    public string ToShortString()
    {
        return $"{TypeDisplayName}: {FormattedValue}";
    }

    /// <summary>
    /// Obtém uma representação para debug
    /// </summary>
    /// <returns>String para debug</returns>
    public string ToDebugString()
    {
        return $"Contact[Id={Id}, Type={Type}, Category={Category}, Value={Value}, " +
               $"EntityId={EntityId}, EntityType={EntityType}, IsPrimary={IsPrimary}, " +
               $"IsActive={IsActive}, IsVerified={IsVerified}]";
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Cria uma cópia do contato com novos valores
    /// </summary>
    /// <param name="newEntityId">Novo ID da entidade</param>
    /// <param name="newEntityType">Novo tipo da entidade</param>
    /// <returns>Nova instância de Contact</returns>
    public Contact CloneForEntity(Guid newEntityId, string newEntityType)
    {
        return new Contact
        {
            Type = Type,
            Category = Category,
            Value = Value,
            Label = Label,
            IsPrimary = false, // Clone não é principal por padrão
            IsActive = IsActive,
            Notes = Notes,
            EntityId = newEntityId,
            EntityType = newEntityType,
            Priority = Priority,
            LastVerifiedAt = null, // Clone não é verificado
            IsVerified = false,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Verifica se o contato é equivalente a outro (mesmo valor normalizado)
    /// </summary>
    /// <param name="other">Outro contato</param>
    /// <returns>True se equivalente, false caso contrário</returns>
    public bool IsEquivalentTo(Contact? other)
    {
        if (other is null)
            return false;

        return Type == other.Type &&
               NormalizedValue == other.NormalizedValue &&
               EntityId == other.EntityId &&
               EntityType == other.EntityType;
    }

    /// <summary>
    /// Obtém informações de debug detalhadas
    /// </summary>
    /// <returns>String com informações detalhadas</returns>
    public string GetDebugInfo()
    {
        return $"Contact Debug Info:\n" +
               $"- ID: {Id}\n" +
               $"- Type: {Type} ({TypeDisplayName})\n" +
               $"- Category: {Category} ({CategoryDisplayName})\n" +
               $"- Value: {Value}\n" +
               $"- Normalized: {NormalizedValue}\n" +
               $"- Formatted: {FormattedValue}\n" +
               $"- Label: {Label ?? "N/A"}\n" +
               $"- Entity: {EntityType} ({EntityId})\n" +
               $"- Primary: {IsPrimary}\n" +
               $"- Active: {IsActive}\n" +
               $"- Verified: {IsVerified}\n" +
               $"- Priority: {Priority}\n" +
               $"- Created: {CreatedAtUtc:yyyy-MM-dd HH:mm:ss} UTC\n" +
               $"- Updated: {UpdatedAtUtc:yyyy-MM-dd HH:mm:ss} UTC\n" +
               $"- Last Verified: {LastVerifiedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never"} UTC";
    }

    /// <summary>
    /// Obtém um resumo do contato para logs
    /// </summary>
    /// <returns>String resumida</returns>
    public string GetLogSummary()
    {
        return $"[{TypeDisplayName}] {FormattedValue} ({CategoryDisplayName}) - {EntityType}:{EntityId}";
    }

    #endregion
}
