namespace MoviesMinimalAPI.DTOs
{
    public class CreateMovieDto
    {
        public string Title { get; set; } = null!;
        public bool onCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile? Poster { get; set; }
    }
}
