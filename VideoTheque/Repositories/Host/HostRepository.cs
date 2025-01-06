using Microsoft.EntityFrameworkCore;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Host
{
    public class HostRepository: IHostRepository
    {
        private readonly VideothequeDb _db;

        public HostRepository(VideothequeDb db)
        {
            _db = db;
        }

        public Task<List<HostDto>> GetHosts() => _db.Hosts.ToListAsync();

        public ValueTask<HostDto?> GetHost(int id) => _db.Hosts.FindAsync(id);

        public async Task InsertHost(HostDto host)
        {
            await _db.Hosts.AddAsync(host);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateHost(int id, HostDto host)
        {
            var hostToUpdate = _db.Hosts.FindAsync(id).Result;
            if (hostToUpdate is null)
            {
                throw new KeyNotFoundException($"Host '{id}' non trouvé");
            }
            hostToUpdate.Name = host.Name;
            hostToUpdate.Url = host.Url;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteHost(int id)
        {
            var hostToDelete = _db.Hosts.FindAsync(id).Result;
            if (hostToDelete is null)
            {
                throw new KeyNotFoundException($"Host '{id}' non trouvé");
            }
            _db.Hosts.Remove(hostToDelete);
            await _db.SaveChangesAsync();
        }
        
    }
}