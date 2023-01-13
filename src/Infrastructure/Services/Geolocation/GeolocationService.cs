using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Constants;
using Core.Entities;
using Core.Interfaces.Services;
using MTS.Core.Interfaces.Services;

namespace MTS.Infrastructure.Services.Geolocation
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _client;
        public GeolocationService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(HttpClientNames.GEO_LOCATION_CLIENT);
        }

        public async Task<GeolocationResponse?> GetGeolocationAsync(string ipAddress)
        {
            if (ValidateIPv4(ipAddress) == false)
                return null;

            var result = await _client.GetAsync($"/json/{ipAddress}?fields=status,message,country,countryCode,region,regionName,city,zip,isp,org,query");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadFromJsonAsync<GeolocationResponse>();
                return content;
            }

            return null;
        }

        private bool ValidateIPv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
        }
    }
}