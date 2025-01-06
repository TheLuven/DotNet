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
        public async Task<List<PersonneViewModel>> GetPersonnes()
        {
            _logger.LogInformation("Getting all personnes");
            return (await _personneBusiness.GetPersonnes()).Adapt<List<PersonneViewModel>>();
        }

        [HttpGet("{id}")]
        public async Task<PersonneViewModel> GetPersonne([FromRoute] int id)
        {
            _logger.LogInformation($"Getting personne {id}");
            return _personneBusiness.GetPersonne(id).Adapt<PersonneViewModel>();
        }

        [HttpGet("{firstName}/{lastName}")]
        public async Task<PersonneViewModel> GetPersonne([FromRoute] string firstName, [FromRoute] string lastName)
        {
            _logger.LogInformation($"Getting personne {firstName} {lastName}");
            return _personneBusiness.GetPersonne(firstName, lastName).Adapt<PersonneViewModel>();
        }

        [HttpPost]
        public async Task<IResult> InsertPersonne([FromBody] PersonneViewModel personneVM)
        {
            if(GetPersonne(personneVM.FirstName, personneVM.LastName).Id != 0)
            {
                _logger.LogWarning("Personne already exists");
                return Results.BadRequest("Personne already exists");
            }
            _logger.LogInformation("Creating personne");
            var created = _personneBusiness.InsertPersonne(personneVM.Adapt<PersonneDto>());
            _logger.LogInformation($"Personne {created.Id} created");
            return Results.Created($"/personnes/{created.Id}", created);
        }
        
        [HttpPut("{id}")]
        public async Task<IResult> UpdatePersonne([FromRoute] int id, [FromBody] PersonneViewModel personneVM)
        {
            _logger.LogInformation($"Updating personne {id}");
            _personneBusiness.UpdatePersonne(id, personneVM.Adapt<PersonneDto>());
            _logger.LogInformation($"Personne {id} updated");
            return Results.NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IResult> DeletePersonne([FromRoute] int id)
        {
            _logger.LogInformation($"Deleting personne {id}");
            _personneBusiness.DeletePersonne(id);
            _logger.LogInformation($"Personne {id} deleted");
            return Results.Ok();
        }
    }
}