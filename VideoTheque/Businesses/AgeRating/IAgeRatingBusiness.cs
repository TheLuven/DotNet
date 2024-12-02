using VideoTheque.DTOs;

namespace VideoTheque.Businesses.AgeRating
{
    public interface IAgeRatingBusiness
    {
        Task<List<AgeRatingDto>> GetAgeRatings();

        AgeRatingDto GetAgeRating(int id);

        AgeRatingDto InsertAgeRating(AgeRatingDto ageRating);

        void UpdateAgeRating(int id, AgeRatingDto ageRating);

        void DeleteAgeRating(int id);
    }
}
