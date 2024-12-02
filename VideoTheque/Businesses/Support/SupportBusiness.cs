using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Support;

namespace VideoTheque.Businesses.Support
{
    public class SupportBusiness: ISupportBusiness
    
    {
        private readonly ISupportRepository _supportDao;
        
        public SupportBusiness(ISupportRepository supportDao)
        {
            _supportDao = supportDao;
        }
        
        public async Task<List<SupportDto>> GetSupports()
        {
            return await _supportDao.GetSupports();
        }
        
        public SupportDto GetSupport(int id)
        {
            var support = _supportDao.GetSupport(id).Result;
            if (support == null)
            {
                throw new NotFoundException($"Support '{id}' non trouvé");
            }
            return support;
        }
    }
}