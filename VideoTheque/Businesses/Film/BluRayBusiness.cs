

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

        private string GetPersonFullName(int personId)
        {
            var person = _personneDao.GetPersonne(personId).Result;
            return person != null ? $"{person.FirstName} {person.LastName}" : "Unknown";
        }

        private string GetAgeRatingName(int ageRatingId)
        {
            var ageRating = _ageRatingRepository.GetAgeRating(ageRatingId).Result;
            return ageRating?.Name ?? "Unknown";
        }

        private string GetGenreName(int genreId)
        {
            var genre = _genresRepository.GetGenre(genreId).Result;
            return genre?.Name ?? "Unknown";
        }

        public async Task<List<FilmDto>> GetBluRays()
        {
            var bluRays = await _filmDao.GetBluRays();
            return bluRays.Select(film => new FilmDto
            {
                Id = film.Id,
                Title = film.Title,
                Duration = film.Duration,
                MainActor = GetPersonFullName(film.IdFirstActor),
                Director = GetPersonFullName(film.IdDirector),
                Writer = GetPersonFullName(film.IdScenarist),
                AgeRating = GetAgeRatingName(film.IdAgeRating),
                Genre = GetGenreName(film.IdGenre),
                Support = "BLURAY"
            }).ToList();
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
                MainActor = GetPersonFullName(film.IdFirstActor),
                Director = GetPersonFullName(film.IdDirector),
                Writer = GetPersonFullName(film.IdScenarist),
                AgeRating = GetAgeRatingName(film.IdAgeRating),
                Genre = GetGenreName(film.IdGenre),
                Support = "BLURAY"
            };
        }
    }
}