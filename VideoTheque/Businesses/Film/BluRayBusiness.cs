
using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Film;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.PersonneRepository;
using VideoTheque.Repositories.Host;
using System.Net.Http;
using Mapster;
using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Film
{
    public class BluRayBusiness : IBluRayBusiness
    {
        private readonly IBluRayRepository _filmDao;
        private readonly IPersonneRepository _personneDao;
        private readonly IGenresRepository _genresRepository;
        private readonly IAgeRatingRepository _ageRatingRepository;
        private readonly IHostRepository _hostRepository;
        private readonly ILogger<BluRayBusiness> _logger;

        public BluRayBusiness(IBluRayRepository filmDao, IPersonneRepository personneDao, IGenresRepository genresRepository, IAgeRatingRepository ageRatingRepository, IHostRepository hostRepository,ILogger<BluRayBusiness> logger)
        {
            _filmDao = filmDao;
            _personneDao = personneDao;
            _genresRepository = genresRepository;
            _ageRatingRepository = ageRatingRepository;
            _hostRepository = hostRepository;
            _logger = logger;
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
            film.IsAvailable = true;
            if (_filmDao.InsertBluRay(film).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du film {film.Title}");
            }

            return film;
        }

        public async void DeleteBluRay(int id)
        {
            try
            {
                var film = _filmDao.GetBluRay(id).Result;
                if (film != null && film.IdOwner != null)
                {
                    var host = _hostRepository.GetHost(film.IdOwner.Value);

                    var title = film.Title.Replace("%20", " ");
                    var client = new HttpClient();
                    _logger.LogInformation("Suppression du film {0} chez l'hôte {1}", title, host.Result.Url);
                    HttpResponseMessage response = 
                        await client.DeleteAsync($"http://{host.Result.Url}:5000/emprunt/{title}");
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Erreur lors de la suppression du film {0}", title);
                        throw new InternalErrorException(
                            $"Erreur lors de la suppression du film {film.Title}, le film n'a pas été rendu");
                    }

                }

                _logger.LogInformation("Suppression du film {0}", film.Title);
                if (film != null && film.IsAvailable != true && film.IdOwner == null)
                {
                    throw new InternalErrorException(
                        "Le film ne peut pas être supprimé car il est emprunté par un partenaire");
                }

                if (_filmDao.DeleteBluRay(id).IsFaulted)
                {
                    _logger.LogError("Erreur lors de la suppression du film d'identifiant {0}", id);
                    throw new InternalErrorException($"Erreur lors de la suppression du film d'identifiant {id}");
                }
            
            }
            catch (Exception e)
            {
                _logger.LogError("Erreur lors de la suppression du film d'identifiant {0}", e.Message);
               // throw new InternalErrorException($"Erreur lors de la suppression du film d'identifiant {id} : " + e.Message);
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

        public async Task<List<EmpruntPauvreDto?>> GetEmpruntAvailable(int idHost)
        {
            var client = new HttpClient();
            var ipHost = await _hostRepository.GetHost(idHost);
            _logger.LogInformation("Get emprunt available from host {0}", ipHost.Url);
            HttpResponseMessage response = await client.GetAsync($"http://{ipHost.Url}:5000/emprunt/dispo");
            if (response.IsSuccessStatusCode)
            {
				var responseBody = await response.Content.ReadAsStringAsync();
        		Console.WriteLine("response : "  ,responseBody);
                var films = await response.Content.ReadFromJsonAsync<List<EmpruntViewModel>>();
                var filmsDto = films.Adapt<List<EmpruntPauvreDto>>();
                return filmsDto;
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
            HttpResponseMessage response = await client.PostAsync($"http://{ipHost.Url}:5000/emprunt/{idFilm}", null);
            if (response.IsSuccessStatusCode)
            {
                HttpResponseMessage responseRiche = await client.GetAsync($"http://{ipHost.Url}:5000/emprunt/{idFilm}");

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


                    try
                    {
                        _logger.LogInformation("Recherche du genre ...");
                        var genre = _genresRepository.GetGenre(film.Genre.Name).Result;
                        _logger.LogInformation("Genre trouvé : {0}", genre.Name);
                        genreId = genre.Id;
                    }
                    catch (Exception e)
                    {
                        await _genresRepository.InsertGenre(new GenreDto { Name = film.Genre.Name });
                        genreId = _genresRepository.GetGenre(film.Genre.Name).Result.Id;
                    }

                    try
                    {
                        _logger.LogInformation("Recherche de l'age rating ...");
                        var ageRating = _ageRatingRepository.GetAgeRating(film.AgeRating.Name).Result;
                        if (ageRating == null)
                        {
                            throw new NotFoundException();
                        }
                        _logger.LogInformation("Age rating trouvé : {0}", ageRating.Name);
                        ageRatingId = ageRating.Id;
                    }
                    catch (Exception e)
                    {
                         await _ageRatingRepository.InsertAgeRating(new AgeRatingDto { Abreviation = film.AgeRating.Abreviation, Name = film.AgeRating.Name });
                         ageRatingId = _ageRatingRepository.GetAgeRating(film.AgeRating.Name).Result.Id;
                    }

                    try
                    {
                        _logger.LogInformation("Recherche du director ...");
                        var director = _personneDao.GetPersonne(film.Director.FirstName, film.Director.LastName).Result;
                        _logger.LogInformation("Director trouvé : {0}", director.FirstName);
                        directorId = director.Id;
                    }
                    catch (Exception e)
                    {
                        await _personneDao.InsertPersonne(new PersonneDto
                        {
                            FirstName = film.Director.FirstName, LastName = film.Director.LastName,
                            Nationality = film.Director.Nationality, BirthDay = film.Director.BirthDay
                        });
                        directorId = _personneDao.GetPersonne(film.Director.FirstName, film.Director.LastName).Result
                            .Id;
                    }

                    try
                    {
                        _logger.LogInformation("Recherche du first actor ...");
                        var firstActor = _personneDao.GetPersonne(film.FirstActor.FirstName, film.FirstActor.LastName)
                            .Result;
                        _logger.LogInformation("First actor trouvé : {0}", firstActor.FirstName);
                        firstActorId = firstActor.Id;
                    }
                    catch (Exception e)
                    {
                        await _personneDao.InsertPersonne(new PersonneDto { FirstName = film.FirstActor.FirstName, LastName = film.FirstActor.LastName, Nationality = film.FirstActor.Nationality, BirthDay = film.FirstActor.BirthDay });
                        firstActorId = _personneDao.GetPersonne(film.FirstActor.FirstName, film.FirstActor.LastName).Result.Id;
                    }

                    try
                    {
                        _logger.LogInformation("Recherche du writer ...");
                        var writer = _personneDao.GetPersonne(film.Scenarist.FirstName, film.Scenarist.LastName).Result;
                        writerId = writer.Id;
                    }
                    catch (Exception e)
                    {
                        await _personneDao.InsertPersonne(new PersonneDto
                                                {
                                                    FirstName = film.Scenarist.FirstName, LastName = film.Scenarist.LastName,
                                                    Nationality = film.Scenarist.Nationality, BirthDay = film.Scenarist.BirthDay
                                                });
                        _logger.LogInformation("Writer trouvé : {0}", film.Scenarist.FirstName);
                        writerId = _personneDao.GetPersonne(film.Scenarist.FirstName, film.Scenarist.LastName).Result.Id;
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

                    _logger.LogInformation("Insertion du film ", bluRay);
                    
                    await _filmDao.InsertBluRay(bluRay);
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