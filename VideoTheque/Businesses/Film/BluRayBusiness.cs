

using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Film;

namespace VideoTheque.Businesses.Film
{
    public class BluRayBusiness : IBluRayBusiness
    {
        private readonly IBluRayRepository _filmDao;
        
        public BluRayBusiness(IBluRayRepository filmDao)
        {
            _filmDao = filmDao;
        }


        public Task<List<BluRayDto>> GetBluRays()
        {
            var films = _filmDao.GetBluRays().Result;
            
            if (films == null)
            {
                throw new NotFoundException("Aucun film trouvé");
            }
        }

        public BluRayDto InsertBluRay(BluRayDto film)
        {
            if (_filmDao.InsertBluRay(film).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du film {film.Title}");
            }

            return film;
        }

        public Void DeleteBluRay(int id)
        {
            if (_filmDao.DeleteBluRay(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du film d'identifiant {id}");
            }
        }

        public Void UpdateBluRay(int id, BluRayDto film)
        {
            if (_filmDao.UpdateBluRay(id, film).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du film {film.Title}");
            }
        }

        public BluRayDto GetBluRay(int id)
        {
            var film = _filmDao.GetBluRay(id).Result;

            if (film == null)
            {
                throw new NotFoundException($"Film '{id}' non trouvé");
            }

            return film;
        }
    }
}