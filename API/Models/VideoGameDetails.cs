namespace API.Models
{
    public class VideoGameDetails
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CoverId { get; set; }
        public int? Year { get; set; } = null;
        public string[] Genres { get; set; }
        public string Summary { get; set; }
        public VideoGameScreenshot[] Screenshots { get; set; }

        public VideoGameDetails(long id)
        {
            Id = id;
        }
    }
}
