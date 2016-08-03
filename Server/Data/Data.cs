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
        // QUESTS DATA
        // *****************************************
        public static IQuest GetInitialQuest(int spec)
        {
            switch (spec)
            {
                case SharedData.UNIT_DEMONHUNTER:
                case SharedData.UNIT_MAGE:
                case SharedData.UNIT_PRIEST:
                    return GetQuest(SharedData.QUEST_CALL_TO_ARMS);

                case SharedData.UNIT_WARLOCK:
                case SharedData.UNIT_UNKHERO1:
                case SharedData.UNIT_UNKHERO2:
                    return null;

                default:
                    return null;
            }
        }

        public static IQuest GetQuest(int id)
        {
            switch (id)
            {
                case SharedData.QUEST_CALL_TO_ARMS:
                    return new Quest(new IQuestObjective[] 
                    {
                        new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX)
                    }, GetQuestDescription(SharedData.QUEST_CALL_TO_ARMS));

                case SharedData.QUEST_WOLFPACK:
                    return new Quest(new IQuestObjective[]
                    {
                        new UnitKillObjective(SharedData.UNIT_WOLF, 10),
                        new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX)
                    }, GetQuestDescription(SharedData.QUEST_WOLFPACK));

                case SharedData.QUEST_SLAY_THEIR_LEADER:
                    return new Quest(new IQuestObjective[]
                    {
                        new UnitKillObjective(SharedData.UNIT_WOLF_PACK_LEADER, 1),
                        new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX)
                    }, GetQuestDescription(SharedData.QUEST_SLAY_THEIR_LEADER));
                default:
                    return null;
            }
        }

        public static string GetQuestDescription(int id)
        {
            switch (id)
            {
                default:
                    return "No text!";
            }
        }

        // *****************************************
        // GAME DATA
        // *****************************************

        // unit id -> base stats (max hp, ...)
        static Dictionary<int, InitialData> unitInitialStats = new Dictionary<int, InitialData>()
        {
            { SharedData.UNIT_WOLF,
                new InitialData(new BaseStats(100), 1, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_WOLF_PACK_LEADER,
                new InitialData(new BaseStats(250), 2, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_LIEUTENANT_LANDAX,
                new InitialData(new BaseStats(15200), 10, SharedData.FRACTION_HUMAN_REALM) }
        };

        static Dictionary<int, CharacterInitialData> characterBaseStats = new Dictionary<int, CharacterInitialData>()
        {
            { SharedData.UNIT_DEMONHUNTER,new CharacterInitialData(SharedData.FRACTION_HUMAN_REALM, new BaseStats[3]
                { new BaseStats(120), new BaseStats(140), new BaseStats(165) } ) },

            { SharedData.UNIT_MAGE, new CharacterInitialData(SharedData.FRACTION_HUMAN_REALM, new BaseStats[3]
                { new BaseStats(90), new BaseStats(100), new BaseStats(115) } ) },

            { SharedData.UNIT_PRIEST, new CharacterInitialData(SharedData.FRACTION_HUMAN_REALM, new BaseStats[3]
                { new BaseStats(80), new BaseStats(90), new BaseStats(105) } ) },

            { SharedData.UNIT_WARLOCK, new CharacterInitialData(SharedData.FRACTION_DEMON_KINGDOM, new BaseStats[3]
                { new BaseStats(70), new BaseStats(80), new BaseStats(90) } ) },

            { SharedData.UNIT_UNKHERO1, new CharacterInitialData(SharedData.FRACTION_DEMON_KINGDOM, new BaseStats[3]
                { new BaseStats(110), new BaseStats(135), new BaseStats(155) } ) },

            { SharedData.UNIT_UNKHERO2, new CharacterInitialData(SharedData.FRACTION_DEMON_KINGDOM, new BaseStats[3]
                { new BaseStats(85), new BaseStats(100), new BaseStats(115) } ) },
        };

        // map id -> map name
        static Dictionary<int, string> mapNames = new Dictionary<int, string>()
        {
            { SharedData.MAP_KINGDOM, "Kingdom of ___" },
            { SharedData.MAP_FORTRESS, "The DarkFortress" }
        };

        // spec id -> spec starting location
        static Dictionary<int, Location> startingLocations = new Dictionary<int, Location>()
        {
            { DEMON_HUNTER,  new Location(SharedData.MAP_KINGDOM, 286, 2929) },
            { MAGE, new Location(SharedData.MAP_KINGDOM,286,2929) },
            { PRIEST, new Location(SharedData.MAP_KINGDOM,286,2929) },

            { WARLOCK, new Location(SharedData.MAP_FORTRESS,100,100) },
            { UNK_1, new Location(SharedData.MAP_FORTRESS,20,20) },
            { UNK_2, new Location(SharedData.MAP_FORTRESS,20,20) },
        };

        // xp rewards
        public static int GetXPReward(int level)
        {
            return (int)(level * 2.5f + 7.5f);
        }

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

        public static InitialData GetInitialData(int unitId)
        {
            return unitInitialStats[unitId];
        }

        public static InitialData GetCharacterBaseStats(int id, int level)
        {
            InitialData data = new InitialData(characterBaseStats[id].GetData(level), level, characterBaseStats[id].Fraction);
            return data;
        }

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
