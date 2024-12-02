using Microsoft.EntityFrameworkCore;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.PersonneRepository
{
    public class PersonneRepository : IPersonneRepository
    {
        private readonly VideothequeDb _db;
        
        public PersonneRepository(VideothequeDb db)
        {
            _db = db;
        }
        
        public Task<List<PersonneDto>> GetPersonnes() => _db.Personnes.ToListAsync();

        public ValueTask<PersonneDto> GetPersonne(int id) => _db.Personnes.FindAsync(id);

        public Task DeletePersonne(int id)
        {
            var personneToDelete = _db.Personnes.FindAsync(id).Result;
            if (personneToDelete is null)
            {
                throw new KeyNotFoundException($"Personne '{id}' non trouvée");
            }
            _db.Personnes.Remove(personneToDelete);
            return _db.SaveChangesAsync();
        }

        public Task UpdatePersonne(int id, PersonneDto personne)
        {
            var personneToUpdate = _db.Personnes.FindAsync(id).Result;
            if (personneToUpdate is null)
            {
                throw new KeyNotFoundException($"Personne '{id}' non trouvée");
            }

            personneToUpdate.Nationality = personne.Nationality;
            personneToUpdate.BirthDay = personne.BirthDay;
            personneToUpdate.FirstName = personne.FirstName;
            personneToUpdate.LastName = personne.LastName;
            
            return _db.SaveChangesAsync();
        }

        public Task InsertPersonne(PersonneDto personne)
        {
            _db.Personnes.AddAsync(personne);
            return _db.SaveChangesAsync();
        }
    }
}