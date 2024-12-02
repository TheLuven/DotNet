using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Support
{
    public class SupportRepository: ISupportRepository
    {
        //private readonly VideothequeDb _db;

		public SupportRepository()
	    {
			//_db = db;
        }

        public async Task<List<SupportDto>> GetSupports()
        {
            /*return await _db.Supports.Select(s => new SupportDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToListAsync();*/
			var supportDtos = SupportEnum.GetValues(typeof(SupportEnum)).Cast<SupportEnum>().Select(s => new SupportDto
            {
                Id = (int)s,
                Name = s.ToString()
            }).ToList();
    		return await Task.FromResult(supportDtos);
        }

        public async ValueTask<SupportDto?> GetSupport(int id)
        {
            /*var support = await _db.Supports.FindAsync(id);
            return support == null ? null : new SupportDto
            {
                Id = support.Id,
                Name = support.Name
            };*/
			var support = SupportEnum.GetValues(typeof(SupportEnum)).Cast<SupportEnum>().FirstOrDefault(s => (int)s == id);
			return support == null ? null : new SupportDto
            {
                Id = (int)support,
                Name = support.ToString()
            };
        }
        
    }

	
}