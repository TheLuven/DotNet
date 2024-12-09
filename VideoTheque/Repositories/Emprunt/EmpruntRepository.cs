using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Emprunt
{
    public class EmpruntRepository: IEmpruntRepository
    {
        private readonly VideothequeDb _db;
        
        public EmpruntRepository(VideothequeDb db)
        {
            _db = db;
        }
        
        public Task<List<BluRayDto>> GetEmpruntsDispo() => _db.BluRays.Where(b => b.IdOwner == null && b.IsAvailable).ToListAsync();
        
        public Task<BluRayDto> GetEmprunt(int id) => _db.BluRays.FindAsync(id);
    }
}