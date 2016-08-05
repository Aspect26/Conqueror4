using System.Collections.Generic;
using Shared;
using System.Drawing;

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
                    return new Quest(new QuestData(
                        new IQuestObjective[]
                        {
                            new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX)
                        },
                        "Call to Arms!",
                        "War! Those demons on the other shore of Crystal River appeared just two days ago." +
                        "They have started pillaging our villages and butchering out people." +
                        "It is already too late for those on the other side of river but we can still at least " +
                        "save those who survived this initial purge. #NAME go and meet Lieutenant Landax up " +
                        "ahead. He will give you next instructions.",
                        SharedData.QUEST_WOLFPACK
                        ));

                case SharedData.QUEST_WOLFPACK:
                    IQuestObjective kills = new UnitKillObjective(SharedData.UNIT_WOLF, 10);
                    IQuestObjective visit = new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX, 
                        new IQuestObjective []{ kills });

                    return new Quest(new QuestData(
                        new IQuestObjective[] { kills, visit },
                        "Wolfpack",
                        "Welcome #NAME to the front. Do you hear that howling? They are not just ordinary wolves." +
                        "It's a pack of spawned beasts by those filthy demons occuping our lands. We need to stop " +
                        "them #NAME. They are trying to break in here. Yout first task is to make them stop.",
                        SharedData.QUEST_SLAY_THEIR_LEADER
                        ));

                case SharedData.QUEST_SLAY_THEIR_LEADER:
                    kills = new UnitKillObjective(SharedData.UNIT_WOLF_PACK_LEADER, 1);
                    visit = new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX,
                        new IQuestObjective[] { kills });

                    return new Quest(new QuestData(
                        new IQuestObjective[] { kills, visit },
                        "Slay their leader",
                        "Good job out there young one. You managed to repel their attack. But they are still " +
                        "still out there waiting for their chance. We need to end this once and for all if we " +
                        "are to sleep safely. You ought to kill their leader. You will recognize him easily - " +
                        "it's the one with big white claws violet eyes and with significant textures on his " +
                        "body.",
                        SharedData.QUEST_KILL_THEM_ALL
                        ));

                case SharedData.QUEST_KILL_THEM_ALL:
                    kills = new UnitKillObjective(SharedData.UNIT_WOLF, 15);
                    visit = new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX,
                        new IQuestObjective[] { kills });

                    return new Quest(new QuestData(
                        new IQuestObjective[] { kills, visit },
                        "Kill them all!",
                        "They went berserk #NAME! They think they can break us by sending so many of their spawns. "+
                        "Go and take care of it. Kill as many as you can. Show them that quantity does not go "+
                        "before quality. Meanwhile I'll think of a way to stop them once and for all.",
                        SharedData.QUEST_PLAN_OF_HOPE
                        ));

                case SharedData.QUEST_PLAN_OF_HOPE:
                    kills = new UnitKillObjective(SharedData.UNIT_WARLOCK_SPAWNER, 5);
                    visit = new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX,
                        new IQuestObjective[] { kills });

                    return new Quest(new QuestData(
                        new IQuestObjective[] { kills, visit },
                        "A plan of hope",
                        "Good news #NAME! Just a moment ago our scouts gave us the most valuable information to "+
                        "our case. These wolves or whatever they are are being spawned by enemy's warlocks which "+
                        "reside just north of here. Travel there and kill them #NAME. Hopefully it will put an "+
                        "end to this madness.",
                        SharedData.QUEST_PYRESTEEL
                        ));

                case SharedData.QUEST_PYRESTEEL:
                    return new Quest(new QuestData(
                        new IQuestObjective[]
                        {
                            new UnitVisitedObjective(SharedData.UNIT_BERLOC_PYRESTEEL)
                        },
                        "Pyresteel",
                        "You have done very well out there young one. You sure deserve a reward for your service "+
                        "to the Realm of Men. Take this token and bring it to Berloc Pyresteel. He is out smith "+
                        "in the front. Just follow this path to the east and it will lead you to him. Oh and "+
                        "please don't forget to send him my regards.",
                        SharedData.QUEST_PYREWOOD
                        ));

                case SharedData.QUEST_PYREWOOD:
                    IQuestObjective region = new RegionVisitedObjective(new Point(52 * 50, 37 * 50), 500);
                    visit = new UnitVisitedObjective(SharedData.UNIT_BERLOC_PYRESTEEL, 
                        new IQuestObjective[] { region });

                    return new Quest(new QuestData(
                        new IQuestObjective[] { region, visit },
                        "Pyrewood",
                        "You carry Lieutenant Landax's token I see. That's a worthy one lad. If you are here "+
                        "already you can len me a helping hand I suppose. Do you see that forest up ahead? "+
                        "I need wood from there to fuel my forges. The problem is that my workers refuse to go "+
                        "there since they saw some foul creatures there as they said. Can you please have "+
                        "a look in there? It's just north from here.",
                        SharedData.QUEST_FOREST_OF_SOULS
                        ));

                case SharedData.QUEST_FOREST_OF_SOULS:
                    kills = new UnitKillObjective(SharedData.UNIT_AWAKENED_SOUL, 15);
                    visit = new UnitVisitedObjective(SharedData.UNIT_BERLOC_PYRESTEEL,
                        new IQuestObjective[] { kills });

                    return new Quest(new QuestData(
                        new IQuestObjective[] { kills, visit },
                        "Forest of souls",
                        "So they were saying the truth. That is an unpleasant one. I cannot supply our troops "+
                        "in the battlefield if someone won't take care of this. Could you please clear the "+
                        "forest from those poor souls for me? I'll be much grteful to you.",
                        SharedData.QUEST_INFORM_THE_LIEUTENANT
                        ));

                case SharedData.QUEST_INFORM_THE_LIEUTENANT:
                    return new Quest(new QuestData(
                        new IQuestObjective[]
                        {
                            new UnitVisitedObjective(SharedData.UNIT_LIEUTENANT_LANDAX)
                        },
                        "Inform the lieutenant",
                        "They are very strange these souls my lad. I've never seen something like that before. "+
                        "It sure is work of that demons. They appeared at the same time. I think it is best "+
                        "that you inform the lieutenant lad.",
                        SharedData.QUEST_NO_QUEST
                        ));
                default:
                    return null;
            }
        }

        // *****************************************
        // GAME DATA
        // *****************************************
        public const int VisitDistance = 100;

        // unit id -> base stats (max hp, ...)
        static Dictionary<int, InitialData> unitInitialStats = new Dictionary<int, InitialData>()
        {
            { SharedData.UNIT_WOLF,
                new InitialData(new BaseStats(100), 1, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_WOLF_PACK_LEADER,
                new InitialData(new BaseStats(250), 2, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_WARLOCK_SPAWNER,
                new InitialData(new BaseStats(150), 2, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_AWAKENED_SOUL,
                new InitialData(new BaseStats(150), 2, SharedData.FRACTION_HOSTILE_UNITS) },

            { SharedData.UNIT_LIEUTENANT_LANDAX,
                new InitialData(new BaseStats(15200), 10, SharedData.FRACTION_HUMAN_REALM) },
            { SharedData.UNIT_BERLOC_PYRESTEEL,
                new InitialData(new BaseStats(5350), 7, SharedData.FRACTION_HUMAN_REALM) }
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
            { DEMON_HUNTER,  new Location(SharedData.MAP_KINGDOM, 86, 3131) },
            { MAGE, new Location(SharedData.MAP_KINGDOM, 86, 3131) },
            { PRIEST, new Location(SharedData.MAP_KINGDOM, 86, 3131) },

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
