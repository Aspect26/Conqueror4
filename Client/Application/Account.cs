using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Account
    {
        public string Username { get; set; }
        public PlayerCharacter PlayCharacter { get; set; }

        private List<PlayerCharacter> characters = new List<PlayerCharacter>();

        public void AddCharacter(string name, int level, int spec)
        {
            characters.Add(new PlayerCharacter(name, level, spec));
        }

        public List<PlayerCharacter> GetCharacters()
        {
            return characters;
        }
    }
}
