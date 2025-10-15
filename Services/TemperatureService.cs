using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;

namespace Services;

public interface ITemperatureService
{
    Task<TemperatureValidationResult> CheckThresholdAsync(TemperatureReading reading, CancellationToken ct = default);
}

public class TemperatureService(AppDbContext db, ILogger<TemperatureService> logger) : ITemperatureService
{
    private readonly AppDbContext _db = db;
    private readonly ILogger<TemperatureService> _logger = logger;

    public async Task<TemperatureValidationResult> CheckThresholdAsync(TemperatureReading reading, CancellationToken ct = default)
    {
        var position = NormalizePosition(reading.Position);
        var result = new TemperatureValidationResult
        {
            ProductId = reading.ProductId
        };

        // Optional: update product temperature if ProductId provided
        if (reading.ProductId is int pid)
        {
            var product = await _db.TempQcProducts.FirstOrDefaultAsync(p => p.Id == pid, ct);
            if (product is not null)
            {
                product.Temperature = reading.Temperature;
                if (!string.IsNullOrWhiteSpace(reading.Position)) product.Position = position;
                if (!string.IsNullOrWhiteSpace(reading.Sku)) product.Sku = reading.Sku;
                await _db.SaveChangesAsync(ct);
            }
        }

        var threshold = await _db.VendorThresholds
            .AsNoTracking()
            .FirstOrDefaultAsync(t =>
                t.Vendor == reading.Vendor &&
                t.Sku == reading.Sku &&
                t.Position == position, ct);

        if (threshold is null)
        {
            result.Compliant = false;
            result.Deviations.Add($"No threshold found for Vendor={reading.Vendor}, SKU={reading.Sku}, Position={position}.");
            return result;
        }

        result.Threshold = new ThresholdInfo(threshold.MinTemp, threshold.MaxTemp, threshold.Vendor, threshold.Sku, threshold.Position);

        if (reading.Temperature < threshold.MinTemp || reading.Temperature > threshold.MaxTemp)
        {
            result.Compliant = false;
            result.Deviations.Add($"Temperature {reading.Temperature} outside [{threshold.MinTemp}, {threshold.MaxTemp}] for {position}.");
        }
        else
        {
            result.Compliant = true;
        }

        return result;

        static string NormalizePosition(string pos) =>
            string.IsNullOrWhiteSpace(pos) ? "" :
            char.ToUpperInvariant(pos[0]) + (pos.Length > 1 ? pos[1..].ToLowerInvariant() : "");
    }
}