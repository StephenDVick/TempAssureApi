namespace Models.DTOs;

public class TemperatureReading
{
    public int? ProductId { get; set; } // Optional: to update an existing product record
    public int PoId { get; set; }
    public string Vendor { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty; // Top, Middle, Bottom
    public double Temperature { get; set; }
    public string? ProbeId { get; set; }
    public DateTime? TimestampUtc { get; set; }
}