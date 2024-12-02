using Microsoft.EntityFrameworkCore;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Film
{
    public class BluRayRepository : IBluRayRepository
    {
        
        private readonly VideothequeDb _db;
        
        public BluRayRepository(VideothequeDb db)
        {
            _db = db;
        }
        public Task<List<BluRayDto>> GetBluRays() => _db.BluRays.ToListAsync();

        public Task InsertBluRay(BluRayDto bluRay)
        {
            _db.BluRays.AddAsync(bluRay);
            return _db.SaveChangesAsync();
        }

        public Task DeleteBluRay(Int32 id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBluRay(int id, BluRayDto bluRay)
        {
            var bluRayToUpdate = _db.BluRays.FindAsync(id).Result;
            
            if (bluRayToUpdate is null)
            {
                throw new KeyNotFoundException($"BluRay '{id}' non trouvé");
            }
            
            bluRayToUpdate.Title = bluRay.Title;
            bluRayToUpdate.Duration = bluRay.Duration;
            bluRayToUpdate.IdFirstActor = bluRay.IdFirstActor;
            bluRayToUpdate.IdDirector = bluRay.IdDirector;
            bluRayToUpdate.IdScenarist = bluRay.IdScenarist;
            bluRayToUpdate.IdAgeRating = bluRay.IdAgeRating;
            bluRayToUpdate.IdGenre = bluRay.IdGenre;
            bluRayToUpdate.IsAvailable = bluRay.IsAvailable;
            bluRayToUpdate.IdOwner = bluRay.IdOwner;
            
            return _db.SaveChangesAsync();
        }

        public ValueTask<BluRayDto?> GetBluRay(int id) => _db.BluRays.FindAsync(id);
    }
}