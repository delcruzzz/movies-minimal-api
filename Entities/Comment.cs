namespace MoviesMinimalAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; } = null!;
        
        // add 'foreign key' for a movie entity
        public int MovieId { get; set; }
    }
}