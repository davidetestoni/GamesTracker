﻿using System.Collections.Generic;

namespace API.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }

        public ICollection<UserGame> PlayedBy { get; set; }
    }
}
