using Core.Entities;

namespace MTS.Core.Interfaces.Services
{
    public interface IGeolocationService
    {
        Task<GeolocationResponse?> GetGeolocationAsync(string ipAddress);
    }
}