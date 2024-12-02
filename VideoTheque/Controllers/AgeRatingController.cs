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
        public async Task<List<AgeRatingViewModel>> GetAgeRatings() => (await _ageRatingBusiness.GetAgeRatings()).Adapt<List<AgeRatingViewModel>>();

        [HttpGet("{id}")]
        public async Task<AgeRatingViewModel> GetAgeRating([FromRoute] int id) => _ageRatingBusiness.GetAgeRating(id).Adapt<AgeRatingViewModel>();

        [HttpPost]
        public async Task<IResult> InsentAgeRating([FromBody] AgeRatingViewModel ageRatingVM)
        {
            var created = _ageRatingBusiness.InsertAgeRating(ageRatingVM.Adapt<AgeRatingDto>());
            return Results.Created($"/age-rating/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateAgeRating([FromRoute] int id, [FromBody] AgeRatingViewModel ageRatingVM)
        {
            _ageRatingBusiness.UpdateAgeRating(id, ageRatingVM.Adapt<AgeRatingDto>());
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteAgeRating([FromRoute] int id)
        {
            _ageRatingBusiness.DeleteAgeRating(id);
            return Results.Ok();
        }
    }
}
