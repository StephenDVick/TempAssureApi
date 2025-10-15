using Microsoft.Extensions.Configuration;

namespace Services;

public interface IPinValidator
{
    bool Validate(string pin);
    string GetLast4(string pin);
}

public class PinValidator(IConfiguration config) : IPinValidator
{
    private readonly IConfiguration _config = config;

    public bool Validate(string pin)
    {
        var allowed = _config.GetSection("OverridePins").Get<string[]>() ?? [];
        return allowed.Contains(pin);
    }

    public string GetLast4(string pin) => (pin ?? string.Empty) switch
    {
        { Length: >= 4 } => pin[^4..],
        _ => pin
    };
}