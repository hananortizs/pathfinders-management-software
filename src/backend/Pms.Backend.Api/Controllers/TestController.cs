using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller de teste para verificar middleware
/// </summary>
[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    /// <summary>
    /// Endpoint público de teste
    /// </summary>
    /// <returns>Mensagem de teste</returns>
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult GetPublic()
    {
        return Ok(new { message = "Endpoint público funcionando", timestamp = DateTime.UtcNow });
    }

    /// <summary>
    /// Endpoint protegido de teste
    /// </summary>
    /// <returns>Mensagem de teste</returns>
    [HttpGet("protected")]
    public IActionResult GetProtected()
    {
        return Ok(new { message = "Endpoint protegido funcionando", timestamp = DateTime.UtcNow });
    }
}
