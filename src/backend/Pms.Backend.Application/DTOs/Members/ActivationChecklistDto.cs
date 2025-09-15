using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para checklist de ativação do membro
/// </summary>
public class ActivationChecklistDto
{
    /// <summary>
    /// Indica se o endereço está completo
    /// </summary>
    [Required]
    public bool HasCompleteAddress { get; set; }

    /// <summary>
    /// Indica se tem contato de email
    /// </summary>
    [Required]
    public bool HasContactEmail { get; set; }

    /// <summary>
    /// Indica se tem contato mobile (recomendado, não obrigatório no MVP)
    /// </summary>
    [Required]
    public bool HasContactMobile { get; set; }

    /// <summary>
    /// Indica se todos os critérios de ativação foram atendidos
    /// </summary>
    [Required]
    public bool IsActivationComplete => HasCompleteAddress && HasContactEmail;

    /// <summary>
    /// Lista de campos obrigatórios que estão faltando para ativação
    /// </summary>
    public List<string> GetMissingActivationFields()
    {
        var missing = new List<string>();

        if (!HasCompleteAddress)
            missing.Add("Address");

        if (!HasContactEmail)
            missing.Add("ContactEmail");

        return missing;
    }

    /// <summary>
    /// Lista de campos obrigatórios para operações de negócio
    /// </summary>
    public List<string> GetMissingOperationFields(bool hasScarfInvestiture, bool hasBaptismInfo)
    {
        var missing = new List<string>();

        if (!hasScarfInvestiture)
            missing.Add("ScarfInvestiture");

        if (!hasBaptismInfo)
            missing.Add("BaptismInfo");

        return missing;
    }

    /// <summary>
    /// Lista de campos opcionais
    /// </summary>
    public List<string> GetOptionalFields(bool hasMedicalInfo)
    {
        var optional = new List<string>();

        if (!hasMedicalInfo)
            optional.Add("MedicalInfo");

        return optional;
    }
}
