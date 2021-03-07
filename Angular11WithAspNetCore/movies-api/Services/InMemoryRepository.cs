using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.Entities;

namespace MoviesAPI.Services
{
    public class InMemoryRepository : IRepository
    {
        private List<Genre> genres;

        public InMemoryRepository()
        {
            this.genres = new List<Genre>()
            {
                new Genre() { Id = 1, Name = "Comedy" },
                new Genre() { Id = 2, Name = "Action" },
            };
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return this.genres;
        }

        public Genre GetGenreById(int id)
        {
            return this.genres.FirstOrDefault(x => x.Id == id);
        }

        public void AddGenre(Genre genre)
        {
            genre.Id = this.genres.Max(x => x.Id) + 1;
            this.genres.Add(genre);
        }
    }
}