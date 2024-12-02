using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Host
{
    public interface IHostBusiness
    {
        Task<List<HostDto>> GetHosts();

        HostDto GetHost(int id);

        HostDto InsertHost(HostDto host);

        void UpdateHost(int id, HostDto host);

        void DeleteHost(int id);
        
    }
}