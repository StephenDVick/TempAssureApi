namespace Models;

public class OverrideApproval
{
    public int Id { get; set; }
    public int TempQcProductId { get; set; }
    public TempQcProduct? TempQcProduct { get; set; }
    public DateTime ApprovedAt { get; set; }
    public string PinLast4 { get; set; } = string.Empty; // Do not store full PINs
    public string? Reason { get; set; }
}