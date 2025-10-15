using Microsoft.AspNetCore.Mvc;
using Services;
using Models.DTOs;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController(ITemperatureService service) : ControllerBase
{
    private readonly ITemperatureService _service = service;

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateTemperature([FromBody] TemperatureReading reading, CancellationToken ct)
    {
        if (reading is null) return BadRequest("Reading payload required.");

        var result = await _service.CheckThresholdAsync(reading, ct);

        // Todo: If not compliant, you could kick off an override/notification flow here.
        return Ok(new
        {
            Compliant = result.Compliant,
            Deviations = result.Deviations,
            ProductId = result.ProductId,
            Threshold = result.Threshold
        });
    }
}