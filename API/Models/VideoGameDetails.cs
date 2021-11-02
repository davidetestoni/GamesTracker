namespace API.Models
{
    public class VideoGameDetails
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CoverId { get; set; }

        public VideoGameDetails(long id)
        {
            Id = id;
        }
    }
}
