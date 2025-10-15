namespace Models;

public class TempQcPo
{
    public int Id { get; set; }
    public string PoNumber { get; set; } = string.Empty;
    public DateTime UnloadDate { get; set; }
    public string Vendor { get; set; } = string.Empty;
    public List<TempQcProduct> Products { get; set; } = new();
}