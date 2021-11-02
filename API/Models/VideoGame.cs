namespace API.Models
{
    public class VideoGame
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CoverId { get; set; }
        public int? Year { get; set; } = null;

        public VideoGame(long id)
        {
            Id = id;
        }
    }
}
