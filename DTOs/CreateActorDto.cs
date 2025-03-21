namespace MoviesMinimalAPI.DTOs
{
    public class CreateActorDto
    {
        public string Name { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public IFormFile? Photo { get; set; } // resprents a file sent with the HttpRequest
    }
}
