namespace Models;

public class TempQcUnloadedBy
{
    public int Id { get; set; }
    public int TempQcPoId { get; set; }
    public TempQcPo? TempQcPo { get; set; }
    public string UnloadedBy { get; set; } = string.Empty;
    public DateTime UnloadedAt { get; set; }
}