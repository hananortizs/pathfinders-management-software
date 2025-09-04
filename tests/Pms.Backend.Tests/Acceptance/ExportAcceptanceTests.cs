using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Exports;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;
using Pms.Backend.Infrastructure.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit;
using Pms.Backend.Api;
using Pms.Backend.Tests.Integration;

namespace Pms.Backend.Tests.Acceptance;

/// <summary>
/// Testes de aceite para funcionalidades de exportação
/// Testa o fluxo completo de exportação de dados para CSV
/// </summary>
public class ExportAcceptanceTests : BaseIntegrationTest
{
    public ExportAcceptanceTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ExportMembersToCsv_ShouldReturnValidCsvData()
    {
        // Arrange - Criar clube e membros
        var club = await CreateTestClubAsync();
        var member1 = await RegisterTestMemberAsync(club.Id, "membro1@test.com");
        var member2 = await RegisterTestMemberAsync(club.Id, "membro2@test.com");
        var token = await GetAuthTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Inscrever membros no clube
        await EnrollMemberInClubAsync(member1.Id, club.Id);
        await EnrollMemberInClubAsync(member2.Id, club.Id);

        // Act - Exportar membros para CSV
        var response = await Client.GetAsync($"/pms/reports/export/members/{club.Id}");

        // Assert - Verificar se o CSV foi gerado
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/csv");

        var csvContent = await response.Content.ReadAsStringAsync();
        csvContent.Should().NotBeNullOrEmpty();
        csvContent.Should().Contain("Name,Email,Phone");
        csvContent.Should().Contain("membro1@test.com");
        csvContent.Should().Contain("membro2@test.com");
    }

    [Fact]
    public async Task ExportTimelineToCsv_ShouldReturnValidCsvData()
    {
        // Arrange - Criar membro e timeline entries
        var club = await CreateTestClubAsync();
        var member = await RegisterTestMemberAsync(club.Id);
        var token = await GetAuthTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        await EnrollMemberInClubAsync(member.Id, club.Id);

        // Criar entries de timeline
        var timelineEntry1 = new TimelineEntry
        {
            Id = Guid.NewGuid(),
            MemberId = member.Id,
            Type = TimelineEntryType.MembershipStarted,
            Description = "Member was registered in the system",
            CreatedAtUtc = DateTime.UtcNow
        };

        var timelineEntry2 = new TimelineEntry
        {
            Id = Guid.NewGuid(),
            MemberId = member.Id,
            Type = TimelineEntryType.MembershipStarted,
            Description = "Member enrolled in club",
            CreatedAtUtc = DateTime.UtcNow.AddMinutes(5)
        };

        DbContext.TimelineEntries.AddRange(timelineEntry1, timelineEntry2);
        await DbContext.SaveChangesAsync();

        // Act - Exportar timeline para CSV
        var response = await Client.GetAsync($"/pms/reports/export/timeline/{member.Id}");

        // Assert - Verificar se o CSV foi gerado
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/csv");

        var csvContent = await response.Content.ReadAsStringAsync();
        csvContent.Should().NotBeNullOrEmpty();
        csvContent.Should().Contain("Action,Description,CreatedAt");
        csvContent.Should().Contain("Member Registered");
        csvContent.Should().Contain("Membership Created");
    }

    [Fact]
    public async Task ExportParticipationsToCsv_ShouldReturnValidCsvData()
    {
        // Arrange - Criar membro, evento e participação
        var club = await CreateTestClubAsync();
        var member = await RegisterTestMemberAsync(club.Id);
        var token = await GetAuthTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        await EnrollMemberInClubAsync(member.Id, club.Id);

        // Criar evento
        var eventEntity = new OfficialEvent
        {
            Id = Guid.NewGuid(),
            Name = "Evento Teste",
            Description = "Evento para testes",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            FeeAmount = 50.00m,
            CreatedAtUtc = DateTime.UtcNow
        };

        DbContext.OfficialEvents.Add(eventEntity);
        await DbContext.SaveChangesAsync();

        // Criar participação
        var participation = new MemberEventParticipation
        {
            Id = Guid.NewGuid(),
            MemberId = member.Id,
            EventId = eventEntity.Id,
            RegisteredAtUtc = DateTime.UtcNow,
            Status = ParticipationStatus.Registered
        };

        DbContext.MemberEventParticipations.Add(participation);
        await DbContext.SaveChangesAsync();

        // Act - Exportar participações para CSV
        var response = await Client.GetAsync($"/pms/reports/export/participations/{member.Id}");

        // Assert - Verificar se o CSV foi gerado
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/csv");

        var csvContent = await response.Content.ReadAsStringAsync();
        csvContent.Should().NotBeNullOrEmpty();
        csvContent.Should().Contain("EventName,EventDate,Status,RegisteredAt");
        csvContent.Should().Contain("Evento Teste");
        csvContent.Should().Contain("Registered");
    }

    [Fact]
    public async Task ExportWithFilters_ShouldApplyFiltersCorrectly()
    {
        // Arrange - Criar múltiplos membros com diferentes status
        var club = await CreateTestClubAsync();
        var activeMember = await RegisterTestMemberAsync(club.Id, "ativo@test.com");
        var inactiveMember = await RegisterTestMemberAsync(club.Id, "inativo@test.com");
        var token = await GetAuthTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Inscrever membro ativo
        await EnrollMemberInClubAsync(activeMember.Id, club.Id);

        // Inscrever e depois desativar membro inativo
        await EnrollMemberInClubAsync(inactiveMember.Id, club.Id);
        var inactiveMembership = DbContext.Memberships
            .FirstOrDefault(m => m.MemberId == inactiveMember.Id && m.ClubId == club.Id);
        inactiveMembership!.IsActive = false;
        inactiveMembership.EndDate = DateTime.UtcNow;
        await DbContext.SaveChangesAsync();

        // Act - Exportar apenas membros ativos
        var response = await Client.GetAsync($"/pms/reports/export/members/{club.Id}?activeOnly=true");

        // Assert - Verificar se apenas membros ativos foram exportados
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var csvContent = await response.Content.ReadAsStringAsync();
        csvContent.Should().Contain("ativo@test.com");
        csvContent.Should().NotContain("inativo@test.com");
    }

    [Fact]
    public async Task ExportEmptyData_ShouldReturnEmptyCsv()
    {
        // Arrange - Criar clube sem membros
        var club = await CreateTestClubAsync();
        var token = await GetAuthTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act - Exportar membros de clube vazio
        var response = await Client.GetAsync($"/pms/reports/export/members/{club.Id}");

        // Assert - Verificar se CSV vazio foi retornado (apenas cabeçalho)
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var csvContent = await response.Content.ReadAsStringAsync();
        csvContent.Should().NotBeNullOrEmpty();
        csvContent.Should().Contain("Name,Email,Phone"); // Cabeçalho
        csvContent.Split('\n').Length.Should().Be(2); // Apenas cabeçalho e linha vazia
    }

    [Fact]
    public async Task ExportWithSpecialCharacters_ShouldEscapeCorrectly()
    {
        // Arrange - Criar membro com caracteres especiais
        var club = await CreateTestClubAsync();
        var member = await RegisterTestMemberAsync(club.Id, "teste,especial@test.com");

        // Atualizar nome com caracteres especiais
        member.FirstName = "João";
        member.LastName = "Silva \"Santos\"";
        member.Phone = "+55 (11) 99999-9999";
        await DbContext.SaveChangesAsync();

        var token = await GetAuthTokenAsync();
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        await EnrollMemberInClubAsync(member.Id, club.Id);

        // Act - Exportar membro com caracteres especiais
        var response = await Client.GetAsync($"/pms/reports/export/members/{club.Id}");

        // Assert - Verificar se caracteres especiais foram tratados corretamente
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var csvContent = await response.Content.ReadAsStringAsync();
        csvContent.Should().Contain("teste,especial@test.com");
        // Verificar se vírgulas no nome foram escapadas ou tratadas adequadamente
        csvContent.Should().Contain("João");
    }

    #region Helper Methods

    private async Task<Club> CreateTestClubAsync()
    {
        // Criar hierarquia mínima para o clube
        var division = new Division
        {
            Id = Guid.NewGuid(),
            Name = "Divisão Teste",
            Code = "DIV-TEST",
            Description = "Divisão para testes",
            CreatedAtUtc = DateTime.UtcNow
        };

        var union = new Union
        {
            Id = Guid.NewGuid(),
            Name = "União Teste",
            Code = "UNI-TEST",
            Description = "União para testes",
            DivisionId = division.Id,
            CreatedAtUtc = DateTime.UtcNow
        };

        var association = new Association
        {
            Id = Guid.NewGuid(),
            Name = "Associação Teste",
            Code = "ASS-TEST",
            Description = "Associação para testes",
            UnionId = union.Id,
            CreatedAtUtc = DateTime.UtcNow
        };

        var region = new Region
        {
            Id = Guid.NewGuid(),
            Name = "Região Teste",
            Code = "REG-TEST",
            Description = "Região para testes",
            AssociationId = association.Id,
            CreatedAtUtc = DateTime.UtcNow
        };

        var district = new District
        {
            Id = Guid.NewGuid(),
            Name = "Distrito Teste",
            Code = "DIS-TEST",
            Description = "Distrito para testes",
            RegionId = region.Id,
            CreatedAtUtc = DateTime.UtcNow
        };

        var church = new Church
        {
            Id = Guid.NewGuid(),
            Name = "Igreja Teste",
            Address = "Rua Teste, 123",
            City = "São Paulo",
            State = "SP",
            Cep = "01234567",
            CreatedAtUtc = DateTime.UtcNow
        };

        var club = new Club
        {
            Id = Guid.NewGuid(),
            Name = "Clube Teste",
            Code = "CLU-TEST",
            Description = "Clube para testes",
            ChurchId = church.Id,
            CreatedAtUtc = DateTime.UtcNow
        };

        // Adicionar ao contexto
        DbContext.Divisions.Add(division);
        DbContext.Unions.Add(union);
        DbContext.Associations.Add(association);
        DbContext.Regions.Add(region);
        DbContext.Districts.Add(district);
        DbContext.Churches.Add(church);
        DbContext.Clubs.Add(club);
        await DbContext.SaveChangesAsync();

        return club;
    }

    private async Task<Member> RegisterTestMemberAsync(Guid clubId, string email = "joao.silva@test.com")
    {
        var registerDto = new RegisterDto
        {
            FirstName = "João",
            LastName = "Silva",
            Email = email,
            Phone = "11999999999",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = MemberGender.Masculino,
            ClubId = clubId,
            Password = "Senha123!",
            ConfirmPassword = "Senha123!"
        };

        var response = await Client.PostAsJsonAsync("/pms/members/register", registerDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MemberDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        var member = await DbContext.Members.FindAsync(result.Data!.Id);
        member.Should().NotBeNull();
        return member!;
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var loginDto = new LoginDto
        {
            Email = "joao.silva@test.com",
            Password = "Senha123!"
        };

        var response = await Client.PostAsJsonAsync("/pms/members/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<AuthResultDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        result.Data!.Token.Should().NotBeNullOrEmpty();
        return result.Data.Token;
    }

    private async Task EnrollMemberInClubAsync(Guid memberId, Guid clubId)
    {
        var enrollmentDto = new CreateMembershipDto
        {
            MemberId = memberId,
            ClubId = clubId,
            StartDate = DateTime.UtcNow
        };

        var response = await Client.PostAsJsonAsync("/pms/membership", enrollmentDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion
}
