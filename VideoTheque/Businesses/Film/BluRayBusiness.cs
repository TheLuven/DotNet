

using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Film;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.PersonneRepository;

namespace VideoTheque.Businesses.Film
{
    public class BluRayBusiness : IBluRayBusiness
    {
        private readonly IBluRayRepository _filmDao;
        private readonly IPersonneRepository _personneDao;
        private readonly IGenresRepository _genresRepository;
        private readonly IAgeRatingRepository _ageRatingRepository;
        
        public BluRayBusiness(IBluRayRepository filmDao, IPersonneRepository personneDao, IGenresRepository genresRepository, IAgeRatingRepository ageRatingRepository)
        {
            _filmDao = filmDao;
            _personneDao = personneDao;
            _genresRepository = genresRepository;
            _ageRatingRepository = ageRatingRepository;
        }


        public Task<List<FilmDto>> GetBluRays()
        {
            // Get all BluRays then return them in a FilmDto list after parsing ID to string
            return _filmDao.GetBluRays().ContinueWith(task =>
            {
                return task.Result.Select(film => 
                    new FilmDto
                {
                    Id = film.Id,
                    Title = film.Title,
                    Duration = film.Duration,
                    FirstActor = _personneDao.GetPersonne(film.IdFirstActor).Result.LastName+" "+_personneDao.GetPersonne(film.IdFirstActor).Result.FirstName,
                    Director = _personneDao.GetPersonne(film.IdDirector).Result.LastName+" "+_personneDao.GetPersonne(film.IdDirector).Result.FirstName,
                    Scenarist = _personneDao.GetPersonne(film.IdScenarist).Result.LastName+" "+_personneDao.GetPersonne(film.IdScenarist).Result.FirstName,
                    AgeRating = _ageRatingRepository.GetAgeRating(film.IdAgeRating).Result.Name,
                    Genre = _genresRepository.GetGenre(film.IdGenre).Result.Name,
                    Support = "Blu-Ray"
                }).ToList();
            });
        }

        public BluRayDto InsertBluRay(BluRayDto film)
        {
            if (_filmDao.InsertBluRay(film).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du film {film.Title}");
            }

            return film;
        }

        public void DeleteBluRay(int id)
        {
            if (_filmDao.DeleteBluRay(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du film d'identifiant {id}");
            }
        }

        public void UpdateBluRay(int id, BluRayDto film)
        {
            if (_filmDao.UpdateBluRay(id, film).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du film {film.Title}");
            }
        }

        public FilmDto GetBluRay(int id)
        {
            var film = _filmDao.GetBluRay(id).Result;

            if (film == null)
            {
                throw new NotFoundException($"Film '{id}' non trouvé");
            }

            return new FilmDto
            {
                Id = film.Id,
                Title = film.Title,
                Duration = film.Duration,
                FirstActor = _personneDao.GetPersonne(film.IdFirstActor).Result.LastName+" "+_personneDao.GetPersonne(film.IdFirstActor).Result.FirstName,
                Director = _personneDao.GetPersonne(film.IdDirector).Result.LastName+" "+_personneDao.GetPersonne(film.IdDirector).Result.FirstName,
                Scenarist = _personneDao.GetPersonne(film.IdScenarist).Result.LastName+" "+_personneDao.GetPersonne(film.IdScenarist).Result.FirstName,
                AgeRating = _ageRatingRepository.GetAgeRating(film.IdAgeRating).Result.Name,
                Genre = _genresRepository.GetGenre(film.IdGenre).Result.Name,
                Support = "Blu-Ray"
            };
        }
    }
}