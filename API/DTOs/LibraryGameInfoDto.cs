using API.Entities;
using System;

namespace API.DTOs
{
    public class LibraryGameInfoDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public int? Year { get; set; }

        public UserGameStatus Status { get; set; } = UserGameStatus.Playing;
        public DateTime? FinishedOn { get; set; } = null;
        public int? UserRating { get; set; } = null;
    }
}
