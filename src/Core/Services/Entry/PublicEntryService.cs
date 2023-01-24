using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MTS.Core.DTO;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;

namespace Core.Services.Entry
{
    public class PublicEntryService : IPublicEntryService
    {
        private readonly ILogger _logger;
        private readonly IPublicEntryRepository _publicEntryRepository;
        private readonly IEntryRepository _entryRepository;

        public PublicEntryService(ILogger<PublicEntryService> logger, IPublicEntryRepository publicEntryRepository, IEntryRepository entryRepository)
        {
            _logger = logger;
            _publicEntryRepository = publicEntryRepository;
            _entryRepository = entryRepository;
        }

        public async Task<bool> TogglePublicEntryAsync(string targetId)
        {
            if (string.IsNullOrEmpty(targetId))
                return false;

            var entry = await _entryRepository.GetAsync(targetId);
            if (entry is null)
                return false;

            // Is public
            if (IsPublicEntry(entry))
            {
                await MakePrivateAsync(entry);
            }
            else
            {
                await MakePublicAsync(entry);
            }

            await _entryRepository.SaveChangesAsync();

            return true;
        }

        private async Task MakePublicAsync(EntryModel entry)
        {
            entry.PublicEntry = new()
            {
                UserId = entry.UserId,
                EntryId = entry.Id,
                PublicUrl = GenerateUrl()
            };

            await _entryRepository.UpdateAsync(entry);
        }

        private async Task MakePrivateAsync(EntryModel entry)
        {
            var publicEntry = await _publicEntryRepository
                .GetQueryable()
                .FirstOrDefaultAsync(x => x.Id == entry.PublicEntryId);

            if (publicEntry is null)
                _logger.LogWarning("Public entry not found for entry {entryId}", entry.Id);
            else
                await _publicEntryRepository.RemoveAsync(publicEntry);
        }

        private string GenerateUrl() =>
            Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace('/', '_')
                .Replace('+', '-')
                .TrimEnd('=');

        private static bool IsPublicEntry(EntryModel entry) => entry.PublicEntryId != default;
    }
}