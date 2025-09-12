using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;
using System.Text;

namespace Pms.Backend.Application.Services.Hierarchy;

/// <summary>
/// Serviço para gerenciamento de códigos hierárquicos
/// </summary>
public class HierarchyCodeService : IHierarchyCodeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HierarchyCodeService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do HierarchyCodeService
    /// </summary>
    /// <param name="unitOfWork">Unit of Work</param>
    /// <param name="logger">Logger</param>
    public HierarchyCodeService(IUnitOfWork unitOfWork, ILogger<HierarchyCodeService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Gera um código único para uma entidade hierárquica
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="parentCode">Código da entidade pai (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Código único gerado</returns>
    public async Task<BaseResponse<string>> GenerateUniqueCodeAsync(string entityType, string? parentCode = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando código único para entidade: {EntityType}, Pai: {ParentCode}", entityType, parentCode);

            // Gerar código baseado no tipo de entidade
            var codePrefix = GetCodePrefix(entityType);
            var maxAttempts = 100;
            var attempt = 0;

            while (attempt < maxAttempts)
            {
                var code = GenerateCode(codePrefix, attempt);

                // Verificar se o código é único
                var isUnique = await ValidateCodeUniquenessAsync(code, entityType, null, cancellationToken);
                if (isUnique.IsSuccess && isUnique.Data)
                {
                    _logger.LogInformation("Código único gerado: {Code} para entidade: {EntityType}", code, entityType);
                    return BaseResponse<string>.SuccessResult(code, "Código único gerado com sucesso");
                }

                attempt++;
            }

            _logger.LogError("Não foi possível gerar código único após {MaxAttempts} tentativas para entidade: {EntityType}", maxAttempts, entityType);
            return BaseResponse<string>.ErrorResult("Não foi possível gerar código único");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar código único para entidade: {EntityType}", entityType);
            return BaseResponse<string>.ErrorResult("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Valida se um código é único para uma entidade
    /// </summary>
    /// <param name="code">Código a ser validado</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade (opcional, para exclusão na validação)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o código for único</returns>
    public async Task<BaseResponse<bool>> ValidateCodeUniquenessAsync(string code, string entityType, Guid? entityId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Validando unicidade do código: {Code} para entidade: {EntityType}", code, entityType);

            // Validar formato do código
            if (!IsValidCodeFormat(code))
            {
                return BaseResponse<bool>.ErrorResult("Formato de código inválido");
            }

            // Verificar unicidade baseado no tipo de entidade
            var isUnique = entityType.ToLower() switch
            {
                "division" => await CheckDivisionCodeUniqueness(code, entityId, cancellationToken),
                "union" => await CheckUnionCodeUniqueness(code, entityId, cancellationToken),
                "association" => await CheckAssociationCodeUniqueness(code, entityId, cancellationToken),
                "region" => await CheckRegionCodeUniqueness(code, entityId, cancellationToken),
                "district" => await CheckDistrictCodeUniqueness(code, entityId, cancellationToken),
                "club" => await CheckClubCodeUniqueness(code, entityId, cancellationToken),
                "unit" => await CheckUnitCodeUniqueness(code, entityId, cancellationToken),
                _ => false
            };

            return BaseResponse<bool>.SuccessResult(isUnique, isUnique ? "Código é único" : "Código já existe");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar unicidade do código: {Code} para entidade: {EntityType}", code, entityType);
            return BaseResponse<bool>.ErrorResult("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Gera o CodePath para uma entidade
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>CodePath gerado</returns>
    public async Task<BaseResponse<string>> GenerateCodePathAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando CodePath para entidade: {EntityType}, ID: {EntityId}", entityType, entityId);

            var codePath = await BuildCodePath(entityType, entityId, cancellationToken);

            if (string.IsNullOrEmpty(codePath))
            {
                return BaseResponse<string>.ErrorResult("Não foi possível gerar CodePath");
            }

            _logger.LogInformation("CodePath gerado: {CodePath} para entidade: {EntityType}, ID: {EntityId}", codePath, entityType, entityId);
            return BaseResponse<string>.SuccessResult(codePath, "CodePath gerado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar CodePath para entidade: {EntityType}, ID: {EntityId}", entityType, entityId);
            return BaseResponse<string>.ErrorResult("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Atualiza o CodePath de uma entidade e recalcula em cascata
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> UpdateCodePathAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Atualizando CodePath para entidade: {EntityType}, ID: {EntityId}", entityType, entityId);

            // Gerar novo CodePath
            var codePathResult = await GenerateCodePathAsync(entityType, entityId, cancellationToken);
            if (!codePathResult.IsSuccess)
            {
                return BaseResponse<bool>.ErrorResult(codePathResult.Message ?? "Erro ao gerar CodePath");
            }

            // Atualizar a entidade
            var updateResult = await UpdateEntityCodePath(entityType, entityId, codePathResult.Data!, cancellationToken);
            if (!updateResult)
            {
                return BaseResponse<bool>.ErrorResult("Falha ao atualizar CodePath da entidade");
            }

            // Recalcular CodePaths das entidades filhas
            await RecalculateChildrenCodePaths(entityType, entityId, cancellationToken);

            _logger.LogInformation("CodePath atualizado com sucesso para entidade: {EntityType}, ID: {EntityId}", entityType, entityId);
            return BaseResponse<bool>.SuccessResult(true, "CodePath atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar CodePath para entidade: {EntityType}, ID: {EntityId}", entityType, entityId);
            return BaseResponse<bool>.ErrorResult("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Recalcula todos os CodePaths de uma hierarquia
    /// </summary>
    /// <param name="rootEntityType">Tipo da entidade raiz</param>
    /// <param name="rootEntityId">ID da entidade raiz</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> RecalculateHierarchyCodePathsAsync(string rootEntityType, Guid rootEntityId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Recalculando CodePaths da hierarquia: {EntityType}, ID: {EntityId}", rootEntityType, rootEntityId);

            // Recalcular a partir da raiz
            await RecalculateEntityCodePath(rootEntityType, rootEntityId, cancellationToken);

            _logger.LogInformation("CodePaths da hierarquia recalculados com sucesso: {EntityType}, ID: {EntityId}", rootEntityType, rootEntityId);
            return BaseResponse<bool>.SuccessResult(true, "CodePaths da hierarquia recalculados com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recalcular CodePaths da hierarquia: {EntityType}, ID: {EntityId}", rootEntityType, rootEntityId);
            return BaseResponse<bool>.ErrorResult("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém o código de uma entidade
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Código da entidade</returns>
    public async Task<BaseResponse<string>> GetEntityCodeAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Obtendo código da entidade: {EntityType}, ID: {EntityId}", entityType, entityId);

            var code = entityType.ToLower() switch
            {
                "division" => await GetDivisionCode(entityId, cancellationToken),
                "union" => await GetUnionCode(entityId, cancellationToken),
                "association" => await GetAssociationCode(entityId, cancellationToken),
                "region" => await GetRegionCode(entityId, cancellationToken),
                "district" => await GetDistrictCode(entityId, cancellationToken),
                "club" => await GetClubCode(entityId, cancellationToken),
                "unit" => await GetUnitCode(entityId, cancellationToken),
                _ => null
            };

            if (string.IsNullOrEmpty(code))
            {
                return BaseResponse<string>.ErrorResult("Código não encontrado");
            }

            return BaseResponse<string>.SuccessResult(code, "Código obtido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter código da entidade: {EntityType}, ID: {EntityId}", entityType, entityId);
            return BaseResponse<string>.ErrorResult("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém o CodePath de uma entidade
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>CodePath da entidade</returns>
    public async Task<BaseResponse<string>> GetEntityCodePathAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Obtendo CodePath da entidade: {EntityType}, ID: {EntityId}", entityType, entityId);

            var codePath = entityType.ToLower() switch
            {
                "division" => await GetDivisionCodePath(entityId, cancellationToken),
                "union" => await GetUnionCodePath(entityId, cancellationToken),
                "association" => await GetAssociationCodePath(entityId, cancellationToken),
                "region" => await GetRegionCodePath(entityId, cancellationToken),
                "district" => await GetDistrictCodePath(entityId, cancellationToken),
                "club" => await GetClubCodePath(entityId, cancellationToken),
                "unit" => await GetUnitCodePath(entityId, cancellationToken),
                _ => null
            };

            if (string.IsNullOrEmpty(codePath))
            {
                return BaseResponse<string>.ErrorResult("CodePath não encontrado");
            }

            return BaseResponse<string>.SuccessResult(codePath, "CodePath obtido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter CodePath da entidade: {EntityType}, ID: {EntityId}", entityType, entityId);
            return BaseResponse<string>.ErrorResult("Erro interno do servidor");
        }
    }

    #region Private Methods

    /// <summary>
    /// Obtém o prefixo do código baseado no tipo de entidade
    /// </summary>
    private static string GetCodePrefix(string entityType)
    {
        return entityType.ToLower() switch
        {
            "division" => "D",
            "union" => "U",
            "association" => "A",
            "region" => "R",
            "district" => "DT",
            "club" => "C",
            "unit" => "UN",
            _ => "X"
        };
    }

    /// <summary>
    /// Gera um código baseado no prefixo e tentativa
    /// </summary>
    private static string GenerateCode(string prefix, int attempt)
    {
        var random = new Random();
        var suffix = random.Next(1000, 9999).ToString();
        return $"{prefix}{suffix}";
    }

    /// <summary>
    /// Valida o formato do código
    /// </summary>
    private static bool IsValidCodeFormat(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        if (code.Length > 5)
            return false;

        return code.All(c => char.IsLetterOrDigit(c));
    }

    /// <summary>
    /// Constrói o CodePath para uma entidade
    /// </summary>
    private async Task<string> BuildCodePath(string entityType, Guid entityId, CancellationToken cancellationToken)
    {
        var codePath = new StringBuilder();
        var currentEntityType = entityType;
        var currentEntityId = entityId;

        while (currentEntityId != Guid.Empty)
        {
            var code = await GetEntityCodeAsync(currentEntityType, currentEntityId, cancellationToken);
            if (!code.IsSuccess || string.IsNullOrEmpty(code.Data))
                break;

            if (codePath.Length > 0)
                codePath.Insert(0, ".");

            codePath.Insert(0, code.Data);

            // Obter entidade pai
            var parentInfo = await GetParentEntity(currentEntityType, currentEntityId, cancellationToken);
            if (parentInfo == null)
                break;

            currentEntityType = parentInfo.Value.EntityType;
            currentEntityId = parentInfo.Value.EntityId;
        }

        return codePath.ToString();
    }

    /// <summary>
    /// Obtém informações da entidade pai
    /// </summary>
    private async Task<(string EntityType, Guid EntityId)?> GetParentEntity(string entityType, Guid entityId, CancellationToken cancellationToken)
    {
        return entityType.ToLower() switch
        {
            "union" => await GetDivisionParent(entityId, cancellationToken),
            "association" => await GetUnionParent(entityId, cancellationToken),
            "region" => await GetAssociationParent(entityId, cancellationToken),
            "district" => await GetRegionParent(entityId, cancellationToken),
            "club" => await GetDistrictParent(entityId, cancellationToken),
            "unit" => await GetClubParent(entityId, cancellationToken),
            _ => null
        };
    }

    /// <summary>
    /// Recalcula o CodePath de uma entidade específica
    /// </summary>
    private async Task RecalculateEntityCodePath(string entityType, Guid entityId, CancellationToken cancellationToken)
    {
        var codePathResult = await GenerateCodePathAsync(entityType, entityId, cancellationToken);
        if (codePathResult.IsSuccess)
        {
            await UpdateEntityCodePath(entityType, entityId, codePathResult.Data!, cancellationToken);
        }
    }

    /// <summary>
    /// Recalcula CodePaths das entidades filhas
    /// </summary>
    private async Task RecalculateChildrenCodePaths(string entityType, Guid entityId, CancellationToken cancellationToken)
    {
        var children = await GetChildEntities(entityType, entityId, cancellationToken);

        foreach (var child in children)
        {
            await RecalculateEntityCodePath(child.EntityType, child.EntityId, cancellationToken);
        }
    }

    /// <summary>
    /// Obtém entidades filhas
    /// </summary>
    private async Task<List<(string EntityType, Guid EntityId)>> GetChildEntities(string entityType, Guid entityId, CancellationToken cancellationToken)
    {
        var children = new List<(string EntityType, Guid EntityId)>();

        switch (entityType.ToLower())
        {
            case "division":
                var unions = await _unitOfWork.Repository<Union>()
                    .GetAsync(u => u.DivisionId == entityId, cancellationToken);
                children.AddRange(unions.Select(u => ("union", u.Id)));
                break;

            case "union":
                var associations = await _unitOfWork.Repository<Association>()
                    .GetAsync(a => a.UnionId == entityId, cancellationToken);
                children.AddRange(associations.Select(a => ("association", a.Id)));
                break;

            case "association":
                var regions = await _unitOfWork.Repository<Region>()
                    .GetAsync(r => r.AssociationId == entityId, cancellationToken);
                children.AddRange(regions.Select(r => ("region", r.Id)));
                break;

            case "region":
                var districts = await _unitOfWork.Repository<District>()
                    .GetAsync(d => d.RegionId == entityId, cancellationToken);
                children.AddRange(districts.Select(d => ("district", d.Id)));
                break;

            case "district":
                var clubs = await _unitOfWork.Repository<Club>()
                    .GetAsync(c => c.DistrictId == entityId, cancellationToken);
                children.AddRange(clubs.Select(c => ("club", c.Id)));
                break;

            case "club":
                var units = await _unitOfWork.Repository<Unit>()
                    .GetAsync(u => u.ClubId == entityId, cancellationToken);
                children.AddRange(units.Select(u => ("unit", u.Id)));
                break;
        }

        return children;
    }

    #endregion

    #region Entity-Specific Methods

    // Division methods
    private async Task<bool> CheckDivisionCodeUniqueness(string code, Guid? entityId, CancellationToken cancellationToken)
    {
        var divisions = await _unitOfWork.Repository<Division>()
            .GetAsync(d => d.Code == code && (entityId == null || d.Id != entityId), cancellationToken);
        return !divisions.Any();
    }

    private async Task<string?> GetDivisionCode(Guid entityId, CancellationToken cancellationToken)
    {
        var division = await _unitOfWork.Repository<Division>().GetByIdAsync(entityId, cancellationToken);
        return division?.Code;
    }

    private async Task<string?> GetDivisionCodePath(Guid entityId, CancellationToken cancellationToken)
    {
        var division = await _unitOfWork.Repository<Division>().GetByIdAsync(entityId, cancellationToken);
        return division?.CodePath;
    }

    private Task<(string EntityType, Guid EntityId)?> GetDivisionParent(Guid entityId, CancellationToken cancellationToken)
    {
        // Division não tem pai
        return Task.FromResult<(string EntityType, Guid EntityId)?>(null);
    }

    // Union methods
    private async Task<bool> CheckUnionCodeUniqueness(string code, Guid? entityId, CancellationToken cancellationToken)
    {
        var unions = await _unitOfWork.Repository<Union>()
            .GetAsync(u => u.Code == code && (entityId == null || u.Id != entityId), cancellationToken);
        return !unions.Any();
    }

    private async Task<string?> GetUnionCode(Guid entityId, CancellationToken cancellationToken)
    {
        var union = await _unitOfWork.Repository<Union>().GetByIdAsync(entityId, cancellationToken);
        return union?.Code;
    }

    private async Task<string?> GetUnionCodePath(Guid entityId, CancellationToken cancellationToken)
    {
        var union = await _unitOfWork.Repository<Union>().GetByIdAsync(entityId, cancellationToken);
        return union?.CodePath;
    }

    private async Task<(string EntityType, Guid EntityId)?> GetUnionParent(Guid entityId, CancellationToken cancellationToken)
    {
        var union = await _unitOfWork.Repository<Union>().GetByIdAsync(entityId, cancellationToken);
        return union?.DivisionId != null ? ("division", union.DivisionId) : null;
    }

    // Association methods
    private async Task<bool> CheckAssociationCodeUniqueness(string code, Guid? entityId, CancellationToken cancellationToken)
    {
        var associations = await _unitOfWork.Repository<Association>()
            .GetAsync(a => a.Code == code && (entityId == null || a.Id != entityId), cancellationToken);
        return !associations.Any();
    }

    private async Task<string?> GetAssociationCode(Guid entityId, CancellationToken cancellationToken)
    {
        var association = await _unitOfWork.Repository<Association>().GetByIdAsync(entityId, cancellationToken);
        return association?.Code;
    }

    private async Task<string?> GetAssociationCodePath(Guid entityId, CancellationToken cancellationToken)
    {
        var association = await _unitOfWork.Repository<Association>().GetByIdAsync(entityId, cancellationToken);
        return association?.CodePath;
    }

    private async Task<(string EntityType, Guid EntityId)?> GetAssociationParent(Guid entityId, CancellationToken cancellationToken)
    {
        var association = await _unitOfWork.Repository<Association>().GetByIdAsync(entityId, cancellationToken);
        return association?.UnionId != null ? ("union", association.UnionId) : null;
    }

    // Region methods
    private async Task<bool> CheckRegionCodeUniqueness(string code, Guid? entityId, CancellationToken cancellationToken)
    {
        var regions = await _unitOfWork.Repository<Region>()
            .GetAsync(r => r.Code == code && (entityId == null || r.Id != entityId), cancellationToken);
        return !regions.Any();
    }

    private async Task<string?> GetRegionCode(Guid entityId, CancellationToken cancellationToken)
    {
        var region = await _unitOfWork.Repository<Region>().GetByIdAsync(entityId, cancellationToken);
        return region?.Code;
    }

    private async Task<string?> GetRegionCodePath(Guid entityId, CancellationToken cancellationToken)
    {
        var region = await _unitOfWork.Repository<Region>().GetByIdAsync(entityId, cancellationToken);
        return region?.CodePath;
    }

    private async Task<(string EntityType, Guid EntityId)?> GetRegionParent(Guid entityId, CancellationToken cancellationToken)
    {
        var region = await _unitOfWork.Repository<Region>().GetByIdAsync(entityId, cancellationToken);
        return region?.AssociationId != null ? ("association", region.AssociationId) : null;
    }

    // District methods
    private async Task<bool> CheckDistrictCodeUniqueness(string code, Guid? entityId, CancellationToken cancellationToken)
    {
        var districts = await _unitOfWork.Repository<District>()
            .GetAsync(d => d.Code == code && (entityId == null || d.Id != entityId), cancellationToken);
        return !districts.Any();
    }

    private async Task<string?> GetDistrictCode(Guid entityId, CancellationToken cancellationToken)
    {
        var district = await _unitOfWork.Repository<District>().GetByIdAsync(entityId, cancellationToken);
        return district?.Code;
    }

    private async Task<string?> GetDistrictCodePath(Guid entityId, CancellationToken cancellationToken)
    {
        var district = await _unitOfWork.Repository<District>().GetByIdAsync(entityId, cancellationToken);
        return district?.CodePath;
    }

    private async Task<(string EntityType, Guid EntityId)?> GetDistrictParent(Guid entityId, CancellationToken cancellationToken)
    {
        var district = await _unitOfWork.Repository<District>().GetByIdAsync(entityId, cancellationToken);
        return district?.RegionId != null ? ("region", district.RegionId) : null;
    }

    // Club methods
    private async Task<bool> CheckClubCodeUniqueness(string code, Guid? entityId, CancellationToken cancellationToken)
    {
        var clubs = await _unitOfWork.Repository<Club>()
            .GetAsync(c => c.Code == code && (entityId == null || c.Id != entityId), cancellationToken);
        return !clubs.Any();
    }

    private async Task<string?> GetClubCode(Guid entityId, CancellationToken cancellationToken)
    {
        var club = await _unitOfWork.Repository<Club>().GetByIdAsync(entityId, cancellationToken);
        return club?.Code;
    }

    private async Task<string?> GetClubCodePath(Guid entityId, CancellationToken cancellationToken)
    {
        var club = await _unitOfWork.Repository<Club>().GetByIdAsync(entityId, cancellationToken);
        return club?.CodePath;
    }

    private async Task<(string EntityType, Guid EntityId)?> GetClubParent(Guid entityId, CancellationToken cancellationToken)
    {
        var club = await _unitOfWork.Repository<Club>().GetByIdAsync(entityId, cancellationToken);
        return club?.DistrictId != null ? ("district", club.DistrictId) : null;
    }

    // Unit methods
    private Task<bool> CheckUnitCodeUniqueness(string code, Guid? entityId, CancellationToken cancellationToken)
    {
        // Unit não tem código único, sempre retorna true
        return Task.FromResult(true);
    }

    private Task<string?> GetUnitCode(Guid entityId, CancellationToken cancellationToken)
    {
        // Unit não tem código único
        return Task.FromResult<string?>(null);
    }

    private Task<string?> GetUnitCodePath(Guid entityId, CancellationToken cancellationToken)
    {
        // Unit não tem CodePath
        return Task.FromResult<string?>(null);
    }

    private async Task<(string EntityType, Guid EntityId)?> GetUnitParent(Guid entityId, CancellationToken cancellationToken)
    {
        var unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(entityId, cancellationToken);
        return unit?.ClubId != null ? ("club", unit.ClubId) : null;
    }

    /// <summary>
    /// Atualiza o CodePath de uma entidade
    /// </summary>
    private async Task<bool> UpdateEntityCodePath(string entityType, Guid entityId, string codePath, CancellationToken cancellationToken)
    {
        try
        {
            // CodePath é calculado automaticamente pelas propriedades das entidades
            // Não precisamos atualizar nada, apenas salvar para forçar o recálculo
            switch (entityType.ToLower())
            {
                case "division":
                    var division = await _unitOfWork.Repository<Division>().GetByIdAsync(entityId, cancellationToken);
                    if (division != null)
                    {
                        await _unitOfWork.Repository<Division>().UpdateAsync(division, cancellationToken);
                    }
                    break;

                case "union":
                    var union = await _unitOfWork.Repository<Union>().GetByIdAsync(entityId, cancellationToken);
                    if (union != null)
                    {
                        await _unitOfWork.Repository<Union>().UpdateAsync(union, cancellationToken);
                    }
                    break;

                case "association":
                    var association = await _unitOfWork.Repository<Association>().GetByIdAsync(entityId, cancellationToken);
                    if (association != null)
                    {
                        await _unitOfWork.Repository<Association>().UpdateAsync(association, cancellationToken);
                    }
                    break;

                case "region":
                    var region = await _unitOfWork.Repository<Region>().GetByIdAsync(entityId, cancellationToken);
                    if (region != null)
                    {
                        await _unitOfWork.Repository<Region>().UpdateAsync(region, cancellationToken);
                    }
                    break;

                case "district":
                    var district = await _unitOfWork.Repository<District>().GetByIdAsync(entityId, cancellationToken);
                    if (district != null)
                    {
                        await _unitOfWork.Repository<District>().UpdateAsync(district, cancellationToken);
                    }
                    break;

                case "club":
                    var club = await _unitOfWork.Repository<Club>().GetByIdAsync(entityId, cancellationToken);
                    if (club != null)
                    {
                        await _unitOfWork.Repository<Club>().UpdateAsync(club, cancellationToken);
                    }
                    break;

                case "unit":
                    var unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(entityId, cancellationToken);
                    if (unit != null)
                    {
                        await _unitOfWork.Repository<Unit>().UpdateAsync(unit, cancellationToken);
                    }
                    break;

                default:
                    return false;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar CodePath da entidade: {EntityType}, ID: {EntityId}", entityType, entityId);
            return false;
        }
    }

    #endregion
}
