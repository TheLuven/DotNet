using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Film
{
    public interface IBluRayBusiness
    {
       Task<List<FilmDto>> GetBluRays();
       
       FilmDto GetBluRay(int id);
       
       BluRayDto InsertBluRay(BluRayDto film);
       
       void UpdateBluRay(int id, BluRayDto film);
       
       Task DeleteBluRay(int id);
       
       Task<List<EmpruntPauvreDto?>> GetEmpruntAvailable(int id);
       
        Task<BluRayDto> AddFilmByEmprunt(int idHost, int idFilm);
    }
}