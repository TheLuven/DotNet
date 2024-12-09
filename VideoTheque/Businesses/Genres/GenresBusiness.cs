using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Genres;

namespace VideoTheque.Businesses.Genres
{
    public class GenresBusiness : IGenresBusiness
    {
        private readonly IGenresRepository _genreDao;

        public GenresBusiness(IGenresRepository genreDao)
        {
            _genreDao = genreDao;
        }

        public Task<List<GenreDto>> GetGenres() => _genreDao.GetGenres();

        public GenreDto GetGenre(int id)
        {
            var genre = _genreDao.GetGenre(id).Result;

            if (genre == null)
            {
                throw new NotFoundException($"Genre '{id}' non trouvé");
            }

            return genre;
        }
        
        public GenreDto GetGenre(string filmVmGenre)
        {
            var genre = _genreDao.GetGenre(filmVmGenre).Result;

            if (genre == null)
            {
                throw new NotFoundException($"Genre '{filmVmGenre}' non trouvé");
            }

            return genre;
        }

        public GenreDto InsertGenre(GenreDto genre)
        {
            if (_genreDao.InsertGenre(genre).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du genre {genre.Name}");
            }

            return genre;
        }

        public void UpdateGenre(int id, GenreDto genre)
        {
            if (_genreDao.UpdateGenre(id, genre).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du genre {genre.Name}");
            }
        }
                

        public void DeleteGenre(int id)
        {
            if (_genreDao.DeleteGenre(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du genre d'identifiant {id}");
            }
        }
    }
}
