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

        // unit id -> base stats (max hp, ...)
        static Dictionary<int, BaseStats> unitInitialStats = new Dictionary<int, BaseStats>()
        {
            { UNIT_WOLF, new BaseStats(100) },
            { UNIT_WOLF_PACK_LEADER, new BaseStats(250) }
        };

        // map id -> map name
        static Dictionary<int, string> mapNames = new Dictionary<int, string>()
        {
            { MAP_KINGDOM, "Kingdom of ___" },
            { MAP_FORTRESS, "The DarkFortress" }
        };

        // spec id -> spec starting location
        static Dictionary<int, Location> startingLocations = new Dictionary<int, Location>()
        {
            { DEMON_HUNTER,  new Location(MAP_KINGDOM, 286, 2929) },
            { MAGE, new Location(MAP_KINGDOM,286,2929) },
            { PRIEST, new Location(MAP_KINGDOM,286,2929) },

            { WARLOCK, new Location(MAP_FORTRESS,100,100) },
            { UNK_1, new Location(MAP_FORTRESS,20,20) },
            { UNK_2, new Location(MAP_FORTRESS,20,20) },
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
        /* IDs                       */
        /*****************************/
        // maps
        public const int MAP_KINGDOM = 0;
        public const int MAP_FORTRESS = 1;

        // units
        public const int UNIT_WOLF = 7;
        public const int UNIT_WOLF_PACK_LEADER = 8;

        /*****************************/
        /* MOCK DATA                 */
        /*****************************/
        public static Data createMockData(Game game)
        {
            Data d = new Data(game);

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
            d.CreateCharacter("aspect", "magmaspike", 6);
            d.CreateCharacter("tester", "tester", 6);
            d.CreateCharacter("tester", "testertwo", 1);
            d.CreateCharacter("warlock", "warlock", 4);

            return d;
        }
    }
}
