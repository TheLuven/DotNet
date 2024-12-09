using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.AgeRating
{
    public class AgeRatingRepository: IAgeRatingRepository
    {
        private readonly VideothequeDb _db;

        public AgeRatingRepository(VideothequeDb db)
        {
            _db = db;
        }

        public Task<List<AgeRatingDto>> GetAgeRatings() => _db.AgeRatings.ToListAsync();

        public ValueTask<AgeRatingDto?> GetAgeRating(int id) => _db.AgeRatings.FindAsync(id);
        
        public Task<AgeRatingDto?> GetAgeRating(string filmVmAgeRating) => _db.AgeRatings.FirstOrDefaultAsync(a => a.Name == filmVmAgeRating);

        public async Task InsertAgeRating(AgeRatingDto ageRating)
        {
            await _db.AgeRatings.AddAsync(ageRating);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAgeRating(int id, AgeRatingDto ageRating)
        {
            var ageRatingToUpdate = _db.AgeRatings.FindAsync(id).Result;
            if (ageRatingToUpdate is null)
            {
                throw new KeyNotFoundException($"Age rating '{id}' non trouvé");
            }
            ageRatingToUpdate.Name = ageRating.Name;
            ageRatingToUpdate.Abreviation = ageRating.Abreviation;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAgeRating(int id)
        {
            var ageRatingToDelete = _db.AgeRatings.FindAsync(id).Result;
            if (ageRatingToDelete is null)
            {
                throw new KeyNotFoundException($"Age rating '{id}' non trouvé");
            }
            _db.AgeRatings.Remove(ageRatingToDelete);
            await _db.SaveChangesAsync();
        }
        
        

    }
}
