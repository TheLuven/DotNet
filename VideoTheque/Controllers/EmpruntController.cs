namespace VideoTheque.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.Emprunt;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;
    
    [ApiController]
    [Route("emprunt")]
    public class EmpruntController : ControllerBase
    {
        private readonly IEmpruntBusiness _empruntBusiness;
        protected readonly ILogger<AgeRatingController> _logger;

        public EmpruntController(ILogger<AgeRatingController> logger, IEmpruntBusiness empruntBusiness)
        {
            _logger = logger;
            _empruntBusiness = empruntBusiness;
        }
        
        [HttpGet("dispo")]
        public async Task<List<EmpruntViewModel>> GetEmprunts() => (await _empruntBusiness.GetEmpruntsDispo()).Adapt<List<EmpruntViewModel>>(); 
        
        [HttpGet("{id}")]
        public async Task<EmpruntRicheDto> GetEmprunt([FromRoute] int id) => _empruntBusiness.GetEmprunt(id).Adapt<EmpruntViewModel>();
        
        [HttpPost]
        public async Task<IResult> AddEmprunt([FromBody] int id)
        {
            var created = _empruntBusiness.addEmprunt(id);
            return Results.Created($"/emprunt/{created.Id}", created);
        }
        
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteEmprunt([FromRoute] int id)
        {
            _empruntBusiness.deleteEmprunt(id);
            return Results.NoContent();
        }
    }
}