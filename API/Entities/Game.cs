using System.Collections.Generic;

namespace API.Entities
{
    public class Game
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CoverId { get; set; }
        public int? Year { get; set; }

        public ICollection<UserGame> PlayedBy { get; set; }
    }
}
