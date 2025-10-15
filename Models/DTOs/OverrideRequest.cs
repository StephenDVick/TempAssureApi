namespace Models.DTOs;

public class OverrideRequest
{
    public int ProductId { get; set; }
    public string Pin { get; set; } = string.Empty;
    public string? Reason { get; set; }
}