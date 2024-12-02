using Mapster;
using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Personne;
using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers
{
    [ApiController]
    [Route("personnes")]
    public class PersonneController : ControllerBase
    {
        private readonly IPersonneBusiness _personneBusiness;
        protected readonly ILogger<PersonneController> _logger;
        
        public PersonneController(ILogger<PersonneController> logger, IPersonneBusiness personneBusiness)
        {
            _logger = logger;
            _personneBusiness = personneBusiness;
        }
        
        [HttpGet]
        public async Task<List<PersonneViewModel>> GetPersonnes() => (await _personneBusiness.GetPersonnes()).Adapt<List<PersonneViewModel>>();
        
        [HttpGet("{id}")]
        public async Task<PersonneViewModel> GetPersonne([FromRoute] int id) => _personneBusiness.GetPersonne(id).Adapt<PersonneViewModel>();
        
        [HttpPost]
        public async Task<IResult> InsertPersonne([FromBody] PersonneViewModel personneVM)
        {
            var created = _personneBusiness.InsertPersonne(personneVM.Adapt<PersonneDto>());
            return Results.Created($"/personnes/{created.Id}", created);
        }
        
        [HttpPut("{id}")]
        public async Task<IResult> UpdatePersonne([FromRoute] int id, [FromBody] PersonneViewModel personneVM)
        {
            _personneBusiness.UpdatePersonne(id, personneVM.Adapt<PersonneDto>());
            return Results.NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IResult> DeletePersonne([FromRoute] int id)
        {
            _personneBusiness.DeletePersonne(id);
            return Results.Ok();
        }
    }
}