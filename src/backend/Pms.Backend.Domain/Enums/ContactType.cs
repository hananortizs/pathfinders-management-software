namespace Pms.Backend.Domain.Enums;

/// <summary>
/// Tipos de contato disponíveis no sistema
/// </summary>
public enum ContactType
{
    /// <summary>
    /// Telefone celular
    /// </summary>
    Mobile = 1,

    /// <summary>
    /// Telefone fixo
    /// </summary>
    Landline = 2,

    /// <summary>
    /// Email
    /// </summary>
    Email = 3,

    /// <summary>
    /// WhatsApp
    /// </summary>
    WhatsApp = 4,

    /// <summary>
    /// Facebook
    /// </summary>
    Facebook = 5,

    /// <summary>
    /// Instagram
    /// </summary>
    Instagram = 6,

    /// <summary>
    /// YouTube
    /// </summary>
    YouTube = 7,

    /// <summary>
    /// TikTok
    /// </summary>
    TikTok = 8,

    /// <summary>
    /// LinkedIn
    /// </summary>
    LinkedIn = 9,

    /// <summary>
    /// Twitter/X
    /// </summary>
    Twitter = 10,

    /// <summary>
    /// Site/Website
    /// </summary>
    Website = 11,

    /// <summary>
    /// Outro tipo de contato
    /// </summary>
    Other = 99
}

/// <summary>
/// Categorias de contato para organização
/// </summary>
public enum ContactCategory
{
    /// <summary>
    /// Contato pessoal principal
    /// </summary>
    Personal = 1,

    /// <summary>
    /// Contato de emergência
    /// </summary>
    Emergency = 2,

    /// <summary>
    /// Contato profissional/trabalho
    /// </summary>
    Professional = 3,

    /// <summary>
    /// Contato da igreja (pastor, ancião, secretário)
    /// </summary>
    Church = 4,

    /// <summary>
    /// Contato de responsável legal (para menores)
    /// </summary>
    LegalGuardian = 5,

    /// <summary>
    /// Contato de parente
    /// </summary>
    Family = 6,

    /// <summary>
    /// Contato de amigo
    /// </summary>
    Friend = 7,

    /// <summary>
    /// Outra categoria
    /// </summary>
    Other = 99
}

/// <summary>
/// Helper class para operações com ContactType
/// </summary>
public static class ContactTypeHelper
{
    /// <summary>
    /// Verifica se o tipo de contato é uma rede social
    /// </summary>
    /// <param name="contactType">Tipo de contato</param>
    /// <returns>True se for rede social, false caso contrário</returns>
    public static bool IsSocialMedia(ContactType contactType)
    {
        return contactType switch
        {
            ContactType.Facebook or
            ContactType.Instagram or
            ContactType.YouTube or
            ContactType.TikTok or
            ContactType.LinkedIn or
            ContactType.Twitter => true,
            _ => false
        };
    }

    /// <summary>
    /// Verifica se o tipo de contato é telefônico
    /// </summary>
    /// <param name="contactType">Tipo de contato</param>
    /// <returns>True se for telefônico, false caso contrário</returns>
    public static bool IsPhone(ContactType contactType)
    {
        return contactType switch
        {
            ContactType.Mobile or
            ContactType.Landline or
            ContactType.WhatsApp => true,
            _ => false
        };
    }

    /// <summary>
    /// Verifica se o tipo de contato é digital (email, redes sociais, site)
    /// </summary>
    /// <param name="contactType">Tipo de contato</param>
    /// <returns>True se for digital, false caso contrário</returns>
    public static bool IsDigital(ContactType contactType)
    {
        return contactType switch
        {
            ContactType.Email or
            ContactType.Website or
            ContactType.Facebook or
            ContactType.Instagram or
            ContactType.YouTube or
            ContactType.TikTok or
            ContactType.LinkedIn or
            ContactType.Twitter => true,
            _ => false
        };
    }

    /// <summary>
    /// Obtém o nome de exibição do tipo de contato
    /// </summary>
    /// <param name="contactType">Tipo de contato</param>
    /// <returns>Nome de exibição</returns>
    public static string GetDisplayName(ContactType contactType)
    {
        return contactType switch
        {
            ContactType.Mobile => "Celular",
            ContactType.Landline => "Telefone Fixo",
            ContactType.Email => "Email",
            ContactType.WhatsApp => "WhatsApp",
            ContactType.Facebook => "Facebook",
            ContactType.Instagram => "Instagram",
            ContactType.YouTube => "YouTube",
            ContactType.TikTok => "TikTok",
            ContactType.LinkedIn => "LinkedIn",
            ContactType.Twitter => "Twitter/X",
            ContactType.Website => "Site/Website",
            ContactType.Other => "Outro",
            _ => contactType.ToString()
        };
    }

    /// <summary>
    /// Obtém o nome de exibição da categoria de contato
    /// </summary>
    /// <param name="category">Categoria de contato</param>
    /// <returns>Nome de exibição</returns>
    public static string GetDisplayName(ContactCategory category)
    {
        return category switch
        {
            ContactCategory.Personal => "Pessoal",
            ContactCategory.Emergency => "Emergência",
            ContactCategory.Professional => "Profissional",
            ContactCategory.Church => "Igreja",
            ContactCategory.LegalGuardian => "Responsável Legal",
            ContactCategory.Family => "Família",
            ContactCategory.Friend => "Amigo",
            ContactCategory.Other => "Outro",
            _ => category.ToString()
        };
    }
}
