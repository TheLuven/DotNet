using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Emprunt
{
    public interface IEmpruntRepository
    {
        Task<List<BluRayDto>> GetEmpruntsDispo();
        
        Task<BluRayDto> GetEmprunt(int id);
    }
}