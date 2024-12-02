using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Support
{
    public interface ISupportRepository
    {
        Task<List<SupportDto>> GetSupports();

        ValueTask<SupportDto?> GetSupport(int id);
    }
}