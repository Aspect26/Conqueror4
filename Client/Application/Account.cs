using Shared;
using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// Holds information about an account. Contains username, list of all characters belonging to that account and a 
    /// character which user choose to play with
    /// </summary>
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

            // the character is loaded later -> when the PlayScreen is being created
            this.PlayCharacter = new PlayedCharacter(server, character.Name, character.Level, character.Spec, -1, 
                new BaseStats(), new BaseStats(), 0);
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
