using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Film;
using VideoTheque.DTOs;

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
        public async Task<List<BluRayDto>> GetBluRays() => await _bluRayBusiness.GetBluRays();
        
        [HttpGet("{id}")]
        public BluRayDto GetBluRay([FromRoute] int id) => _bluRayBusiness.GetBluRay(id);
        
        [HttpPost]
        public async Task<IResult> InsertBluRay([FromBody] BluRayDto bluRayDto)
        {
            var created = _bluRayBusiness.InsertBluRay(bluRayDto);
            return Results.Created($"/films/{created.Id}", created);
        }
        
        [HttpPut("{id}")]
        public async Task<IResult> UpdateBluRay([FromRoute] int id, [FromBody] BluRayDto bluRayDto)
        {
            _bluRayBusiness.UpdateBluRay(id, bluRayDto);
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