using VideoTheque.Businesses.Personne;
using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.PersonneRepository;

namespace VideoTheque.Businesses.Personne
{
    public class PersonneBusiness : IPersonneBusiness
    {
        private readonly IPersonneRepository _personneDao;
        
        public PersonneBusiness(IPersonneRepository personneDao)
        {
            _personneDao = personneDao;
        }
        
        public Task<List<PersonneDto>> GetPersonnes() => _personneDao.GetPersonnes();

        public PersonneDto GetPersonne(int id)
        {
            var personne = _personneDao.GetPersonne(id).Result;

            if (personne == null)
            {
                throw new NotFoundException($"Personne '{id}' non trouvée");
            }
            return personne;
        }

        public PersonneDto InsertPersonne(PersonneDto personne)
        {
            if (_personneDao.InsertPersonne(personne).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion de la personne {personne.FirstName}");
            }
            return personne;
        }

        public void UpdatePersonne(int id, PersonneDto personne)
        {
            if (_personneDao.UpdatePersonne(id, personne).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification de la personne {personne.FirstName}");
            }
        }

        public void DeletePersonne(int id)
        {
            if (_personneDao.DeletePersonne(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression de la personne d'identifiant {id}");
            }
        }
    }
}