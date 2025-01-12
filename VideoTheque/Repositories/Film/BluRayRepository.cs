using Microsoft.EntityFrameworkCore;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Film
{
    public class BluRayRepository : IBluRayRepository
    {
        
        private readonly VideothequeDb _db;
        private readonly ILogger<BluRayRepository> _logger;
        
        public BluRayRepository(VideothequeDb db,ILogger<BluRayRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public Task<List<BluRayDto>> GetBluRays() => _db.BluRays.ToListAsync();
        
        public Task<BluRayDto?> GetBluRay(string title) => _db.BluRays.FirstOrDefaultAsync(b => b.Title == title);

        public Task InsertBluRay(BluRayDto bluRay)
        {
            _db.BluRays.AddAsync(bluRay);
            return _db.SaveChangesAsync();
        }

        public async Task DeleteBluRay(Int32 id)
        {
            _logger.LogInformation($"Commencement de la suppression du BluRay '{id}'");
            var bluRayToDelete = await _db.BluRays.FindAsync(id);
            _logger.LogInformation($"BluRay '{id}' trouvé");
            if (bluRayToDelete is null)
            {
                throw new KeyNotFoundException($"BluRay '{id}' non trouvé");
            }
            _logger.LogInformation($"delete id '{id}' trouvé");

            /*if (bluRayToDelete.IsAvailable == false)
            {
                throw new InvalidOperationException($"BluRay '{id}' non disponible");
            }*/

            _db.BluRays.Remove(bluRayToDelete);
            _logger.LogInformation($"BluRay '{id}' supprimé");
            await _db.SaveChangesAsync();
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
        
        public async Task<bool> HasBluRayByOwner(int idOwner)
        {
            var bluRay = await _db.BluRays.FirstOrDefaultAsync(b => b.IdOwner == idOwner);
            return bluRay != null;
        }
    }
}