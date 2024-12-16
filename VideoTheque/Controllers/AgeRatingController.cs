namespace VideoTheque.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.AgeRating;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;

    [ApiController]
    [Route("age-ratings")]
    public class AgeRatingController : ControllerBase
    {
        private readonly IAgeRatingBusiness _ageRatingBusiness;
        protected readonly ILogger<AgeRatingController> _logger;

        public AgeRatingController(ILogger<AgeRatingController> logger, IAgeRatingBusiness ageRatingBusiness)
        {
            _logger = logger;
            _ageRatingBusiness = ageRatingBusiness;
        }

        [HttpGet]
        public async Task<List<AgeRatingViewModel>> GetAgeRatings()
        {
            _logger.LogInformation("Getting all age ratings");
            return (await _ageRatingBusiness.GetAgeRatings()).Adapt<List<AgeRatingViewModel>>();
        }

        [HttpGet("{id}")]
        public async Task<AgeRatingViewModel> GetAgeRating([FromRoute] int id)
        {
            _logger.LogInformation($"Getting age rating {id}");
            return _ageRatingBusiness.GetAgeRating(id).Adapt<AgeRatingViewModel>();
        }

        [HttpPost]
        public async Task<IResult> InsentAgeRating([FromBody] AgeRatingViewModel ageRatingVM)
        {
            _logger.LogInformation("Creating age rating");
            var created = _ageRatingBusiness.InsertAgeRating(ageRatingVM.Adapt<AgeRatingDto>());
            _logger.LogInformation($"Age rating {created.Id} created");
            return Results.Created($"/age-rating/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateAgeRating([FromRoute] int id, [FromBody] AgeRatingViewModel ageRatingVM)
        {
            _logger.LogInformation($"Updating age rating {id}");
            _ageRatingBusiness.UpdateAgeRating(id, ageRatingVM.Adapt<AgeRatingDto>());
            _logger.LogInformation($"Age rating {id} updated");
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteAgeRating([FromRoute] int id)
        {
            _logger.LogInformation($"Deleting age rating {id}");
            _ageRatingBusiness.DeleteAgeRating(id);
            _logger.LogInformation($"Age rating {id} deleted");
            return Results.Ok();
        }
    }
}
