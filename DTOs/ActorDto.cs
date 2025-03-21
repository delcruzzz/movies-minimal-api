namespace MoviesMinimalAPI.DTOs
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string? Photo { get; set; }
    }
}
