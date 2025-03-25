namespace MoviesMinimalAPI.Entities
{
    public class GenderMovie
    {
        public Gender Gender { get; set; } = null!;
        public int GenderId { get; set; }

        public Movie Movie { get; set; } = null!;
        public int MovieId { get; set; }
    }
}