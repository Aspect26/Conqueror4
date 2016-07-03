using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Character
    {
        public string Name { get; set; }
        public int Spec { get; set; }
        public int Level { get; set; }

        public Character(string name, int spec)
        {
            this.Name = name;
            this.Spec = spec;
            this.Level = 1;
        }
    }
}
