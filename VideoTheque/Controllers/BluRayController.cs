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
        
        public BluRayController(ILogger<BluRayController> logger, IBluRayBusiness bluRayBusiness)
        {
            _logger = logger;
            _bluRayBusiness = bluRayBusiness;
        }
        
        [HttpGet]
        public async Task<List<FilmViewModel>> GetBluRays() => (await _bluRayBusiness.GetBluRays()).Adapt<List<FilmViewModel>>();
        
        [HttpGet("{id}")]
        public async Task<FilmViewModel> GetBluRay([FromRoute] int id) => _bluRayBusiness.GetBluRay(id).Adapt<FilmViewModel>();
        
        [HttpPost]
        public async Task<IResult> InsertBluRay([FromBody] FilmViewModel filmVm)
        {
            int id = filmVm.Id;
            string title = filmVm.Title;
            long duration = filmVm.Duration;
            string director_firstName = filmVm.Director.Split(" ")[0];
            string director_lastName = filmVm.Director.Split(" ")[1];
            int idDirector = _personneBusiness.GetPersonne(director_firstName,director_lastName).Id;
            string writer_firstName = filmVm.Writer.Split(" ")[0];
            string writer_lastName = filmVm.Writer.Split(" ")[1];
            int idScenarist = _personneBusiness.GetPersonne(writer_firstName,writer_lastName).Id;
            int idAgeRating = _ageRatingBusiness.GetAgeRating(filmVm.AgeRating).Id;
            int idGenre = _genresBusiness.GetGenre(filmVm.Genre).Id;
            string mainActor_firstName = filmVm.MainActor.Split(" ")[0];
            string mainActor_lastName = filmVm.MainActor.Split(" ")[1];
            int idFirstActor = _personneBusiness.GetPersonne(mainActor_firstName,mainActor_lastName).Id;
            
            BluRayDto bluRayDto = new BluRayDto()
            {
                Id = id,
                Title = title,
                Duration = duration,
                IdDirector = idDirector,
                IdScenarist = idScenarist,
                IdAgeRating = idAgeRating,
                IdGenre = idGenre,
                IdFirstActor = idFirstActor
            };
            var created = _bluRayBusiness.InsertBluRay(bluRayDto);
            return Results.Created($"/films/{created.Id}", created);
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