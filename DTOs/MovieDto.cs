namespace MoviesMinimalAPI.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool onCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Poster { get; set; }

        // comments dto
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();

        // genders dto
        public List<GenderDto> Genders { get; set; } = new List<GenderDto>();

        // actorsmovies dto
        public List<ActorMovieDto> Actors { get; set; } = new List<ActorMovieDto>();
    }
}
