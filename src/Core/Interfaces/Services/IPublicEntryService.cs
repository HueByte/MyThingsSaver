using MTS.Core.DTO;

namespace MTS.Core.Interfaces.Services
{
    public interface IPublicEntryService
    {
        Task<bool> TogglePublicEntryAsync(string targetId);
    }
}