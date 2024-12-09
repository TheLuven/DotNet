using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Emprunt
{
    public interface IEmpruntBusiness
    {
        Task<List<EmpruntPauvreDto>> GetEmpruntsDispo();
        Task<EmpruntRicheDto> GetEmprunt(int id);
        
        void UpdateEmprunt(int id, BluRayDto film);
    }
}