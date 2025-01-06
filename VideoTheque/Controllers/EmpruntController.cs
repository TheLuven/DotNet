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
        public async Task<EmpruntRicheDto> GetEmprunt([FromRoute] int id)
        {
            var emprunt = await _empruntBusiness.GetEmprunt(id);
            return emprunt.Adapt<EmpruntRicheDto>();
        }

        [HttpPost("{id}")]
        public async Task<IResult> AddEmprunt([FromRoute] int id)
        {
            var created = await _empruntBusiness.AddEmprunt(id);
			if (created == null)
	            return Results.BadRequest("Film already borrowed.");

            return Results.Created($"/emprunt/{created.Id}", created);
        }
        
        [HttpDelete("{title}")]
        public async Task<IResult> DeleteEmprunt([FromRoute] string title)
        {
            try
            {
                title = title.Replace("%20", " ");
               _empruntBusiness.DeleteEmprunt(title); 
               return Results.NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting emprunt with title {Title}", title);
                return Results.BadRequest("Film is not borrowed.");
            }
            
        }
    }
}