

using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Film;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.PersonneRepository;
using VideoTheque.Repositories.Host;
using System.Net.Http;

namespace VideoTheque.Businesses.Film
{
    public class BluRayBusiness : IBluRayBusiness
    {
        private readonly IBluRayRepository _filmDao;
        private readonly IPersonneRepository _personneDao;
        private readonly IGenresRepository _genresRepository;
        private readonly IAgeRatingRepository _ageRatingRepository;
        private readonly IHostRepository _hostRepository;

        public BluRayBusiness(IBluRayRepository filmDao, IPersonneRepository personneDao, IGenresRepository genresRepository, IAgeRatingRepository ageRatingRepository, IHostRepository hostRepository)
        {
            _filmDao = filmDao;
            _personneDao = personneDao;
            _genresRepository = genresRepository;
            _ageRatingRepository = ageRatingRepository;
            _hostRepository = hostRepository;
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
                Support = "Blu-Ray"
            }).ToList();
        }

        public BluRayDto InsertBluRay(BluRayDto film)
        {
            film.IsAvailable = true;
            if (_filmDao.InsertBluRay(film).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du film {film.Title}");
            }

            return film;
        }

        public async void DeleteBluRay(int id)
        {
            var film = _filmDao.GetBluRay(id).Result;
            if (film != null && film.IdOwner != null)
            {
                var host = _hostRepository.GetHost(film.IdOwner.Value);
               
                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync($"http://{host.Result.Url}/emprunt/{film.Title}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new InternalErrorException($"Erreur lors de la suppression du film {film.Title}");
                }
                                
            }
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
                Support = "Blu-Ray"
            };
        }

        public async Task<List<EmpruntPauvreDto?>> GetEmpruntAvailable(int idHost)
        {
            var client = new HttpClient();
            var ipHost = await _hostRepository.GetHost(idHost);
            HttpResponseMessage response = await client.GetAsync($"http://{ipHost.Url}/emprunt/dispo");
			Console.WriteLine(response.StatusCode);
			Console.WriteLine($"http://{ipHost.Url}/emprunt/dispo");
            if (response.IsSuccessStatusCode)
            {
				var responseBody = await response.Content.ReadAsStringAsync();
        		Console.WriteLine(responseBody);
                var films = await response.Content.ReadFromJsonAsync<List<EmpruntPauvreDto>>();
                return films;
            }
            else
            {
                return null;
            }
        }

        public async Task<BluRayDto> AddFilmByEmprunt(int idHost, int idFilm)
        {
            var client = new HttpClient();
            var ipHost = await _hostRepository.GetHost(idHost);
            HttpResponseMessage response = await client.PostAsync($"http://{ipHost.Url}/emprunt/{idFilm}", null);
            if (response.IsSuccessStatusCode)
            {
                HttpResponseMessage responseRiche = await client.GetAsync($"http://{ipHost.Url}/emprunt/{idFilm}");

                if (responseRiche.IsSuccessStatusCode)
                {
                    var responseBody = await responseRiche.Content.ReadAsStringAsync();
                    var film = await responseRiche.Content.ReadFromJsonAsync<EmpruntRicheDto>();
                    
                    
                    var title = film.Title;
                    var duration = film.Duration;
                    var genreId = -1;
                    var ageRatingId = -1;
                    var directorId = -1;
                    var firstActorId = -1;
                    var writerId = -1;


                    var genre = _genresRepository.GetGenre(film.Genre.Name).Result;
                    if (genre == null)
                    {
                        _genresRepository.InsertGenre(new GenreDto { Name = film.Genre.Name });
                        genreId = _genresRepository.GetGenre(film.Genre.Name).Result.Id;
                    }
                    else
                    {
                        genreId = genre.Id;
                    }
                    
                    var ageRating = _ageRatingRepository.GetAgeRating(film.AgeRating.Abreviation).Result;
                    if (ageRating == null)
                    {
                        _ageRatingRepository.InsertAgeRating(new AgeRatingDto { Abreviation = film.AgeRating.Abreviation, Name = film.AgeRating.Name });
                        ageRatingId = _ageRatingRepository.GetAgeRating(film.AgeRating.Abreviation).Result.Id;
                    }
                    else
                    {
                        ageRatingId = ageRating.Id;
                    }
                    
                    var director = _personneDao.GetPersonne(film.Director.FirstName, film.Director.LastName).Result;
                    if (director == null)
                    {
                        _personneDao.InsertPersonne(new PersonneDto { FirstName = film.Director.FirstName, LastName = film.Director.LastName, Nationality = film.Director.Nationality, BirthDay = film.Director.BirthDay });
                        directorId = _personneDao.GetPersonne(film.Director.FirstName, film.Director.LastName).Result.Id;
                    }
                    else
                    {
                        directorId = director.Id;
                    }
                    
                    var firstActor = _personneDao.GetPersonne(film.FirstActor.FirstName, film.FirstActor.LastName).Result;
                    if (firstActor == null)
                    {
                        _personneDao.InsertPersonne(new PersonneDto { FirstName = film.FirstActor.FirstName, LastName = film.FirstActor.LastName, Nationality = film.FirstActor.Nationality, BirthDay = film.FirstActor.BirthDay });
                        firstActorId = _personneDao.GetPersonne(film.FirstActor.FirstName, film.FirstActor.LastName).Result.Id;
                    }
                    else
                    {
                        firstActorId = firstActor.Id;
                    }
                    
                    var writer = _personneDao.GetPersonne(film.Scenarist.FirstName, film.Scenarist.LastName).Result;
                    if (writer == null)
                    {
                        _personneDao.InsertPersonne(new PersonneDto
                        {
                            FirstName = film.Scenarist.FirstName, LastName = film.Scenarist.LastName,
                            Nationality = film.Scenarist.Nationality, BirthDay = film.Scenarist.BirthDay
                        });
                        writerId = _personneDao.GetPersonne(film.Scenarist.FirstName, film.Scenarist.LastName)
                            .Result.Id;
                    }
                    else
                    {
                        writerId = writer.Id;
                    }
                    
                    var bluRay = new BluRayDto
                    {
                        Title = title,
                        Duration = duration,
                        IdDirector = directorId,
                        IdFirstActor = firstActorId,
                        IdScenarist = writerId,
                        IdAgeRating = ageRatingId,
                        IdGenre = genreId,
                        IsAvailable = false,
                        IdOwner = idHost
                    };
                    
                    _filmDao.InsertBluRay(bluRay);
                    return bluRay;
                    

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}