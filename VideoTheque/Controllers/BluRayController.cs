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
        public async Task<List<BluRayViewModel>> GetBluRays() => (await _bluRayBusiness.GetBluRays()).Adapt<List<BluRayViewModel>>();
        
        [HttpGet("{id}")]
        public async Task<BluRayViewModel> GetBluRay([FromRoute] int id) => _bluRayBusiness.GetBluRay(id).Adapt<BluRayViewModel>();
        
        [HttpPost]
        public async Task<IResult> InsertBluRay([FromBody] BluRayViewModel bluRayVM)
        {
            var created = _bluRayBusiness.InsertBluRay(bluRayVM.Adapt<BluRayDto>());
            return Results.Created($"/films/{created.Id}", created);
        }
        
        [HttpPut("{id}")]
        public async Task<IResult> UpdateBluRay([FromRoute] int id, [FromBody] BluRayViewModel bluRayVM)
        {
            _bluRayBusiness.UpdateBluRay(id, bluRayVM.Adapt<BluRayDto>());
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