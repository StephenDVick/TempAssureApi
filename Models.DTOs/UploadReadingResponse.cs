namespace Models.DTOs;

public class UploadReadingResponse
{
    public int ProductId { get; set; }
    public TemperatureValidationResult Validation { get; set; } = null!;
}