using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Film
{
    public interface IBluRayBusiness
    {
       Task<List<FilmDto>> GetBluRays();
       
       FilmDto GetBluRay(int id);
       
       BluRayDto InsertBluRay(BluRayDto film);
       
       void UpdateBluRay(int id, BluRayDto film);
       
       void DeleteBluRay(int id);
    }
}