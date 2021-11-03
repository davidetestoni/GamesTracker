using API.Entities;
using System;

namespace API.DTOs
{
    public class UpdateGameDto
    {
        public long Id { get; set; }
        public UserGameStatus Status { get; set; } = UserGameStatus.Playing;
        public DateTime? FinishedOn { get; set; } = null;
        public int? UserRating { get; set; } = null;
    }
}
