using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Spec { get; set; }

        public Location Location { get; set; }

        public Character(string name, int level, int spec)
        {
            this.Name = name;
            this.Level = level;
            this.Spec = spec;

            Location = new Location();
        }
    }
}
