using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class Character : IUnit
    {
        public string Name { get; set; }
        public int Spec { get; set; }
        public int Level { get; set; }
        public Location Location { get; set; }

        public Character(string name, int spec)
        {
            this.Name = name;
            this.Spec = spec;
            this.Level = 1;

            this.Location = Data.GetStartingLocation(spec);
        }
    }
}
