namespace Models;

public class TempQcProduct
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public double Temperature { get; set; } // Captured from probe
    public string Position { get; set; } = string.Empty; // Top, Middle, Bottom
    public bool ApprovedDeviation { get; set; }

    // Relationship (FK) back to TempQcPo
    public int TempQcPoId { get; set; }
    public TempQcPo? TempQcPo { get; set; }
}