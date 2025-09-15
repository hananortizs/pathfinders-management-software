using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Classe base para Controllers com métodos auxiliares padronizados
/// Centraliza a lógica de resposta para evitar duplicação de código
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Processa uma resposta de serviço e retorna o ActionResult apropriado
    /// </summary>
    /// <typeparam name="T">Tipo de dados da resposta</typeparam>
    /// <param name="result">Resultado do serviço</param>
    /// <param name="successStatusCode">Status code para sucesso (padrão: 200)</param>
    /// <returns>ActionResult apropriado</returns>
    protected IActionResult ProcessResponse<T>(BaseResponse<T> result, int successStatusCode = StatusCodes.Status200OK)
    {
        if (!result.IsSuccess)
        {
            return result.StatusCode switch
            {
                400 => BadRequest(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                404 => NotFound(result),
                422 => UnprocessableEntity(result),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        return successStatusCode switch
        {
            201 => Created("", result),
            204 => NoContent(),
            _ => Ok(result)
        };
    }

    /// <summary>
    /// Processa uma resposta de serviço e retorna o ActionResult apropriado com CreatedAtAction
    /// </summary>
    /// <typeparam name="T">Tipo de dados da resposta</typeparam>
    /// <param name="result">Resultado do serviço</param>
    /// <param name="actionName">Nome da ação para CreatedAtAction</param>
    /// <param name="routeValues">Valores da rota para CreatedAtAction</param>
    /// <returns>ActionResult apropriado</returns>
    protected IActionResult ProcessResponseWithCreatedAtAction<T>(
        BaseResponse<T> result,
        string actionName,
        object? routeValues = null)
    {
        if (!result.IsSuccess)
        {
            return result.StatusCode switch
            {
                400 => BadRequest(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                404 => NotFound(result),
                422 => UnprocessableEntity(result),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        return CreatedAtAction(actionName, routeValues, result);
    }

    /// <summary>
    /// Processa uma resposta de serviço e retorna o ActionResult apropriado para operações de arquivo
    /// </summary>
    /// <param name="result">Resultado do serviço</param>
    /// <param name="fileBytes">Bytes do arquivo</param>
    /// <param name="contentType">Tipo de conteúdo do arquivo</param>
    /// <param name="fileName">Nome do arquivo</param>
    /// <returns>ActionResult apropriado</returns>
    protected IActionResult ProcessFileResponse<T>(
        BaseResponse<T> result,
        byte[] fileBytes,
        string contentType,
        string fileName)
    {
        if (!result.IsSuccess)
        {
            return result.StatusCode switch
            {
                400 => BadRequest(result),
                401 => Unauthorized(result),
                403 => Forbid(),
                404 => NotFound(result),
                422 => UnprocessableEntity(result),
                500 => StatusCode(500, result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        return File(fileBytes, contentType, fileName);
    }
}
