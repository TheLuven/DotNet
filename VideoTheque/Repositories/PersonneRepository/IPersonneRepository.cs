using VideoTheque.DTOs;

namespace VideoTheque.Repositories.PersonneRepository
{
    public interface IPersonneRepository
    {
        Task<List<PersonneDto>> GetPersonnes();
        
        ValueTask<PersonneDto?> GetPersonne(int id);
        
        Task InsertPersonne(PersonneDto personne);
        
        Task UpdatePersonne(int id, PersonneDto personne);
        
        Task DeletePersonne(int id);
    }
}