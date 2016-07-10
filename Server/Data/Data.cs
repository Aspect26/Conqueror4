using System.Collections.Generic;
using Shared;

namespace Server
{
    public partial class Data
    {
        // *****************************************
        // ACCOUNTS DATA
        // *****************************************
        static Dictionary<string, Account> accountData = new Dictionary<string, Account>();
        static Dictionary<string, Character> characterData = new Dictionary<string, Character>();

        // getters
        public static Character GetCharacter(string name)
        {
            Character rtrn;
            if (characterData.TryGetValue(name, out rtrn))
                return rtrn;
            else return null;
        }

        // *****************************************
        // GAME DATA
        // *****************************************

        // map id -> map name
        static Dictionary<int, string> mapNames = new Dictionary<int, string>()
        {
            { 0, "Kingdom of ___" },
            { 1, "The DarkFortress" }
        };

        // spec id -> spec starting location
        static Dictionary<int, Location> startingLocations = new Dictionary<int, Location>()
        {
            { DEMON_HUNTER,  new Location(0, 500, 500) },
            { MAGE, new Location(0,20,20) },
            { PRIEST, new Location(0,20,100) },

            { WARLOCK, new Location(1,100,100) },
            { UNK_1, new Location(1,20,20) },
            { UNK_2, new Location(1,20,20) },
        };

        // getters
        public static Location GetStartingLocation(int spec)
        {
            Location l = startingLocations[spec];
            return new Location(l.MapID, l.X, l.Y);
        }

        public static Dictionary<int, string> GetMaps()
        {
            return mapNames;
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
            d.RegisterAccount("warlock", "warlock");

            // "load" characters
            d.CreateCharacter("aspect", "aspect", 1);
            d.CreateCharacter("aspect", "unholyshark", 2);
            d.CreateCharacter("aspect", "earthbound", 3);
            d.CreateCharacter("aspect", "nightbringer", 4);
            d.CreateCharacter("aspect", "heavenstar", 5);
            d.CreateCharacter("tester", "tester", 6);
            d.CreateCharacter("tester", "testertwo", 1);
            d.CreateCharacter("warlock", "warlock", 4);

            return d;
        }
    }
}
