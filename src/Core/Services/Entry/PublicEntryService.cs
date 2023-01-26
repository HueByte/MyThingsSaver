using System.Diagnostics.CodeAnalysis;
using Core.DTO;
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

        public async Task<PublicEntryDto?> GetPublicEntryAsync(string publicUrl)
        {
            var publicEntry = await _publicEntryRepository
                .GetQueryable()
                .FirstOrDefaultAsync(x => x.PublicUrl == publicUrl);

            if (publicEntry is null)
                return null!;

            var entry = await _entryRepository
                .GetQueryable()
                .Include(e => e.User)
                .Where(e => e.Id == publicEntry.EntryId)
                .Select(e => new PublicEntryDto
                {
                    Title = e.Name,
                    Content = e.Content,
                    CreatedOn = e.CreatedOn,
                    LastUpdatedOn = e.LastUpdatedOn,
                    Owner = e.User!.UserName!,
                    OwnerAvatar = e.User!.AvatarUrl,
                    Size = e.Size
                })
                .FirstOrDefaultAsync();

            return entry;
        }

        public async Task<string> TogglePublicEntryAsync(string targetId)
        {
            string? result = null;

            if (string.IsNullOrEmpty(targetId))
                return result!;

            var entry = await _entryRepository.GetAsync(targetId);
            if (entry is null)
                return result!;

            if (IsPublicEntry(entry))
            {
                await MakePrivateAsync(entry);
            }
            else
            {
                result = await MakePublicAsync(entry);
            }

            await _entryRepository.SaveChangesAsync();

            return result!;
        }

        private async Task<string> MakePublicAsync(EntryModel entry)
        {
            entry.PublicEntry = new()
            {
                UserId = entry.UserId,
                EntryId = entry.Id,
                PublicUrl = GenerateUrl()
            };

            await _entryRepository.UpdateAsync(entry);

            return entry.PublicEntry.PublicUrl;
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