using Mapster;
using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.AgeRating;
using VideoTheque.Businesses.Film;
using VideoTheque.Businesses.Genres;
using VideoTheque.Businesses.Personne;
using VideoTheque.Businesses.Host;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.PersonneRepository;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers
{
    [ApiController]
    [Route("films")]
    public class BluRayController
    {
        private readonly IBluRayBusiness _bluRayBusiness;
        private readonly IPersonneRepository _personneRepository;
        private readonly IAgeRatingRepository _ageRatingRepository;
        private readonly IGenresRepository _genresRepository;
        protected readonly ILogger<BluRayController> _logger;
        protected readonly IHostBusiness _hostBusiness;

        public BluRayController(ILogger<BluRayController> logger, IBluRayBusiness bluRayBusiness,
            IPersonneRepository personneRepository, IAgeRatingRepository ageRatingRepository,
            IGenresRepository genresRepository, IHostBusiness hostBusiness)
        {
            _logger = logger;
            _bluRayBusiness = bluRayBusiness;
            _hostBusiness = hostBusiness;
            _personneRepository = personneRepository;
            _ageRatingRepository = ageRatingRepository;
            _genresRepository = genresRepository;
        }

        [HttpGet]
        public async Task<List<FilmViewModel>> GetBluRays()
        {
            _logger.LogInformation("Getting all BluRays");
            return (await _bluRayBusiness.GetBluRays()).Adapt<List<FilmViewModel>>();
        }

        [HttpGet("{id}")]
        public async Task<FilmViewModel> GetBluRay([FromRoute] int id)
        {
            _logger.LogInformation("Getting BluRay with id {id}", id);
            return _bluRayBusiness.GetBluRay(id).Adapt<FilmViewModel>();
        }

        [HttpPost]
        public async Task<IResult> InsertBluRay([FromBody] FilmViewModel filmVm)
        {
            if (filmVm == null)
            {
                _logger.LogWarning("Film data is required.");
                return Results.BadRequest("Film data is required.");
            }

            if (string.IsNullOrEmpty(filmVm.Director) || string.IsNullOrEmpty(filmVm.Writer) ||
                string.IsNullOrEmpty(filmVm.MainActor))
            {
                _logger.LogWarning("Director, Writer, and Main Actor are required.");
                return Results.BadRequest("Director, Writer, and Main Actor are required.");
            }

            try
            {
                var directorNames = filmVm.Director.Split(" ");
                if (directorNames.Length < 2)
                {
                    _logger.LogWarning("Director's full name is required.");
                    return Results.BadRequest("Director's full name is required.");
                }

                var director = _personneRepository.GetPersonne(directorNames[0], directorNames[1]).Result
                    ?.Adapt<PersonneViewModel>();
                if (director == null)
                {
                    _logger.LogWarning("Director not found.");
                    return Results.NotFound("Director not found.");
                }

                var writerNames = filmVm.Writer.Split(" ");
                if (writerNames.Length < 2)
                {
                    _logger.LogWarning("Writer's full name is required.");
                    return Results.BadRequest("Writer's full name is required.");
                }

                var writer = _personneRepository.GetPersonne(writerNames[0], writerNames[1]).Result
                    ?.Adapt<PersonneViewModel>();
                if (writer == null)
                {
                    _logger.LogWarning("Writer not found.");
                    return Results.NotFound("Writer not found.");
                }

                var actorNames = filmVm.MainActor.Split(" ");
                if (actorNames.Length < 2)
                {
                    _logger.LogWarning("Main actor's full name is required.");
                    return Results.BadRequest("Main actor's full name is required.");
                }

                var mainActor = _personneRepository.GetPersonne(actorNames[0], actorNames[1]).Result
                    ?.Adapt<PersonneViewModel>();
                if (mainActor == null)
                {
                    _logger.LogWarning("Main actor not found.");
                    return Results.NotFound("Main actor not found.");
                }

                var ageRating = _ageRatingRepository.GetAgeRating(filmVm.AgeRating).Result;
                if (ageRating == null)
                {
                    _logger.LogWarning("Age rating not found.");
                    return Results.NotFound("Age rating not found.");
                }

                var genre = _genresRepository.GetGenre(filmVm.Genre).Result;
                if (genre == null)
                {
                    _logger.LogWarning("Genre not found.");
                    return Results.NotFound("Genre not found.");
                }

                BluRayDto bluRayDto = new BluRayDto()
                {
                    Id = filmVm.Id,
                    Title = filmVm.Title,
                    Duration = filmVm.Duration,
                    IdDirector = director.Id,
                    IdScenarist = writer.Id,
                    IdAgeRating = ageRating.Id,
                    IdGenre = genre.Id,
                    IdFirstActor = mainActor.Id
                };

                var created = _bluRayBusiness.InsertBluRay(bluRayDto);
                _logger.LogInformation("BluRay created with id {id}", created.Id);
                return Results.Created($"/films/{created.Id}", created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return Results.Problem("An error occurred while processing the request.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IResult> UpdateBluRay([FromRoute] int id, [FromBody] FilmViewModel filmVm)
        {
            var directorNames = filmVm.Director.Split(" ");
            if (directorNames.Length < 2)
            {
                _logger.LogWarning("Director's full name is required.");
                return Results.BadRequest("Director's full name is required.");
            }

            var director = _personneRepository.GetPersonne(directorNames[0], directorNames[1]).Result
                ?.Adapt<PersonneViewModel>();
            if (director == null)
            {
                _logger.LogWarning("Director not found.");
                return Results.NotFound("Director not found.");
            }

            var writerNames = filmVm.Writer.Split(" ");
            if (writerNames.Length < 2)
            {
                _logger.LogWarning("Writer's full name is required.");
                return Results.BadRequest("Writer's full name is required.");
            }

            var writer = _personneRepository.GetPersonne(writerNames[0], writerNames[1]).Result
                ?.Adapt<PersonneViewModel>();
            if (writer == null)
            {
                _logger.LogWarning("Writer not found.");
                return Results.NotFound("Writer not found.");
            }

            var actorNames = filmVm.MainActor.Split(" ");
            if (actorNames.Length < 2)
            {
                _logger.LogWarning("Main actor's full name is required.");
                return Results.BadRequest("Main actor's full name is required.");
            }

            var mainActor = _personneRepository.GetPersonne(actorNames[0], actorNames[1]).Result
                ?.Adapt<PersonneViewModel>();
            if (mainActor == null)
            {
                _logger.LogWarning("Main actor not found.");
                return Results.NotFound("Main actor not found.");
            }

            var ageRating = _ageRatingRepository.GetAgeRating(filmVm.AgeRating).Result;
            if (ageRating == null)
            {
                _logger.LogWarning("Age rating not found.");
                return Results.NotFound("Age rating not found.");
            }

            var genre = _genresRepository.GetGenre(filmVm.Genre).Result;
            if (genre == null)
            {
                _logger.LogWarning("Genre not found.");
                return Results.NotFound("Genre not found.");
            }

            BluRayDto bluRayDto = new BluRayDto()
            {
                Id = filmVm.Id,
                Title = filmVm.Title,
                Duration = filmVm.Duration,
                IdDirector = director.Id,
                IdScenarist = writer.Id,
                IdAgeRating = ageRating.Id,
                IdGenre = genre.Id,
                IdFirstActor = mainActor.Id
            };

            _logger.LogInformation("Updating BluRay with id {id}", id);
            _bluRayBusiness.UpdateBluRay(id, bluRayDto);
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteBluRay([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Deleting BluRay with id {id}", id);
                await _bluRayBusiness.DeleteBluRay(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return Results.BadRequest("An error occurred while processing the request : " + ex.Message);
            }
            return Results.Ok();
        }


        [HttpGet("{idHost}/available")]
        public async Task<List<EmpruntViewModel?>> GetEmpruntAvailable([FromRoute] int idHost)
        {
            
			var films_available = await _bluRayBusiness.GetEmpruntAvailable(idHost);
            return films_available.Adapt<List<EmpruntViewModel>>();
        }

		[HttpPost("{idHost}/{idFilm}")]
        public async Task<IResult> AddEmprunt([FromRoute] int idHost, [FromRoute] int idFilm)
        {
            var created = await _bluRayBusiness.AddFilmByEmprunt(idHost, idFilm);
			return Results.Created($"/films/{created.Id}", created);
        }
    }
}