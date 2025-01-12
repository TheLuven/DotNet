using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Film;
using VideoTheque.Repositories.Host;

namespace VideoTheque.Businesses.Host
{
    public class HostBusiness: IHostBusiness
    {
        private readonly IHostRepository _hostDao;
        private readonly IBluRayRepository _bluRayDao;

        public HostBusiness(IHostRepository hostDao, IBluRayRepository bluRayDao)
        {
            _hostDao = hostDao;
            _bluRayDao = bluRayDao;
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
            var isHostUsed = _bluRayDao.HasBluRayByOwner(id).Result;
            if (!isHostUsed)
            {
                if (_hostDao.DeleteHost(id).IsFaulted)
                { 
                    throw new InternalErrorException($"Erreur lors de la suppression du host d'identifiant {id}");
                }
            }
            else
            {
                throw new InvalidOperationException($"Le host d'identifiant {id} est utilis� par un ou plusieurs BluRay(s)");
            }
            
        }
    }
}