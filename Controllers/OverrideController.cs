using Microsoft.AspNetCore.Mvc;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Services;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class OverrideController(AppDbContext db, IPinValidator pinValidator) : ControllerBase
{
    private readonly AppDbContext _db = db;
    private readonly IPinValidator _pinValidator = pinValidator;

    [HttpPost("approve")]
    public async Task<IActionResult> Approve([FromBody] ApproveOverrideRequest request, CancellationToken ct)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Pin))
            return BadRequest("PIN required.");

        if (!_pinValidator.Validate(request.Pin))
            return Unauthorized("Invalid PIN.");

        var product = await _db.TempQcProducts
            .Include(p => p.TempQcPo)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);

        if (product is null) return NotFound("Product not found.");

        product.ApprovedDeviation = true;

        var approval = new OverrideApproval
        {
            TempQcProductId = product.Id,
            ApprovedAt = DateTime.UtcNow,
            PinLast4 = _pinValidator.GetLast4(request.Pin),
            Reason = request.Reason
        };

        _db.OverrideApprovals.Add(approval);
        await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            Approved = true,
            ProductId = product.Id,
            PoId = product.TempQcPoId,
            Sku = product.Sku
        });
    }
}