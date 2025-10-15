namespace Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Models.DTOs;

    public interface ITemperatureService
    {
        Task<TemperatureValidationResult> CheckThresholdAsync(TemperatureReading reading, CancellationToken cancellationToken);
    }
}