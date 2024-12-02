using Mapster;
using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Film;
using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers
{
    [ApiController]
    [Route("films")]
    public class BluRayController
    {
        private readonly IBluRayBusiness _bluRayBusiness;
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
            var created = _bluRayBusiness.InsertBluRay(filmVm.Adapt<BluRayDto>());
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