using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Film
{
    public interface IBluRayRepository
    {
        Task<List<BluRayDto>> GetBluRays();
        
        ValueTask<BluRayDto?> GetBluRay(int id);
        
        Task<BluRayDto?> GetBluRay(string title);
        
        Task InsertBluRay(BluRayDto bluRay);
        
        Task UpdateBluRay(int id, BluRayDto bluRay);
        
        Task DeleteBluRay(int id);
    }
}