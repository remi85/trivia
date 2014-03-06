using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia
{
    public class Player
    {
        public string Name { get; set; }
        public int Location { get; set; }
        public int Score { get; set; }
        public bool IsInPenalty { get; set; }

        public Player(string name)
        {
            Name = name;
        }

    }
}
