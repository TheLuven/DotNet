using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Support
{
    public interface ISupportBusiness
    {
        Task<List<SupportDto>> GetSupports();

        SupportDto GetSupport(int id);
        
    }
}