using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Host;

namespace VideoTheque.Businesses.Host
{
    public class HostBusiness: IHostBusiness
    {
        private readonly IHostRepository _hostDao;

        public HostBusiness(IHostRepository hostDao)
        {
            _hostDao = hostDao;
        }

        public Task<List<HostDto>> GetHosts() => _hostDao.GetHosts();

        public HostDto GetHost(int id)
        {
            var host = _hostDao.GetHost(id).Result;

            if (host == null)
            {
                throw new NotFoundException($"Host '{id}' non trouv�");
            }

            return host;
        }

        public HostDto InsertHost(HostDto host)
        {
            if (_hostDao.InsertHost(host).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du host {host.Name}");
            }

            return host;
        }

        public void UpdateHost(int id, HostDto host)
        {
            if (_hostDao.UpdateHost(id, host).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du host {host.Name}");
            }
        }

        public void DeleteHost(int id)
        {
            if (_hostDao.DeleteHost(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du host d'identifiant {id}");
            }
        }
    }
}