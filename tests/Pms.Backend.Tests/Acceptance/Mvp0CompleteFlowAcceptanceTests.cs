using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;
using Pms.Backend.Infrastructure.Data;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Pms.Backend.Api;
using Pms.Backend.Tests.Integration;

namespace Pms.Backend.Tests.Acceptance;

/// <summary>
/// Testes de aceite para o fluxo completo do MVP-0
/// Testa todo o ciclo de vida desde a criação da hierarquia até exportação de dados
/// </summary>
public class Mvp0CompleteFlowAcceptanceTests : BaseIntegrationTest
{
    public Mvp0CompleteFlowAcceptanceTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task CompleteMvp0Flow_ShouldWorkEndToEnd()
    {
        // ===== FASE 1: CRIAÇÃO DA HIERARQUIA =====

        // 1.1 Criar Divisão
        var division = await CreateDivisionAsync("Divisão Sul", "DIV-SUL");
        division.Should().NotBeNull();
        division.Name.Should().Be("Divisão Sul");

        // 1.2 Criar União
        var union = await CreateUnionAsync(division.Id, "União São Paulo", "UNI-SP");
        union.Should().NotBeNull();
        union.DivisionId.Should().Be(division.Id);

        // 1.3 Criar Associação
        var association = await CreateAssociationAsync(union.Id, "Associação Central", "ASS-CENTRAL");
        association.Should().NotBeNull();
        association.UnionId.Should().Be(union.Id);

        // 1.4 Criar Região
        var region = await CreateRegionAsync(association.Id, "Região Metropolitana", "REG-METRO");
        region.Should().NotBeNull();
        region.AssociationId.Should().Be(association.Id);

        // 1.5 Criar Distrito
        var district = await CreateDistrictAsync(region.Id, "Distrito Centro", "DIS-CENTRO");
        district.Should().NotBeNull();
        district.RegionId.Should().Be(region.Id);

        // 1.6 Criar Igreja
        var church = await CreateChurchAsync(district.Id, "Igreja Central", "IGR-CENTRAL");
        church.Should().NotBeNull();
        // church.DistrictId.Should().Be(district.Id); // Church entity doesn't have DistrictId property

        // 1.7 Criar Clube
        var club = await CreateClubAsync(church.Id, "Clube Desbravadores", "CLU-DESBRAV");
        club.Should().NotBeNull();
        club.ChurchId.Should().Be(church.Id);

        // 1.8 Criar Unidade
        var unit = await CreateUnitAsync(club.Id, "Unidade Pioneiros", "UNI-PION", Pms.Backend.Application.DTOs.Hierarchy.UnitGender.Masculina, 15, 18);
        unit.Should().NotBeNull();
        unit.ClubId.Should().Be(club.Id);
        unit.Gender.Should().Be(Pms.Backend.Application.DTOs.Hierarchy.UnitGender.Masculina);
        unit.AgeMin.Should().Be(15);
        unit.AgeMax.Should().Be(18);

        // ===== FASE 2: REGISTRO E AUTENTICAÇÃO DE MEMBROS =====

        // 2.1 Registrar primeiro membro
        var member1 = await RegisterMemberAsync("João Silva", "joao.silva@test.com", club.Id, new DateTime(2005, 6, 15));
        member1.Should().NotBeNull();
        member1.FullName.Should().Be("João Silva");
        member1.Email.Should().Be("joao.silva@test.com");

        // 2.2 Registrar segundo membro
        var member2 = await RegisterMemberAsync("Maria Santos", "maria.santos@test.com", club.Id, new DateTime(2006, 3, 20));
        member2.Should().NotBeNull();
        member2.FullName.Should().Be("Maria Santos");

        // 2.3 Fazer login com primeiro membro
        var token1 = await LoginMemberAsync("joao.silva@test.com", "Senha123!");
        token1.Should().NotBeNullOrEmpty();

        // 2.4 Fazer login com segundo membro
        var token2 = await LoginMemberAsync("maria.santos@test.com", "Senha123!");
        token2.Should().NotBeNullOrEmpty();

        // ===== FASE 3: MEMBERSHIP E ALOCAÇÃO =====

        // 3.1 Inscrever primeiro membro no clube
        var membership1 = await EnrollMemberInClubAsync(member1.Id, club.Id, token1);
        membership1.Should().NotBeNull();
        membership1.MemberId.Should().Be(member1.Id);
        membership1.ClubId.Should().Be(club.Id);
        membership1.IsActive.Should().BeTrue();

        // 3.2 Inscrever segundo membro no clube
        var membership2 = await EnrollMemberInClubAsync(member2.Id, club.Id, token2);
        membership2.Should().NotBeNull();
        membership2.MemberId.Should().Be(member2.Id);

        // 3.3 Alocar primeiro membro na unidade
        var updatedMembership1 = await AssignMemberToUnitAsync(membership1.Id, unit.Id, token1);
        updatedMembership1.Should().NotBeNull();
        updatedMembership1.UnitId.Should().Be(unit.Id);

        // 3.4 Alocar segundo membro na unidade
        var updatedMembership2 = await AssignMemberToUnitAsync(membership2.Id, unit.Id, token2);
        updatedMembership2.Should().NotBeNull();
        updatedMembership2.UnitId.Should().Be(unit.Id);

        // ===== FASE 4: VERIFICAÇÃO DE DADOS =====

        // 4.1 Verificar membros do clube
        var clubMembers = await GetClubMembersAsync(club.Id, token1);
        clubMembers.Should().HaveCount(2);
        clubMembers.Should().Contain(m => m.Email == "joao.silva@test.com");
        clubMembers.Should().Contain(m => m.Email == "maria.santos@test.com");

        // 4.2 Verificar perfil do membro
        var memberProfile = await GetMemberProfileAsync(member1.Id, token1);
        memberProfile.Should().NotBeNull();
        memberProfile.FullName.Should().Be("João Silva");
        memberProfile.Email.Should().Be("joao.silva@test.com");

        // ===== FASE 5: EXPORTAÇÃO DE DADOS =====

        // 5.1 Exportar membros para CSV
        var membersCsv = await ExportMembersToCsvAsync(club.Id, token1);
        membersCsv.Should().NotBeNullOrEmpty();
        membersCsv.Should().Contain("Name,Email,Phone");
        membersCsv.Should().Contain("joao.silva@test.com");
        membersCsv.Should().Contain("maria.santos@test.com");

        // 5.2 Exportar timeline do membro
        var timelineCsv = await ExportTimelineToCsvAsync(member1.Id, token1);
        timelineCsv.Should().NotBeNullOrEmpty();
        timelineCsv.Should().Contain("Action,Description,CreatedAt");

        // ===== FASE 6: OPERAÇÕES DE MANUTENÇÃO =====

        // 6.1 Atualizar perfil do membro
        var updatedProfile = await UpdateMemberProfileAsync(member1.Id, "João Silva Santos", "11888888888", token1);
        updatedProfile.Should().NotBeNull();
        updatedProfile.FullName.Should().Be("João Silva Santos");
        updatedProfile.Phone.Should().Be("11888888888");

        // 6.2 Desativar membership
        await DeactivateMembershipAsync(membership2.Id, token2);
        var inactiveMembership = await GetMembershipAsync(membership2.Id, token2);
        inactiveMembership.IsActive.Should().BeFalse();
        inactiveMembership.EndDate.Should().NotBeNull();

        // ===== VERIFICAÇÃO FINAL =====

        // Verificar se apenas um membro ativo permanece
        var activeMembers = await GetClubMembersAsync(club.Id, token1);
        activeMembers.Should().HaveCount(1);
        activeMembers.First().Email.Should().Be("joao.silva@test.com");

        // Verificar se a hierarquia está intacta
        var retrievedClub = await GetClubAsync(club.Id, token1);
        retrievedClub.Should().NotBeNull();
        retrievedClub.Name.Should().Be("Clube Desbravadores");
    }

    #region Helper Methods

    private async Task<DivisionDto> CreateDivisionAsync(string name, string code)
    {
        var createDto = new CreateDivisionDto
        {
            Name = name,
            Code = code,
            Description = $"Descrição da {name}"
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/divisions", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<DivisionDto>>();
        return result!.Data;
    }

    private async Task<UnionDto> CreateUnionAsync(Guid divisionId, string name, string code)
    {
        var createDto = new CreateUnionDto
        {
            Name = name,
            Code = code,
            Description = $"Descrição da {name}",
            DivisionId = divisionId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/unions", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<UnionDto>>();
        return result!.Data;
    }

    private async Task<AssociationDto> CreateAssociationAsync(Guid unionId, string name, string code)
    {
        var createDto = new CreateAssociationDto
        {
            Name = name,
            Code = code,
            Description = $"Descrição da {name}",
            UnionId = unionId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/associations", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<AssociationDto>>();
        return result!.Data;
    }

    private async Task<RegionDto> CreateRegionAsync(Guid associationId, string name, string code)
    {
        var createDto = new CreateRegionDto
        {
            Name = name,
            Code = code,
            Description = $"Descrição da {name}",
            AssociationId = associationId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/regions", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<RegionDto>>();
        return result!.Data;
    }

    private async Task<DistrictDto> CreateDistrictAsync(Guid regionId, string name, string code)
    {
        var createDto = new CreateDistrictDto
        {
            Name = name,
            Code = code,
            Description = $"Descrição do {name}",
            RegionId = regionId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/districts", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<DistrictDto>>();
        return result!.Data;
    }

    private async Task<ChurchDto> CreateChurchAsync(Guid districtId, string name, string code)
    {
        var createDto = new CreateChurchDto
        {
            Name = name,
            Cep = "01234567",
            Address = "Rua Teste, 123",
            City = "São Paulo",
            State = "SP",
            Country = "Brasil"
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/churches", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<ChurchDto>>();
        return result!.Data;
    }

    private async Task<ClubDto> CreateClubAsync(Guid churchId, string name, string code)
    {
        var createDto = new CreateClubDto
        {
            Name = name,
            Code = code,
            Description = $"Descrição do {name}",
            ChurchId = churchId
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/clubs", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<ClubDto>>();
        return result!.Data;
    }

    private async Task<UnitDto> CreateUnitAsync(Guid clubId, string name, string code, Pms.Backend.Application.DTOs.Hierarchy.UnitGender gender, int ageMin, int ageMax)
    {
        var createDto = new CreateUnitDto
        {
            Name = name,
            Description = $"Descrição da {name}",
            ClubId = clubId,
            Gender = gender,
            AgeMin = ageMin,
            AgeMax = ageMax
        };

        var response = await Client.PostAsJsonAsync("/pms/hierarchy/units", createDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<UnitDto>>();
        return result!.Data;
    }

    private async Task<MemberDto> RegisterMemberAsync(string fullName, string email, Guid clubId, DateTime dateOfBirth)
    {
        var nameParts = fullName.Split(' ');
        var registerDto = new RegisterDto
        {
            FirstName = nameParts[0],
            LastName = string.Join(" ", nameParts.Skip(1)),
            Email = email,
            Phone = "11999999999",
            DateOfBirth = dateOfBirth,
            Gender = MemberGender.Masculino,
            ClubId = clubId,
            Password = "Senha123!",
            ConfirmPassword = "Senha123!"
        };

        var response = await Client.PostAsJsonAsync("/pms/members/register", registerDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MemberDto>>();
        return result!.Data;
    }

    private async Task<string> LoginMemberAsync(string email, string password)
    {
        var loginDto = new LoginDto
        {
            Email = email,
            Password = password
        };

        var response = await Client.PostAsJsonAsync("/pms/members/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<AuthResultDto>>();
        return result!.Data.Token;
    }

    private async Task<MembershipDto> EnrollMemberInClubAsync(Guid memberId, Guid clubId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var enrollmentDto = new CreateMembershipDto
        {
            MemberId = memberId,
            ClubId = clubId,
            StartDate = DateTime.UtcNow
        };

        var response = await Client.PostAsJsonAsync("/pms/membership", enrollmentDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MembershipDto>>();
        return result!.Data;
    }

    private async Task<MembershipDto> AssignMemberToUnitAsync(Guid membershipId, Guid unitId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var assignmentDto = new UpdateMembershipDto
        {
            UnitId = unitId
        };

        var response = await Client.PutAsJsonAsync($"/pms/membership/{membershipId}", assignmentDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MembershipDto>>();
        return result!.Data;
    }

    private async Task<List<MemberDto>> GetClubMembersAsync(Guid clubId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync($"/pms/membership/club/{clubId}/members");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<MemberDto>>>();
        return result!.Data;
    }

    private async Task<MemberDto> GetMemberProfileAsync(Guid memberId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync($"/pms/members/{memberId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MemberDto>>();
        return result!.Data;
    }

    private async Task<string> ExportMembersToCsvAsync(Guid clubId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync($"/pms/reports/export/members/{clubId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<string> ExportTimelineToCsvAsync(Guid memberId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync($"/pms/reports/export/timeline/{memberId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<MemberDto> UpdateMemberProfileAsync(Guid memberId, string fullName, string phone, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var nameParts = fullName.Split(' ');
        var updateDto = new UpdateMemberDto
        {
            FirstName = nameParts[0],
            LastName = string.Join(" ", nameParts.Skip(1)),
            Phone = phone
        };

        var response = await Client.PutAsJsonAsync($"/pms/members/{memberId}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MemberDto>>();
        return result!.Data;
    }

    private async Task DeactivateMembershipAsync(Guid membershipId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await Client.DeleteAsync($"/pms/membership/{membershipId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task<MembershipDto> GetMembershipAsync(Guid membershipId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync($"/pms/membership/{membershipId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MembershipDto>>();
        return result!.Data;
    }

    private async Task<ClubDto> GetClubAsync(Guid clubId, string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync($"/pms/hierarchy/clubs/{clubId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<ClubDto>>();
        return result!.Data;
    }

    #endregion
}
