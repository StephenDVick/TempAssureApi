using Microsoft.AspNetCore.Mvc;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db;

    [HttpGet("po/{poId:int}")]
    public async Task<IActionResult> GetPoReport([FromRoute] int poId, CancellationToken ct)
    {
        var po = await _db.TempQcPos
            .Include(p => p.Products)
            .FirstOrDefaultAsync(p => p.Id == poId, ct);

        if (po is null) return NotFound("PO not found.");

        // Fetch thresholds for this vendor in one shot
        var thresholds = await _db.VendorThresholds
            .Where(t => t.Vendor == po.Vendor)
            .ToListAsync(ct);

        var thresholdMap = thresholds
            .GroupBy(t => (t.Sku, Position: NormalizePosition(t.Position)))
            .ToDictionary(g => g.Key, g => g.First());

        int rejected = 0, overridden = 0, compliant = 0;

        var rejectedBySku = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var prod in po.Products)
        {
            var key = (prod.Sku, Position: NormalizePosition(prod.Position));
            var hasThreshold = thresholdMap.TryGetValue(key, out var th);

            bool inRange = hasThreshold && prod.Temperature >= th!.MinTemp && prod.Temperature <= th!.MaxTemp;

            if (inRange)
            {
                compliant++;
                continue;
            }

            if (prod.ApprovedDeviation)
            {
                overridden++;
            }
            else
            {
                rejected++;
                if (!rejectedBySku.TryAdd(prod.Sku, 1))
                    rejectedBySku[prod.Sku]++;
            }
        }

        return Ok(new
        {
            Po = new { po.Id, po.PoNumber, po.Vendor, po.UnloadDate },
            Totals = new { Products = po.Products.Count, Compliant = compliant, Overridden = overridden, Rejected = rejected },
            RejectedSkus = rejectedBySku.Select(kvp => new { Sku = kvp.Key, Count = kvp.Value })
        });

        static string NormalizePosition(string pos) =>
            string.IsNullOrWhiteSpace(pos) ? "" :
            char.ToUpperInvariant(pos[0]) + (pos.Length > 1 ? pos[1..].ToLowerInvariant() : "");
    }
}