using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Account
    {
        public string Username { get; set; }
        public Character PlayCharacter { get; set; }

        private List<Character> characters = new List<Character>();

        public void AddCharacter(string name, int level, int spec)
        {
            characters.Add(new Character(name, level, spec));
        }

        public List<Character> GetCharacters()
        {
            return characters;
        }
    }
}
