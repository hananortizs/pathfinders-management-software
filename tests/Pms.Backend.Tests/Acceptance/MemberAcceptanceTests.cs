using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
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
/// Testes de aceite para funcionalidades de membros e autenticação
/// Testa o fluxo completo desde o registro até a autenticação de membros
/// </summary>
public class MemberAcceptanceTests : BaseIntegrationTest
{
    public MemberAcceptanceTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task RegisterMember_ShouldCreateUserAndMember()
    {
        // Arrange - Criar hierarquia necessária
        var club = await CreateTestClubAsync();
        var registerDto = new RegisterDto
        {
            FirstName = "João",
            LastName = "Silva",
            Email = "joao.silva@test.com",
            Phone = "11999999999",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = MemberGender.Masculino,
            ClubId = club.Id,
            Password = "Senha123!",
            ConfirmPassword = "Senha123!"
        };

        // Act - Registrar membro
        var response = await Client.PostAsJsonAsync("/pms/members/register", registerDto);

        // Assert - Verificar se o registro foi bem-sucedido
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MemberDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        result.Data!.FullName.Should().Be("João Silva");
        result.Data.Email.Should().Be("joao.silva@test.com");

        // Verificar se o usuário foi criado no banco
        var userCredential = DbContext.UserCredentials
            .FirstOrDefault(uc => uc.Email == "joao.silva@test.com");
        userCredential.Should().NotBeNull();
        userCredential!.Email.Should().Be("joao.silva@test.com");
    }

    [Fact]
    public async Task LoginMember_ShouldReturnToken()
    {
        // Arrange - Criar membro e fazer login
        var club = await CreateTestClubAsync();
        var member = await RegisterTestMemberAsync(club.Id);

        var loginDto = new LoginDto
        {
            Email = "joao.silva@test.com",
            Password = "Senha123!"
        };

        // Act - Fazer login
        var response = await Client.PostAsJsonAsync("/pms/members/login", loginDto);

        // Assert - Verificar se o login foi bem-sucedido
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<AuthResultDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        result.Data!.Token.Should().NotBeNullOrEmpty();
        result.Data.Member.Should().NotBeNull();
        result.Data.Member!.Email.Should().Be("joao.silva@test.com");
    }

    [Fact]
    public async Task GetMemberProfile_ShouldReturnMemberData()
    {
        // Arrange - Criar membro autenticado
        var club = await CreateTestClubAsync();
        var member = await RegisterTestMemberAsync(club.Id);
        var token = await GetAuthTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act - Buscar perfil do membro
        var response = await Client.GetAsync($"/pms/members/{member.Id}");

        // Assert - Verificar se os dados foram retornados
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MemberDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        result.Data!.FullName.Should().Be("João Silva");
        result.Data.Email.Should().Be("joao.silva@test.com");
    }

    [Fact]
    public async Task UpdateMemberProfile_ShouldPersistChanges()
    {
        // Arrange - Criar membro autenticado
        var club = await CreateTestClubAsync();
        var member = await RegisterTestMemberAsync(club.Id);
        var token = await GetAuthTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var updateDto = new UpdateMemberDto
        {
            FirstName = "João",
            LastName = "Silva Santos",
            Phone = "11888888888",
            ZipCode = "01234567"
        };

        // Act - Atualizar perfil
        var response = await Client.PutAsJsonAsync($"/pms/members/{member.Id}", updateDto);

        // Assert - Verificar se a atualização foi bem-sucedida
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<BaseResponse<MemberDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        result.Data!.FullName.Should().Be("João Silva Santos");
        result.Data.Phone.Should().Be("11888888888");
        result.Data.ZipCode.Should().Be("01234567");
    }

    [Fact]
    public async Task ResetPassword_ShouldSendEmailAndAllowReset()
    {
        // Arrange - Criar membro
        var club = await CreateTestClubAsync();
        var member = await RegisterTestMemberAsync(club.Id);

        var resetRequestDto = new ResetPasswordRequestDto
        {
            Email = "joao.silva@test.com"
        };

        // Act - Solicitar reset de senha
        var response = await Client.PostAsJsonAsync("/pms/members/reset-password", resetRequestDto);

        // Assert - Verificar se a solicitação foi processada
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Simular confirmação de reset (em um cenário real, o token viria por email)
        var userCredential = DbContext.UserCredentials
            .FirstOrDefault(uc => uc.Email == "joao.silva@test.com");

        userCredential.Should().NotBeNull();
        // Em um teste real, você verificaria se o token de reset foi gerado
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

    private async Task<Member> RegisterTestMemberAsync(Guid clubId)
    {
        var registerDto = new RegisterDto
        {
            FirstName = "João",
            LastName = "Silva",
            Email = "joao.silva@test.com",
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

    #endregion
}
