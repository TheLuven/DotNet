using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Emprunt;
using VideoTheque.Repositories.PersonneRepository;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.Film;
using VideoTheque.Repositories.AgeRating;

namespace VideoTheque.Businesses.Emprunt
{
    public class EmpruntBusiness : IEmpruntBusiness
    {
        private readonly IEmpruntRepository _empruntRepository;
        private readonly IPersonneRepository _personRepository;
        private readonly IGenresRepository _genreRepository;
        private readonly IBluRayRepository _bluRayRepository;
        private readonly IAgeRatingRepository _ageRatingRepository;
        
        public EmpruntBusiness(IEmpruntRepository empruntRepository, IPersonneRepository personneRepository, IGenresRepository genresRepository, IBluRayRepository bluRayRepository, IAgeRatingRepository ageRatingRepository)
        {
            _empruntRepository = empruntRepository;
            _personRepository = personneRepository;
            _genreRepository = genresRepository;
            _bluRayRepository = bluRayRepository;
            _ageRatingRepository = ageRatingRepository;
        }
        
        public async Task<List<EmpruntPauvreDto>> GetEmpruntsDispo()
        {
            var films = await _empruntRepository.GetEmpruntsDispo();
            var filmsDispo = new List<EmpruntPauvreDto>();

            foreach (var film in films)
            { 
                var director = await _personRepository.GetPersonne(film.IdDirector);
                var firstActor = await _personRepository.GetPersonne(film.IdFirstActor);
                var genre = await _genreRepository.GetGenre(film.IdGenre);

                var empruntDDto = new EmpruntPauvreDto
                {
                    Id = film.Id,
                    Title = film.Title,
                    Genre = genre.Name,
                    Director = director.FirstName + " " + director.LastName,
                    FirstActor = firstActor.FirstName + " " + firstActor.LastName
                };

                filmsDispo.Add(empruntDDto);
            }

            return filmsDispo;
        }

        public async Task<EmpruntRicheDto> GetEmprunt(int id)
        {
            var film = await _bluRayRepository.GetBluRay(id);
            var director = await _personRepository.GetPersonne(film.IdDirector);
            var firstActor = await _personRepository.GetPersonne(film.IdFirstActor);
            var scenarist = await _personRepository.GetPersonne(film.IdScenarist);
            var genre = await _genreRepository.GetGenre(film.IdGenre);
            var ageRating = await _ageRatingRepository.GetAgeRating(film.IdAgeRating);
            
            var directorWithoutId = new PersonneDto 
            { 
                FirstName = director.FirstName, 
                LastName = director.LastName, 
                Nationality = director.Nationality, 
                BirthDay = director.BirthDay 
            };            
            
            var firstActorWithoutId = new PersonneDto
            {
                FirstName = firstActor.FirstName,
                LastName = firstActor.LastName,
                Nationality = firstActor.Nationality,
                BirthDay = firstActor.BirthDay
            };

            var scenaristWithoutId = new PersonneDto
            {
                FirstName = scenarist.FirstName,
                LastName = scenarist.LastName,
                Nationality = scenarist.Nationality,
                BirthDay = scenarist.BirthDay
            };
            
            
           
            var genreWithoutId = new GenreDto {Name = genre.Name };
            var ageRatingWithoutId = new AgeRatingDto { Abreviation = ageRating.Abreviation, Name = ageRating.Name  };

            var emprunt = new EmpruntRicheDto();
            emprunt.Title = film.Title;
            emprunt.Duration = film.Duration;
            emprunt.Genre = genreWithoutId;
            emprunt.Director = directorWithoutId;
            emprunt.FirstActor = firstActorWithoutId;
            emprunt.Scenarist = scenaristWithoutId;
            emprunt.AgeRating = ageRatingWithoutId;
            
            return emprunt;

        }
        
        public async Task<EmpruntPauvreDto> AddEmprunt(int id)
        {
            var film = await _bluRayRepository.GetBluRay(id);
			if (film.IsAvailable == false){
				return null;
			}
            film.IsAvailable = false;
            await _bluRayRepository.UpdateBluRay(id,film);

            var filmDto = new EmpruntPauvreDto();
            filmDto.Id = film.Id;
            filmDto.Title = film.Title;

            return filmDto;

        }
        
        public async void DeleteEmprunt(string title)
        {
            var film = await _bluRayRepository.GetBluRay(title);
			if (film.IsAvailable == true){
			    throw new InvalidOperationException("Film is not borrowed.");
			}
            film.IsAvailable = true;
            await _bluRayRepository.UpdateBluRay(film.Id,film);
        }
    }
}