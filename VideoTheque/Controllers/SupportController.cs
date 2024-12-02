namespace VideoTheque.Controllers
{
	
	using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.Support;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;

	[ApiController]
	[Route("supports")]
    public class SupportController: ControllerBase
    {
		private readonly ISupportBusiness _supportBusiness; 
		private readonly ILogger<SupportController> _logger;

		public SupportController(ILogger<SupportController> logger, ISupportBusiness supportBusiness)
	    {
	        _logger = logger;
			_supportBusiness = supportBusiness;       
		}
		
		[HttpGet]
		public async Task<List<SupportViewModel>> GetSupports() => (await _supportBusiness.GetSupports()).Adapt<List<SupportViewModel>>();
		
		[HttpGet("{id}")]
		public async Task<SupportViewModel> GetSupport([FromRoute] int id) => _supportBusiness.GetSupport(id).Adapt<SupportViewModel>();
    }
}