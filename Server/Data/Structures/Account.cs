using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }

        private List<Character> characters;

        public Account(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;

            this.characters = new List<Character>();
            this.LoggedIn = false;
        }

        public List<Character> GetCharacters()
        {
            return characters;
        }

        public void AddCharacter(Character character)
        {
            this.characters.Add(character);
        }
    }
}
