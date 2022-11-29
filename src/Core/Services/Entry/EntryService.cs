using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Services.CurrentUser;

namespace Core.Services.Entry;

public class EntryService
{
    private readonly IEntryRepository _repository;
    private readonly ICurrentUserService _currentUser;
    public EntryService(IEntryRepository repository, ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<EntryModel> GetEntryAsync(string id)
    {
        return await _repository.GetAsync(id);
    }
}