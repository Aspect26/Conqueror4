using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Data
    {
        Dictionary<string, Account> accountData = new Dictionary<string, Account>();
        Dictionary<string, Character> characterData = new Dictionary<string, Character>();

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

            Character newChar = new Character(name, spec);
            characterData.Add(name, newChar);
            accountData[username].AddCharacter(newChar);

            return newChar;
        }
        /*****************************/
        /* MOCK DATA                 */
        /*****************************/
        public static Data createMockData()
        {
            Data d = new Data();

            // "load" accounts
            d.RegisterAccount("aspect", "asdfg");
            d.RegisterAccount("anderson", "qwert");
            d.RegisterAccount("tester", "tester");

            // "load" characters
            d.CreateCharacter("aspect", "aspect", 1);
            d.CreateCharacter("aspect", "unholyshark", 2);
            d.CreateCharacter("aspect", "earthbound", 3);
            d.CreateCharacter("aspect", "nightbringer", 4);
            d.CreateCharacter("aspect", "heavenstar", 5);
            d.CreateCharacter("tester", "tester", 6);
            d.CreateCharacter("tester", "testertwo", 1);

            return d;
        }
    }
}
