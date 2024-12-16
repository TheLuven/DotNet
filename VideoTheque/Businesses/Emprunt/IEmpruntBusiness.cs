using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Emprunt
{
    public interface IEmpruntBusiness
    {
        Task<List<EmpruntPauvreDto>> GetEmpruntsDispo();
        Task<EmpruntRicheDto> GetEmprunt(int id);
        
        void DeleteEmprunt(int id);

        Task<EmpruntPauvreDto> AddEmprunt(int id);
    }
}