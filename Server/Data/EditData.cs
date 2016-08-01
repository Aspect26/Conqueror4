using Shared;

namespace Server
{
    public partial class Data
    {
        private Game game;

        public Data(Game game)
        {
            this.game = game;
        }

        public bool RegisterAccount(string username, string password)
        {
            username = username.ToLower();
            password = password.ToLower();

            if (accountData.ContainsKey(username))
                return false;

            accountData.Add(username, new Account(username, password));
            return true;
        }

        public Account LoginAccount(string username, string password)
        {
            username = username.ToLower();
            password = password.ToLower();

            if (!accountData.ContainsKey(username))
                return null;

            if (accountData[username].Password != password)
                return null;

            if (accountData[username].LoggedIn) //TODO: disconnect account
                return null;

            accountData[username].LoggedIn = true;
            return accountData[username];
        }

        public Character CreateCharacter(string username, string name, int spec)
        {
            name = name.ToLower();
            if (characterData.ContainsKey(name))
                return null;

            if (!accountData.ContainsKey(username))
                return null;

            Location startingLocation = GetStartingLocation(spec);
            Character newChar = new Character(name, spec, -1, startingLocation, 
                game.GetMapInstance(startingLocation.MapID));
            characterData.Add(name, newChar);
            accountData[username].AddCharacter(newChar);

            return newChar;
        }
    }
}
