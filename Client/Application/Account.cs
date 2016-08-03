using Shared;
using System.Collections.Generic;

namespace Client
{
    public class Account
    {
        public string Username { get; set; }
        public PlayedCharacter PlayCharacter { get; set; }

        private List<Character> characters = new List<Character>();

        public void SetPlayedCharacter(string name, ServerConnection server)
        {
            Character character = null;

            foreach(Character current in characters)
            {
                if(current.Name == name)
                {
                    character = current;
                }
            }

            this.PlayCharacter = new PlayedCharacter(server, character.Name, character.Level, character.Spec, -1, 
                new BaseStats(0), new BaseStats(0), 0);
        }

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
