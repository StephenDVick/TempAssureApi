namespace Models.DTOs;

// Adjust fields to match APEX response
public class ApexLoadDto
{
    public string PoNumber { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
    public DateTime? EstimatedArrival { get; set; }
    public List<string> Skus { get; set; } = new();
}