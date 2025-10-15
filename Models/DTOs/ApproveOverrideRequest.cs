using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class ApproveOverrideRequest
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Required]
    [MinLength(4)]
    public string Pin { get; set; } = string.Empty;

    public string? Reason { get; set; }
}