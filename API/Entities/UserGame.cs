using System;

namespace API.Entities
{
    public class UserGame
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }

        public Game Game { get; set; }
        public long GameId { get; set; }

        public UserGameStatus Status { get; set; } = UserGameStatus.Playing;
        public DateTime? FinishedOn { get; set; } = null;
        public int? UserRating { get; set; } = null;
    }

    public enum UserGameStatus
    {
        ToPlay,
        Playing,
        Dropped,
        Finished
    }
}
