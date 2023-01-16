using System.Net;
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

        public async Task<int> GetLoginLogsCountAsync()
        {
            return await _loginLogRepository.GetQueryable().CountAsync();
        }

        public async Task<int> GetUserLoginLogsCountAsync()
        {
            return await _loginLogRepository.GetAllAsync().CountAsync();
        }

        public async Task<List<LoginLogModel>> GetAllUserLoginLogsAsync()
        {
            return await _loginLogRepository.GetAllAsync().OrderByDescending(prop => prop.LoginDate)
                .ToListAsync();
        }

        public async Task<List<LoginLogModel>> GetLoginLogsPaginatedAsync(int page, int pageSize)
        {
            return await GetPaginatedLoginLogsAsync(_loginLogRepository.GetQueryable(), page, pageSize);
        }

        public async Task<List<LoginLogModel>> GetUserLoginLogsPaginatedAsync(int page, int pageSize)
        {

            return await GetPaginatedLoginLogsAsync(_loginLogRepository.GetAllAsync(), page, pageSize);
        }

        private static async Task<List<LoginLogModel>> GetPaginatedLoginLogsAsync(IQueryable<LoginLogModel> query, int page, int pageSize)
        {
            page = page <= 0 ? 1 : page;

            return await query.OrderByDescending(prop => prop.LoginDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddLoginLogAsync(ApplicationUserModel user, string ipAddress)
        {
            if (user is null)
                throw new EndpointException("User cannot be null");

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
                throw new EndpointException("Geolocation cannot be null");

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

        private static bool CheckIfIpAddressIsLocal(string ipAddress)
        {
            return ipAddress == IPAddress.Loopback.ToString()
                || ipAddress.StartsWith("0")
                || ipAddress.StartsWith("10")
                || ipAddress.StartsWith("172")
                || ipAddress.StartsWith("192");
        }
    }
}