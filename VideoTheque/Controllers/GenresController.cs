namespace VideoTheque.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.Genres;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;

    [ApiController]
    [Route("genres")]
    public class GenresController : ControllerBase
    {
        private readonly IGenresBusiness _genresBusiness;
        protected readonly ILogger<GenresController> _logger;

        public GenresController(ILogger<GenresController> logger, IGenresBusiness genresBusiness)
        {
            _logger = logger;
            _genresBusiness = genresBusiness;
        }

        [HttpGet]
        public async Task<List<GenreViewModel>> GetGenres() => (await _genresBusiness.GetGenres()).Adapt<List<GenreViewModel>>();

        [HttpGet("{id}")]
        public async Task<GenreViewModel> GetGenre([FromRoute] int id) => _genresBusiness.GetGenre(id).Adapt<GenreViewModel>();

        [HttpPost]
        public async Task<IResult> InsentGenre([FromBody] GenreViewModel genreVM)
        {
            var created = _genresBusiness.InsertGenre(genreVM.Adapt<GenreDto>());
            return Results.Created($"/genres/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateGenre([FromRoute] int id, [FromBody] GenreViewModel genreVM)
        {
            _genresBusiness.UpdateGenre(id, genreVM.Adapt<GenreDto>());
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteGenre([FromRoute] int id)
        {
            _genresBusiness.DeleteGenre(id);
            return Results.Ok();
        }
    }
}
