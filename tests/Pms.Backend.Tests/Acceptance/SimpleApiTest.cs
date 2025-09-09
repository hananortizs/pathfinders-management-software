using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
/// Teste simples para verificar se a API está funcionando
/// </summary>
public class SimpleApiTest : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly PmsDbContext _dbContext;

    public SimpleApiTest(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _dbContext = factory.Services.GetRequiredService<PmsDbContext>();
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MemberInvite_ShouldReturnNotFound_WhenClubDoesNotExist()
    {
        // Arrange
        var inviteDto = new InviteMemberRequestDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com",
            Phone = "11999999999",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = MemberGender.Masculino,
            ClubId = Guid.NewGuid() // Club que não existe
        };

        // Act
        var response = await _client.PostAsJsonAsync("/member/invite", inviteDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK); // A API retorna 200 mas com erro no body
        var result = await response.Content.ReadFromJsonAsync<BaseResponse<bool>>();
        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeFalse();
        result.Message.Should().Contain("Club not found");
    }
}

