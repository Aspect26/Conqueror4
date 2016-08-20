using System.Collections.Generic;
using Shared;
using System.Drawing;
using System;
using System.Linq;

namespace Server
{
    public partial class Data
    {
        // *****************************************
        // ACCOUNTS DATA
        // *****************************************
        //TODO: change this to private when no mock data is needed;
        public Dictionary<string, Account> accountData = new Dictionary<string, Account>();
        public Dictionary<string, Character> characterData = new Dictionary<string, Character>();

        // getters
        public Character GetCharacter(string name)
        {
            Character rtrn;
            if (characterData.TryGetValue(name, out rtrn))
                return rtrn;
            else return null;
        }

        // *****************************************
        // ABILITIES
        // *****************************************
        public static IAbility GetCharacterAbility(Character character)
        {
            return new EmptyAbility(character);
        }

        // *****************************************
        // ITEMS
        // *****************************************
        private static Random random = new Random();

        private const byte ITEM_HITPOINTS = 1;
        private const byte ITEM_MANAPOINTS = 2;
        private const byte ITEM_ARMOR = 3;
        private const byte ITEM_DAMAGE = 4;
        private const byte ITEM_SPELLBONUS = 5;
        private static readonly List<int> itemBonuses = new List<int>(){ ITEM_HITPOINTS, ITEM_MANAPOINTS,
            ITEM_ARMOR, ITEM_DAMAGE, ITEM_DAMAGE };

        private static readonly int ITEM_BONUS_BASE = 20;

        public static IItem GenerateItemDropped(IUnit unit)
        {
            if (unit.IsPlayer())
                return null;

            // NOTE: drop chance -> from db maybe?
            if (!(random.Next(1000) < 1000))
                return null;

            // GENERATE THE ITEM
            // get number of item bonuses
            int itemBonusesCount = random.Next(itemBonuses.Count) + 1;
            return generateItemDropped(unit.Level, itemBonusesCount);
        }

        private static int lastUID = 0;
        private static IItem generateItemDropped(int level, int itemBonusesCount)
        {
            int bonusAmount = (ITEM_BONUS_BASE * level) / itemBonusesCount;
            var actualBonuses = itemBonuses.OrderBy((x) => random.Next()).Take<int>(itemBonusesCount);

            ItemStats itemStats = new ItemStats();
            foreach(byte itemBonus in actualBonuses)
            {
                switch(itemBonus)
                {
                    case ITEM_HITPOINTS:
                        itemStats.HitPoints = bonusAmount; break;
                    case ITEM_MANAPOINTS:
                        itemStats.ManaPoints = bonusAmount; break;
                    case ITEM_ARMOR:
                        itemStats.Armor = bonusAmount / 2; break;
                    case ITEM_DAMAGE:
                        itemStats.Damage = bonusAmount / 2; break;
                    case ITEM_SPELLBONUS:
                        itemStats.Armor = bonusAmount; break;
                    default:
                        throw new NotImplementedException("Not specified generating value of item stat!");
                }
            }

            int slot = random.Next(SharedData.ITEM_SLOTS);
            IItem item = new Item(itemStats, slot, lastUID++);
            return item;
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
                case SharedData.UNIT_SHAMAN:
                    return GetQuest(SharedData.QUEST_NO_QUEST);

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
                    return Quest.NoQuestSingleton;
            }
        }

        // *****************************************
        // GAME DATA
        // *****************************************
        public const int VisitDistance = 100;

        public const int CombatRange = 175;
        public const int EnterCombatDistance = 300;
        public const int LeaveCombatDistance = 650;

        public const int HPRegenInterval = 2000;
        public const int MPRegenInterval = 5000;

        // mapId -> revive location
        static Dictionary<int, Point> reviveLocations = new Dictionary<int, Point>()
        {
            { SharedData.MAP_KINGDOM, new Point(86, 3131) },
            { SharedData.MAP_FORTRESS, new Point() } // TODO: set revive point to fortress
        };

        // unit id -> base stats (max hp, ...)
        static Dictionary<int, InitialData> unitInitialStats = new Dictionary<int, InitialData>()
        {
            { SharedData.UNIT_WOLF,
                new InitialData(new BaseStats(100, 12, 1), 1, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_WOLF_PACK_LEADER,
                new InitialData(new BaseStats(250, 16, 2), 2, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_WARLOCK_SPAWNER,
                new InitialData(new BaseStats(150, 15, 0), 2, SharedData.FRACTION_HOSTILE_UNITS) },
            { SharedData.UNIT_AWAKENED_SOUL,
                new InitialData(new BaseStats(150, 20, 2), 2, SharedData.FRACTION_HOSTILE_UNITS) },

            { SharedData.UNIT_LIEUTENANT_LANDAX,
                new InitialData(new BaseStats(15200, 100, 20), 10, SharedData.FRACTION_HUMAN_REALM) },
            { SharedData.UNIT_BERLOC_PYRESTEEL,
                new InitialData(new BaseStats(5350, 56, 7), 7, SharedData.FRACTION_HUMAN_REALM) }
        };

        static Dictionary<int, CharacterInitialData> characterBaseStats = new Dictionary<int, CharacterInitialData>()
        {
            { SharedData.UNIT_DEMONHUNTER,new CharacterInitialData(SharedData.FRACTION_HUMAN_REALM, new BaseStats[3]
                { new BaseStats(120, 75, 18, 3), new BaseStats(140, 85, 20, 4), new BaseStats(165, 100, 24, 6) } ) },

            { SharedData.UNIT_MAGE, new CharacterInitialData(SharedData.FRACTION_HUMAN_REALM, new BaseStats[3]
                { new BaseStats(90, 90, 23, 1), new BaseStats(100, 100, 28, 1), new BaseStats(115, 115, 34, 3) } ) },

            { SharedData.UNIT_PRIEST, new CharacterInitialData(SharedData.FRACTION_HUMAN_REALM, new BaseStats[3]
                { new BaseStats(80, 95, 15, 0), new BaseStats(90, 115, 17, 1), new BaseStats(105, 135, 19, 1) } ) },

            { SharedData.UNIT_WARLOCK, new CharacterInitialData(SharedData.FRACTION_DEMON_KINGDOM, new BaseStats[3]
                { new BaseStats(70, 95, 25, 0), new BaseStats(80, 105, 31, 0), new BaseStats(90, 120, 39, 1) } ) },

            { SharedData.UNIT_UNKHERO1, new CharacterInitialData(SharedData.FRACTION_DEMON_KINGDOM, new BaseStats[3]
                { new BaseStats(110, 65, 15, 4), new BaseStats(135, 70, 18, 5), new BaseStats(155, 80, 21, 7) } ) },

            { SharedData.UNIT_SHAMAN, new CharacterInitialData(SharedData.FRACTION_DEMON_KINGDOM, new BaseStats[3]
                { new BaseStats(85, 90, 15, 1), new BaseStats(100, 105, 17, 2), new BaseStats(115, 130, 19, 3) } ) },
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
        public static Point GetReviveLocation(int mapId)
        {
            return reviveLocations[mapId];
        }

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

            // add items to aspect character
            Equip eq = new Equip();
            eq.Items[SharedData.ITEM_SLOT_PANTS] = new Item(new ItemStats() { HitPoints=15, Armor=10 }, SharedData.ITEM_SLOT_PANTS, 12135);
            eq.Items[SharedData.ITEM_SLOT_WEAPON] = new Item(new ItemStats() { HitPoints = 15, Damage = 10 }, SharedData.ITEM_SLOT_WEAPON, 978354);
            d.addEquipToCharacter("aspect", eq);

            return d;
        }

        // mock functions
        private void addEquipToCharacter(string character, Equip eq)
        {
            this.characterData[character].SetEquip(eq);
        }
    }
}
