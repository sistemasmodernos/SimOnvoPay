using Microsoft.AspNetCore.Mvc;
using SimOnvoPay.Api.DTOs;
using SimOnvoPay.Api.Services;

namespace SimOnvoPay.Api.Controllers;

[ApiController]
[Route("api/v1/sessions")]
public class SessionsController(IPaymentSessionService sessionService, IConfiguration config) : ControllerBase
{
    /// <summary>Crea una nueva sesión de pago. Requiere X-API-Key.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSessionRequest request)
    {
        var frontendUrl = config["AppSettings:FrontendUrl"]
            ?? throw new InvalidOperationException("FrontendUrl no configurado.");

        var result = await sessionService.CreateSessionAsync(request, frontendUrl);
        return Created($"/api/v1/sessions/{result.Token}", result);
    }

    /// <summary>Consulta el estado de una sesión. Requiere X-API-Key.</summary>
    [HttpGet("{token}")]
    public async Task<IActionResult> GetStatus(string token)
    {
        var result = await sessionService.GetSessionStatusAsync(token);
        return result is null ? NotFound(new { error = "Sesión no encontrada." }) : Ok(result);
    }
}
