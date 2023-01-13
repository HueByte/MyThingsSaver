using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;

namespace Core.Services.LoginLog
{
    public class LoginLogService : ILoginLogService
    {
        private readonly ILogger _logger;
        private readonly ILoginLogRepository _loginLogRepository;
        private readonly IGeolocationService _geolocationService;
        public LoginLogService(ILogger<LoginLogService> logger, ILoginLogRepository loginLogRepository, IGeolocationService geolocationService)
        {
            _logger = logger;
            _loginLogRepository = loginLogRepository;
            _geolocationService = geolocationService;
        }

        public async Task<List<LoginLogModel>> GetAllLoginLogsAsync()
        {
            var allQuery = await _loginLogRepository.GetAllAsync();

            return await allQuery.ToListAsync();
        }

        public async Task LogLoginAsync(ApplicationUserModel user, string ipAddress)
        {
            if (user is null)
                throw new EndpointException("User cannot be null");

            var geoLocation = await _geolocationService.GetGeolocationAsync(ipAddress);

            if (geoLocation is null)
                throw new EndpointException("Geolocation cannot be null");

            var loginLog = new LoginLogModel
            {
                UserId = user.Id,
                IpAddress = ipAddress,
                LoginDate = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Location = $"{geoLocation.City}, {geoLocation.Country}, {geoLocation.RegionName}"
            };

            await _loginLogRepository.AddAsync(loginLog);
            await _loginLogRepository.SaveChangesAsync();
            _logger.LogInformation("Login log created");
        }
    }
}