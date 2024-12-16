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
        public async Task<List<GenreViewModel>> GetGenres()
        {
            _logger.LogInformation("Getting all genres");
            return (await _genresBusiness.GetGenres()).Adapt<List<GenreViewModel>>();
        }

        [HttpGet("{id}")]
        public async Task<GenreViewModel> GetGenre([FromRoute] int id)
        {
            _logger.LogInformation($"Getting genre {id}");
            return _genresBusiness.GetGenre(id).Adapt<GenreViewModel>();
        }

        [HttpPost]
        public async Task<IResult> InsertGenre([FromBody] GenreViewModel genreVM)
        {
            _logger.LogInformation("Creating genre");
            var created = _genresBusiness.InsertGenre(genreVM.Adapt<GenreDto>());
            _logger.LogInformation($"Genre {created.Id} created");
            return Results.Created($"/genres/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateGenre([FromRoute] int id, [FromBody] GenreViewModel genreVM)
        {
            _logger.LogInformation($"Updating genre {id}");
            _genresBusiness.UpdateGenre(id, genreVM.Adapt<GenreDto>());
            _logger.LogInformation($"Genre {id} updated");
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteGenre([FromRoute] int id)
        {
            _logger.LogInformation($"Deleting genre {id}");
            _genresBusiness.DeleteGenre(id);
            _logger.LogInformation($"Genre {id} deleted");
            return Results.Ok();
        }
    }
}
