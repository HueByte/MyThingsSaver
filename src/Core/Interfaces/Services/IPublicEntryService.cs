using Core.DTO;
using MTS.Core.DTO;
using MTS.Core.Models;

namespace MTS.Core.Interfaces.Services
{
    public interface IPublicEntryService
    {
        Task<PublicEntryDto?> GetPublicEntryAsync(string publicUrl);
        Task<string> TogglePublicEntryAsync(string targetId);
    }
}