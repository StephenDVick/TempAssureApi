using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Models.DTOs;
using Models.Options;

namespace Services;

public interface IApexClient
{
    Task<ApexLoadDto?> GetLoadAsync(string poNumber, CancellationToken ct = default);
}

public class ApexClient(HttpClient http, IOptions<ApexOptions> options) : IApexClient
{
    private readonly HttpClient _http = http;
    private readonly ApexOptions _opts = options.Value;

    public async Task<ApexLoadDto?> GetLoadAsync(string poNumber, CancellationToken ct = default)
    {
        if (_http.BaseAddress is null && !string.IsNullOrWhiteSpace(_opts.BaseUrl))
            _http.BaseAddress = new Uri(_opts.BaseUrl);

        if (!string.IsNullOrWhiteSpace(_opts.ApiKey) && !_http.DefaultRequestHeaders.Contains("X-API-KEY"))
            _http.DefaultRequestHeaders.Add("X-API-KEY", _opts.ApiKey);

        // Example endpoint path; adjust as needed.
        return await _http.GetFromJsonAsync<ApexLoadDto>($"loads/{poNumber}", ct);
    }
}