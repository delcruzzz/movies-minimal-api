namespace MoviesMinimalAPI.DTOs
{
    public class CreateCommentDto
    {
        public string Body { get; set; } = null!;
        public int MovieId { get; set; }
    }
}