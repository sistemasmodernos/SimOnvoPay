using Microsoft.AspNetCore.Mvc;
using SimOnvoPay.Api.DTOs;
using SimOnvoPay.Api.Services;

namespace SimOnvoPay.Api.Controllers;

[ApiController]
[Route("api/v1/checkout")]
public class CheckoutController(IPaymentSessionService sessionService) : ControllerBase
{
    /// <summary>Retorna la info pública de una sesión para el frontend.</summary>
    [HttpGet("{token}")]
    public async Task<IActionResult> GetInfo(string token)
    {
        var info = await sessionService.GetCheckoutInfoAsync(token);
        return info is null ? NotFound(new { error = "Sesión no encontrada o expirada." }) : Ok(info);
    }

    /// <summary>Ejecuta el pago para una sesión.</summary>
    [HttpPost("{token}/pay")]
    public async Task<IActionResult> Pay(string token, [FromBody] ProcessPaymentRequest request)
    {
        var result = await sessionService.ProcessPaymentAsync(token, request);
        var statusCode = result.Status == "failed" ? 422 : 200;
        return StatusCode(statusCode, result);
    }

    /// <summary>Cancela una sesión de pago.</summary>
    [HttpPost("{token}/cancel")]
    public async Task<IActionResult> Cancel(string token)
    {
        var cancelled = await sessionService.CancelSessionAsync(token);
        return cancelled
            ? Ok(new { status = "cancelled" })
            : BadRequest(new { error = "No se puede cancelar la sesión en su estado actual." });
    }

    /// <summary>Consulta el estado de una sesión (para polling del frontend).</summary>
    [HttpGet("{token}/status")]
    public async Task<IActionResult> Status(string token)
    {
        var status = await sessionService.GetSessionStatusAsync(token);
        if (status is null) return NotFound(new { error = "Sesión no encontrada." });

        return Ok(new
        {
            status = status.Status,
            message = status.ErrorMessage
        });
    }
}
