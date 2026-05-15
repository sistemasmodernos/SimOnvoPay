using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SimOnvoPay.Api.Services;
using SimOnvoPay.Webhooks;

namespace SimOnvoPay.Api.Controllers;

[ApiController]
[Route("api/v1/webhooks")]
public class WebhooksController(IPaymentSessionService sessionService, ILogger<WebhooksController> logger) : ControllerBase
{
    [HttpPost("onvopay")]
    public async Task<IActionResult> HandleOnvoPay([FromBody] OnvoPayWebhookEvent webhookEvent)
    {
        logger.LogInformation("Webhook recibido: {Type} - {Id}", webhookEvent.Type, webhookEvent.Id);

        try
        {
            // Extraer el paymentIntentId del data del webhook
            string? paymentIntentId = null;
            string? intentStatus = null;

            if (webhookEvent.Data.ValueKind == JsonValueKind.Object)
            {
                if (webhookEvent.Data.TryGetProperty("id", out var idProp))
                    paymentIntentId = idProp.GetString();

                if (webhookEvent.Data.TryGetProperty("status", out var statusProp))
                    intentStatus = statusProp.GetString();

                // Para eventos de payment-intent, el objeto raíz es el payment intent
                // Para otros eventos, puede estar anidado en data.object
                if (paymentIntentId is null && webhookEvent.Data.TryGetProperty("object", out var objProp))
                {
                    if (objProp.TryGetProperty("id", out var nestedId))
                        paymentIntentId = nestedId.GetString();
                    if (objProp.TryGetProperty("status", out var nestedStatus))
                        intentStatus = nestedStatus.GetString();
                }
            }

            if (paymentIntentId is null)
            {
                logger.LogWarning("Webhook {Type} sin paymentIntentId extraíble.", webhookEvent.Type);
                return Ok(new { received = true });
            }

            await sessionService.ProcessWebhookAsync(webhookEvent.Type, paymentIntentId, intentStatus);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error procesando webhook {Type}", webhookEvent.Type);
        }

        // Siempre responder 200 para que OnvoPay no reintente
        return Ok(new { received = true });
    }
}
