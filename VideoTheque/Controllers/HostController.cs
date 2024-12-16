namespace VideoTheque.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.Host;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;
    
    [ApiController]
    [Route("hosts")] 
    public class HostController : ControllerBase
    {
        private readonly IHostBusiness _hostBusiness;
        protected readonly ILogger<HostController> _logger;
        
        public HostController(ILogger<HostController> logger, IHostBusiness hostBusiness)
        {
            _logger = logger;
            _hostBusiness = hostBusiness;
        }

        [HttpGet]
        public async Task<List<HostViewModel>> GetHosts()
        {
            _logger.LogInformation("Getting all hosts");
            return (await _hostBusiness.GetHosts()).Adapt<List<HostViewModel>>();
        }

        [HttpGet("{id}")]
        public async Task<HostViewModel> GetHost([FromRoute] int id)
        {
            _logger.LogInformation($"Getting host {id}");
            return _hostBusiness.GetHost(id).Adapt<HostViewModel>();
        }

        [HttpPost]
        public async Task<IResult> InsentHost([FromBody] HostViewModel hostVM)
        {
            _logger.LogInformation("Creating host");
            var created = _hostBusiness.InsertHost(hostVM.Adapt<HostDto>());
            _logger.LogInformation($"Host {created.Id} created");
            return Results.Created($"/host/{created.Id}", created);
        }
        
        [HttpPut("{id}")]
        public async Task<IResult> UpdateHost([FromRoute] int id, [FromBody] HostViewModel hostVM)
        {
            _logger.LogInformation($"Updating host {id}");
            _hostBusiness.UpdateHost(id, hostVM.Adapt<HostDto>());
            _logger.LogInformation($"Host {id} updated");
            return Results.NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteHost([FromRoute] int id)
        {
            _logger.LogInformation($"Deleting host {id}");
            _hostBusiness.DeleteHost(id);
            _logger.LogInformation($"Host {id} deleted");
            return Results.Ok();
        }
    }
}