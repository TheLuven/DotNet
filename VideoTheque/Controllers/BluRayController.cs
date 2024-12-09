using Mapster;
using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.AgeRating;
using VideoTheque.Businesses.Film;
using VideoTheque.Businesses.Genres;
using VideoTheque.Businesses.Personne;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers
{
    [ApiController]
    [Route("films")]
    public class BluRayController
    {
        private readonly IBluRayBusiness _bluRayBusiness;
        private readonly IPersonneBusiness _personneBusiness;
        private readonly IAgeRatingBusiness _ageRatingBusiness;
        private readonly IGenresBusiness _genresBusiness;
        protected readonly ILogger<BluRayController> _logger;

        public BluRayController(ILogger<BluRayController> logger, IBluRayBusiness bluRayBusiness,
            IPersonneBusiness personneBusiness, IAgeRatingBusiness ageRatingBusiness, IGenresBusiness genresBusiness)
        {
            _logger = logger;
            _bluRayBusiness = bluRayBusiness;
            _personneBusiness = personneBusiness;
            _ageRatingBusiness = ageRatingBusiness;
            _genresBusiness = genresBusiness;
        }

        [HttpGet]
        public async Task<List<FilmViewModel>> GetBluRays() =>
            (await _bluRayBusiness.GetBluRays()).Adapt<List<FilmViewModel>>();

        [HttpGet("{id}")]
        public async Task<FilmViewModel> GetBluRay([FromRoute] int id) =>
            _bluRayBusiness.GetBluRay(id).Adapt<FilmViewModel>();

        [HttpPost]
        public async Task<IResult> InsertBluRay([FromBody] FilmViewModel filmVm)
        {
            if (filmVm == null)
                return Results.BadRequest("Film data is required.");

            if (string.IsNullOrEmpty(filmVm.Director) || string.IsNullOrEmpty(filmVm.Writer) ||
                string.IsNullOrEmpty(filmVm.MainActor))
                return Results.BadRequest("Director, Writer, and Main Actor are required.");

            try
            {
                var directorNames = filmVm.Director.Split(" ");
                if (directorNames.Length < 2)
                    return Results.BadRequest("Director's full name is required.");
                var director = _personneBusiness.GetPersonne(directorNames[0], directorNames[1])
                    ?.Adapt<PersonneViewModel>();
                if (director == null)
                    return Results.NotFound("Director not found.");
                
                var writerNames = filmVm.Writer.Split(" ");
                if (writerNames.Length < 2)
                    return Results.BadRequest("Writer's full name is required.");
                var writer = _personneBusiness.GetPersonne(writerNames[0], writerNames[1])?.Adapt<PersonneViewModel>();
                if (writer == null)
                    return Results.NotFound("Writer not found.");
                
                var actorNames = filmVm.MainActor.Split(" ");
                if (actorNames.Length < 2)
                    return Results.BadRequest("Main actor's full name is required.");
                var mainActor = _personneBusiness.GetPersonne(actorNames[0], actorNames[1])?.Adapt<PersonneViewModel>();
                if (mainActor == null)
                    return Results.NotFound("Main actor not found.");
                
                var ageRating = _ageRatingBusiness.GetAgeRating(filmVm.AgeRating);
                if (ageRating == null)
                    return Results.NotFound("Age rating not found.");

                var genre = _genresBusiness.GetGenre(filmVm.Genre);
                if (genre == null)
                    return Results.NotFound("Genre not found.");
                
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
                return Results.Created($"/films/{created.Id}", created);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Results.Problem("An error occurred while processing the request.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IResult> UpdateBluRay([FromRoute] int id, [FromBody] FilmViewModel filmVm)
        {
            _bluRayBusiness.UpdateBluRay(id, filmVm.Adapt<BluRayDto>());
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteBluRay([FromRoute] int id)
        {
            _bluRayBusiness.DeleteBluRay(id);
            return Results.Ok();
        }
    }
}