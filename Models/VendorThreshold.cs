namespace Models;

public class VendorThreshold
{
    public int Id { get; set; }
    public string Vendor { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty; // Top, Middle, Bottom
    public double MinTemp { get; set; }
    public double MaxTemp { get; set; }
}