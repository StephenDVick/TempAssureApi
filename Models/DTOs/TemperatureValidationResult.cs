namespace Models.DTOs;

public record ThresholdInfo(double Min, double Max, string Vendor, string Sku, string Position);

public class TemperatureValidationResult
{
    public bool Compliant { get; set; }
    public List<string> Deviations { get; } = new();
    public int? ProductId { get; set; }
    public ThresholdInfo? Threshold { get; set; }
}