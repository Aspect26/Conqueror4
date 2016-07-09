using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Account
    {
        public string Username { get; set; }
        public MainPlayerCharacter PlayCharacter { get; set; }

        private List<MainPlayerCharacter> characters = new List<MainPlayerCharacter>();

        public void AddCharacter(string name, int level, int spec)
        {
            characters.Add(new MainPlayerCharacter(name, level, spec));
        }

        public List<MainPlayerCharacter> GetCharacters()
        {
            return characters;
        }
    }
}
