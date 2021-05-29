namespace MoviesAPI.Entities
{
    public class MoviesGenres
    {
        public int MovieId { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public Movie Movie { get; set; }
    }
}