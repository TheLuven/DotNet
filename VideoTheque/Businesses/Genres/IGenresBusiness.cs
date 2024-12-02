using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Genres
{
    public interface IGenresBusiness
    {
        Task<List<GenreDto>> GetGenres();

        GenreDto GetGenre(int id);

        GenreDto InsertGenre(GenreDto genre);

        void UpdateGenre(int id, GenreDto genre);

        void DeleteGenre(int id);
    }
}
