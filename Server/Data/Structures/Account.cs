using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// A simple class representing one account. 
    /// </summary>
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account is currently logged
        /// in the game.
        /// </summary>
        /// <value><c>true</c> if logged in; otherwise, <c>false</c>.</value>
        public bool LoggedIn { get; set; }

        private List<Character> characters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="Username">The username.</param>
        /// <param name="Password">The password.</param>
        public Account(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;

            this.characters = new List<Character>();
            this.LoggedIn = false;
        }

        /// <summary>
        /// Returns characters associated with this account.
        /// </summary>
        /// <returns>List of characters.</returns>
        public List<Character> GetCharacters()
        {
            return characters;
        }

        /// <summary>
        /// Associates the given character to this accounts.
        /// </summary>
        /// <param name="character">The character.</param>
        public void AddCharacter(Character character)
        {
            this.characters.Add(character);
        }
    }
}
