using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Host
{
    public interface IHostRepository
    {
        Task<List<HostDto>> GetHosts();

        ValueTask<HostDto?> GetHost(int id);

        Task InsertHost(HostDto host);

        Task UpdateHost(int id, HostDto host);

        Task DeleteHost(int id);
        
    }
}