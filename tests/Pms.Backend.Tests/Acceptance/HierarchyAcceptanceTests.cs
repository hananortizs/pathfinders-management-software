using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;
using Pms.Backend.Infrastructure.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using Pms.Backend.Api;
using Pms.Backend.Tests.Integration;

namespace Pms.Backend.Tests.Acceptance;

/// <summary>
/// Testes de aceite para funcionalidades de hierarquia organizacional
/// Testa o fluxo completo desde a criação de divisões até unidades
/// </summary>
public class HierarchyAcceptanceTests : BaseIntegrationTest
{
    public HierarchyAcceptanceTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateCompleteHierarchy_ShouldWorkEndToEnd()
    {
        // Arrange - Criar uma hierarquia completa
        var division = await CreateDivisionAsync();
        var union = await CreateUnionAsync(division.Id);
        var association = await CreateAssociationAsync(union.Id);
        var region = await CreateRegionAsync(association.Id);
        var district = await CreateDistrictAsync(region.Id);
        var church = await CreateChurchAsync(district.Id);
        var club = await CreateClubAsync(church.Id);
        var unit = await CreateUnitAsync(club.Id);

        // Act & Assert - Verificar se todos os níveis foram criados corretamente
        division.Should().NotBeNull();
        union.Should().NotBeNull();
        association.Should().NotBeNull();
        region.Should().NotBeNull();
        district.Should().NotBeNull();
        church.Should().NotBeNull();
        club.Should().NotBeNull();
        unit.Should().NotBeNull();

        // Verificar se as relações estão corretas
        union.DivisionId.Should().Be(division.Id);
        association.UnionId.Should().Be(union.Id);
        region.AssociationId.Should().Be(association.Id);
        district.RegionId.Should().Be(region.Id);
        // church.DistrictId.Should().Be(district.Id); // Church não tem DistrictId
        club.ChurchId.Should().Be(church.Id);
        unit.ClubId.Should().Be(club.Id);
    }

    [Fact]
    public async Task GetHierarchyByLevel_ShouldReturnCorrectData()
    {
        // Arrange - Criar hierarquia de teste
        var division = await CreateDivisionAsync();
        var union = await CreateUnionAsync(division.Id);
        var association = await CreateAssociationAsync(union.Id);

        // Act - Buscar dados por nível
        var divisionsResponse = await Client.GetAsync("/pms/hierarchy/divisions");
        var unionsResponse = await Client.GetAsync($"/pms/hierarchy/divisions/{division.Id}/unions");
        var associationsResponse = await Client.GetAsync($"/pms/hierarchy/unions/{union.Id}/associations");

        // Assert - Verificar se os dados foram retornados corretamente
        divisionsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        unionsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        associationsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var divisions = await divisionsResponse.Content.ReadFromJsonAsync<BaseResponse<List<DivisionDto>>>();
        var unions = await unionsResponse.Content.ReadFromJsonAsync<BaseResponse<List<UnionDto>>>();
        var associations = await associationsResponse.Content.ReadFromJsonAsync<BaseResponse<List<AssociationDto>>>();

        divisions!.Data.Should().HaveCount(1);
        unions!.Data.Should().HaveCount(1);
        associations!.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateHierarchyEntity_ShouldPersistChanges()
    {
        // Arrange - Criar uma divisão
        var division = await CreateDivisionAsync();
        var updateDto = new UpdateDivisionDto
        {
            Name = "Divisão Atualizada",
            Code = "DIV-UPD",
            Description = "Descrição atualizada"
        };

        // Act - Atualizar a divisão
        var response = await Client.PutAsJsonAsync($"/pms/hierarchy/divisions/{division.Id}", updateDto);

        // Assert - Verificar se a atualização foi bem-sucedida
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedDivision = await response.Content.ReadFromJsonAsync<BaseResponse<DivisionDto>>();
        updatedDivision.Should().NotBeNull();
        updatedDivision!.Data.Should().NotBeNull();
        updatedDivision.Data!.Name.Should().Be("Divisão Atualizada");
        updatedDivision.Data.Code.Should().Be("DIV-UPD");
    }

    [Fact]
    public async Task DeleteHierarchyEntity_ShouldRemoveFromDatabase()
    {
        // Arrange - Criar uma divisão
        var division = await CreateDivisionAsync();

        // Act - Deletar a divisão
        var response = await Client.DeleteAsync($"/pms/hierarchy/divisions/{division.Id}");

        // Assert - Verificar se a divisão foi removida
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await Client.GetAsync($"/pms/hierarchy/divisions/{division.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #region Helper Methods

    private async Task<DivisionDto> CreateDivisionAsync()
    {
        var createDto = new CreateDivisionDto
        {
            Name = "Divisão Teste",
            Code = "DIV-TEST",
            Description = "Divisão para testes"
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/divisions", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<DivisionDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    private async Task<UnionDto> CreateUnionAsync(Guid divisionId)
    {
        var createDto = new CreateUnionDto
        {
            Name = "União Teste",
            Code = "UNI-TEST",
            Description = "União para testes",
            DivisionId = divisionId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/unions", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<UnionDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    private async Task<AssociationDto> CreateAssociationAsync(Guid unionId)
    {
        var createDto = new CreateAssociationDto
        {
            Name = "Associação Teste",
            Code = "ASS-TEST",
            Description = "Associação para testes",
            UnionId = unionId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/associations", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<AssociationDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    private async Task<RegionDto> CreateRegionAsync(Guid associationId)
    {
        var createDto = new CreateRegionDto
        {
            Name = "Região Teste",
            Code = "REG-TEST",
            Description = "Região para testes",
            AssociationId = associationId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/regions", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<RegionDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    private async Task<DistrictDto> CreateDistrictAsync(Guid regionId)
    {
        var createDto = new CreateDistrictDto
        {
            Name = "Distrito Teste",
            Code = "DIS-TEST",
            Description = "Distrito para testes",
            RegionId = regionId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/districts", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<DistrictDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    private async Task<ChurchDto> CreateChurchAsync(Guid districtId)
    {
        var createDto = new CreateChurchDto
        {
            Name = "Igreja Teste",
            Cep = "01234567",
            Address = "Rua Teste, 123",
            City = "São Paulo",
            State = "SP",
            Country = "Brasil"
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/churches", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<ChurchDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    private async Task<ClubDto> CreateClubAsync(Guid churchId)
    {
        var createDto = new CreateClubDto
        {
            Name = "Clube Teste",
            Code = "CLU-TEST",
            Description = "Clube para testes",
            ChurchId = churchId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/clubs", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<ClubDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    private async Task<UnitDto> CreateUnitAsync(Guid clubId)
    {
        var createDto = new CreateUnitDto
        {
            Name = "Unidade Teste",
            Description = "Unidade para testes",
            ClubId = clubId,
            Gender = Pms.Backend.Application.DTOs.Hierarchy.UnitGender.Masculina,
            AgeMin = 10,
            AgeMax = 15
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/units", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<UnitDto>>();
        result.Should().NotBeNull();
        return result!.Data!;
    }

    #endregion
}
