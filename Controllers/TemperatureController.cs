using Microsoft.AspNetCore.Mvc;
using Services;
using Models.DTOs;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController : ControllerBase
{
    private readonly ITemperatureService _service;

    public TemperatureController(ITemperatureService service)
    {
        _service = service;
    }

    [HttpPost("validate")]
    public async Task<ActionResult<TemperatureValidationResult>> ValidateTemperature([FromBody] TemperatureReading reading, CancellationToken ct)
    {
        if (reading is null) return BadRequest("Reading payload required.");

        var result = await _service.CheckThresholdAsync(reading, ct);
        return Ok(result);
    }
}