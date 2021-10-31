using System;

namespace API.Entities
{
    public class UserGame
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }

        public Game Game { get; set; }
        public int GameId { get; set; }

        public bool Finished { get; set; }
        public DateTime FinishedOn { get; set; }
        public int UserRating { get; set; }
    }
}
