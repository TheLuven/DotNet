using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.Context;
using VideoTheque.Core;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Genres
{
    public class GenresRepository : IGenresRepository
    {
        private readonly VideothequeDb _db;

        public GenresRepository(VideothequeDb db)
        {
            _db = db;
        }

        public Task<List<GenreDto>> GetGenres() => _db.Genres.ToListAsync();

        public ValueTask<GenreDto?> GetGenre(int id) => _db.Genres.FindAsync(id);
        
        public async Task<GenreDto?> GetGenre(string filmVmGenre)
        {
            return await _db.Genres.FirstOrDefaultAsync(g => g.Name == filmVmGenre) ?? throw new NotFoundException();
        }

        public Task InsertGenre(GenreDto genre) 
        {
            _db.Genres.AddAsync(genre);
            return _db.SaveChangesAsync();
        }

        public Task UpdateGenre(int id, GenreDto genre)
        {
            var genreToUpdate = _db.Genres.FindAsync(id).Result;

            if (genreToUpdate is null)
            {
                throw new KeyNotFoundException($"Genre '{id}' non trouvé");
            }

            genreToUpdate.Name = genre.Name;
            return _db.SaveChangesAsync();
        }

        public Task DeleteGenre(int id)
        {
            var genreToDelete = _db.Genres.FindAsync(id).Result;

            if (genreToDelete is null)
            {
                throw new KeyNotFoundException($"Genre '{id}' non trouvé");
            }

            _db.Genres.Remove(genreToDelete);
            return _db.SaveChangesAsync();
        }
    }
}
