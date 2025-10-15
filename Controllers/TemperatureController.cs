using Microsoft.AspNetCore.Mvc;
using Services;
using Models.DTOs;
using Data;
using Models;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController : ControllerBase
{
    private readonly ITemperatureService _service;
    private readonly AppDbContext _db;

    public TemperatureController(ITemperatureService service, AppDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpPost("validate")]
    public async Task<ActionResult<TemperatureValidationResult>> ValidateTemperature([FromBody] TemperatureReading reading, CancellationToken ct)
    {
        if (reading is null) return BadRequest("Reading payload required.");

        var result = await _service.CheckThresholdAsync(reading, ct);
        return Ok(result);
    }

    [HttpPost("readings")]
    public async Task<IActionResult> UploadReading([FromBody] TemperatureReading reading, CancellationToken ct)
    {
        if (reading is null) return BadRequest("Reading payload required.");

        var po = await _db.TempQcPos.FindAsync(reading.PoId, ct);
        if (po is null) return NotFound("PO not found.");

        var validation = await _service.CheckThresholdAsync(reading, ct);

        var product = new TempQcProduct
        {
            Sku = reading.Sku,
            Temperature = reading.Temperature,
            Position = reading.Position,
            TempQcPoId = reading.PoId,
            ApprovedDeviation = false
        };

        _db.TempQcProducts.Add(product);
        await _db.SaveChangesAsync(ct);

        return Ok(new UploadReadingResponse { ProductId = product.Id, Validation = validation });
    }
}