using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.Helpers;
using Pms.Backend.Domain.Exceptions;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for testing exception handling
/// </summary>
[ApiController]
[Route("[controller]")]
public class TestExceptionController : ControllerBase
{
    /// <summary>
    /// Tests validation exception
    /// </summary>
    /// <param name="fieldName">Field name</param>
    /// <param name="value">Field value</param>
    /// <returns>Test result</returns>
    [HttpGet("validation")]
    public IActionResult TestValidationException(string fieldName = "TestField", string value = "invalid")
    {
        ExceptionHelper.ThrowValidationException(fieldName, $"Valor '{value}' é inválido", value);
        return Ok(); // This will never be reached
    }

    /// <summary>
    /// Tests not found exception
    /// </summary>
    /// <param name="id">Resource ID</param>
    /// <returns>Test result</returns>
    [HttpGet("not-found")]
    public IActionResult TestNotFoundException(Guid id = default)
    {
        if (id == default)
            id = Guid.NewGuid();

        ExceptionHelper.ThrowNotFoundException("TestResource", id, "Recurso de teste não encontrado");
        return Ok(); // This will never be reached
    }

    /// <summary>
    /// Tests business rule exception
    /// </summary>
    /// <param name="rule">Business rule</param>
    /// <returns>Test result</returns>
    [HttpGet("business-rule")]
    public IActionResult TestBusinessRuleException(string rule = "TestRule")
    {
        ExceptionHelper.ThrowBusinessRuleException(rule, "Regra de negócio violada",
            new Dictionary<string, object> { ["Details"] = "Detalhes adicionais" });
        return Ok(); // This will never be reached
    }

    /// <summary>
    /// Tests duplicate exception
    /// </summary>
    /// <param name="fieldName">Field name</param>
    /// <param name="value">Field value</param>
    /// <returns>Test result</returns>
    [HttpGet("duplicate")]
    public IActionResult TestDuplicateException(string fieldName = "Code", string value = "DUPLICATE")
    {
        ExceptionHelper.ThrowDuplicateException("TestResource", fieldName, value, "Recurso duplicado");
        return Ok(); // This will never be reached
    }

    /// <summary>
    /// Tests argument exception
    /// </summary>
    /// <param name="paramName">Parameter name</param>
    /// <returns>Test result</returns>
    [HttpGet("argument")]
    public IActionResult TestArgumentException(string paramName = "testParam")
    {
        ExceptionHelper.ThrowArgumentException(paramName, "Parâmetro inválido");
        return Ok(); // This will never be reached
    }

    /// <summary>
    /// Tests unauthorized exception
    /// </summary>
    /// <returns>Test result</returns>
    [HttpGet("unauthorized")]
    public IActionResult TestUnauthorizedException()
    {
        ExceptionHelper.ThrowUnauthorizedException("Acesso negado para teste");
        return Ok(); // This will never be reached
    }

    /// <summary>
    /// Tests not implemented exception
    /// </summary>
    /// <returns>Test result</returns>
    [HttpGet("not-implemented")]
    public IActionResult TestNotImplementedException()
    {
        ExceptionHelper.ThrowNotImplementedException("Funcionalidade de teste não implementada");
        return Ok(); // This will never be reached
    }

    /// <summary>
    /// Tests generic exception
    /// </summary>
    /// <returns>Test result</returns>
    [HttpGet("generic")]
    public IActionResult TestGenericException()
    {
        throw new InvalidOperationException("Exceção genérica para teste");
    }
}
