using System.Net;
using Core.DTO;
using Core.Entities;
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

        public Task<int> GetLoginLogsCountAsync()
        {
            return _loginLogRepository.AsQueryable().CountAsync();
        }

        public Task<int> GetUserLoginLogsCountAsync()
        {
            return _loginLogRepository.AsIdentityQueryable().CountAsync();
        }

        public Task<List<LoginLogModel>> GetAllUserLoginLogsAsync()
        {
            return _loginLogRepository.AsIdentityQueryable().OrderByDescending(prop => prop.LoginDate)
                .ToListAsync();
        }

        public Task<List<LoginLogsDto>> GetLoginLogsPaginatedAsync(int page, int pageSize)
        {
            page = GetPage(page);

            return _loginLogRepository
                .AsQueryable()
                .Include(prop => prop.User)
                .OrderByDescending(prop => prop.LoginDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(entity => new LoginLogsDto()
                {
                    Id = entity.Id,
                    LoginDate = entity.LoginDate,
                    IpAddress = entity.IpAddress,
                    Location = entity.Location,
                    UserId = entity.UserId,
                    UserName = entity.User.UserName,
                })
                .ToListAsync();
        }

        public Task<List<LoginLogModel>> GetUserLoginLogsPaginatedAsync(int page, int pageSize)
        {
            page = GetPage(page);

            return _loginLogRepository.AsIdentityQueryable()
                .OrderByDescending(prop => prop.LoginDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddLoginLogAsync(ApplicationUserModel user, string ipAddress)
        {
            if (user is null)
                throw new HandledException("User cannot be null");

            GeolocationResponse? geolocation;
            if (!CheckIfIpAddressIsLocal(ipAddress))
            {
                geolocation = await _geolocationService.GetGeolocationAsync(ipAddress);
            }
            else
            {
                geolocation = new()
                {
                    City = "Localhost",
                };
            }

            if (geolocation is null)
                throw new HandledException("Geolocation cannot be null");

            var loginLog = new LoginLogModel
            {
                UserId = user.Id,
                IpAddress = ipAddress,
                LoginDate = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Location = $"{geolocation.City} {geolocation.Country} {geolocation.RegionName}"
            };

            await _loginLogRepository.AddAsync(loginLog);
            await _loginLogRepository.SaveChangesAsync();
            _logger.LogInformation("Login log created");
        }

        private static int GetPage(int page) => page <= 0 ? 1 : page;

        private static bool CheckIfIpAddressIsLocal(string ipAddress)
        {
            return ipAddress == IPAddress.Loopback.ToString()
                || ipAddress.StartsWith("0.")
                || ipAddress.StartsWith("10.")
                || ipAddress.StartsWith("172.")
                || ipAddress.StartsWith("192.");
        }
    }
}