namespace API.DTOs
{
    public class GameInfoDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public int? Year { get; set; }
    }
}
