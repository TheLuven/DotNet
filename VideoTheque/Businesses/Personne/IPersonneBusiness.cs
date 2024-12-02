using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Personne
{
    public interface IPersonneBusiness
    {
        Task<List<PersonneDto>> GetPersonnes();
        
        PersonneDto GetPersonne(int id);
        
        PersonneDto InsertPersonne(PersonneDto personne);
        
        void UpdatePersonne(int id, PersonneDto personne);
        
        void DeletePersonne(int id);
    }
}