using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Server
{
    public class Data
    {
        private static MySQLConnection sqlConnection;

        // data
        private static Dictionary<string, Account> accountData = new Dictionary<string, Account>();
        private static Dictionary<string, Character> characterData = new Dictionary<string, Character>();
        private static Dictionary<string, int> integerConstants = new Dictionary<string, int>();
        private static Dictionary<int, InitialData> unitsData = new Dictionary<int, InitialData>();
        private static Dictionary<int, InitialData[]> characterBaseStats = new Dictionary<int, InitialData[]>();
        private static Dictionary<int, MapData> mapData = new Dictionary<int, MapData>();
        private static Dictionary<int, int> initialQuestIds = new Dictionary<int, int>();
        private static Dictionary<int, IQuest> questsData = new Dictionary<int, IQuest>();
        private static Dictionary<int, MapInstance> mapInstances = new Dictionary<int, MapInstance>();
        private static Dictionary<int, Location> startingLocations = new Dictionary<int, Location>();

        private static Random random;

        static Data()
        {
            sqlConnection = new MySQLConnection();
            random = new Random();
        }

        public static bool Initialize()
        {
            if (!sqlConnection.Initialize())
                return false;

            sqlConnection.LoadConstants(out integerConstants);
            sqlConnection.LoadInitialQuests(out initialQuestIds);
            sqlConnection.LoadQuests(out questsData);
            sqlConnection.LoadUnitsData(out unitsData);
            sqlConnection.LoadCharactersData(out characterBaseStats);
            sqlConnection.LoadAccounts(out accountData);
            sqlConnection.LoadCharacters(out characterData, accountData);
            sqlConnection.LoadMaps(out mapData, out mapInstances);
            sqlConnection.LoadStartingLocations(out startingLocations);

            return true;
        }

        // *****************************************
        // MISCELANEOUS
        // *****************************************
        public static int GetXPReward(int level)
        {
            return (int)(level * 2.5f + 7.5f);
        }

        // *****************************************
        // MAPS
        // *****************************************
        public static Dictionary<int, MapData> GetMaps()
        {
            return mapData;
        }

        public static Point GetReviveLocation(int mapId)
        {
            return mapData[mapId].ReviveLocation;
        }

        public static Dictionary<int, MapInstance> GetMapInstances()
        {
            return mapInstances;
        }

        // *****************************************
        // ACCOUNTS & CHARACTERS
        // *****************************************
        public static Character GetCharacter(string name)
        {
            Character rtrn;
            if (characterData.TryGetValue(name, out rtrn))
                return rtrn;
            else return null;
        }

        public static Account LoginAccount(string username, string password)
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

        public static bool RegisterAccount(string username, string password)
        {
            username = username.ToLower();
            password = password.ToLower();

            if (accountData.ContainsKey(username))
                return false;

            accountData.Add(username, new Account(username, password));
            return true;
        }

        public static Character CreateCharacter(string username, string name, int spec)
        {
            name = name.ToLower();
            if (characterData.ContainsKey(name))
                return null;

            if (!accountData.ContainsKey(username))
                return null;

            Location startingLocation = startingLocations[spec];
            Character newChar = new Character(name, spec, -1, startingLocation, null);
            characterData.Add(name, newChar);
            accountData[username].AddCharacter(newChar);

            return newChar;
        }

        // *****************************************
        // CONSTANTS FROM DATABASE
        // *****************************************
        private static int ITEM_BONUS_BASE { get { return integerConstants["item_bonus_base"]; } }
        public static int CombatRange { get { return integerConstants["combat_range"]; } }
        public static int EnterCombatDistance { get { return integerConstants["enter_combat_distance"]; } }
        public static int LeaveCombatDistance { get { return integerConstants["leave_combat_distance"]; } }
        public static int VisitDistance { get { return integerConstants["visit_distance"]; } }
        public static int MaxLevel { get { return integerConstants["max_level"]; } }
        public static int HPRegenInterval { get { return integerConstants["hp_regen_interval"]; } }
        public static int MPRegenInterval { get { return integerConstants["mp_regen_interval"]; } }

        // *****************************************
        // CONSTANTS 
        // *****************************************
        private const byte ITEM_HITPOINTS = 1;
        private const byte ITEM_MANAPOINTS = 2;
        private const byte ITEM_ARMOR = 3;
        private const byte ITEM_DAMAGE = 4;
        private const byte ITEM_SPELLBONUS = 5;
        private static readonly List<int> itemBonuses = new List<int>(){ ITEM_HITPOINTS, ITEM_MANAPOINTS,
            ITEM_ARMOR, ITEM_DAMAGE, ITEM_DAMAGE };

        // *****************************************
        // BASE STATS
        // *****************************************
        public static InitialData GetInitialData(int unitId)
        {
            return unitsData[unitId];
        }

        public static InitialData GetCharacterBaseStats(int id, int level)
        {
            return characterBaseStats[id][level];
        }

        // *****************************************
        // QUESTS
        // *****************************************
        public static IQuest GetInitialQuest(int spec)
        {
            switch (spec)
            {
                case SharedData.UNIT_DEMONHUNTER:
                case SharedData.UNIT_MAGE:
                case SharedData.UNIT_PRIEST:
                    return GetQuest(initialQuestIds[spec]);

                case SharedData.UNIT_WARLOCK:
                case SharedData.UNIT_UNKHERO1:
                case SharedData.UNIT_SHAMAN:
                    return GetQuest(initialQuestIds[spec]);

                default:
                    return null;
            }
        }

        public static IQuest GetQuest(int id)
        {
            return questsData[id].Copy();
        }

        // *****************************************
        // ABILITIES
        // *****************************************
        public static IAbility GetCharacterAbility(Character character)
        {
            switch (character.Spec)
            {
                case SharedData.UNIT_PRIEST:
                    return CreateAbility(character, SharedData.ABILITY_PRIEST_HEAL);
                default:
                    return new EmptyAbility(character);
            }
        }

        public static IAbility CreateAbility(IUnit source, int id)
        {
            switch (id)
            {
                case SharedData.ABILITY_PRIEST_HEAL:
                    return new PriestHealAbility(source);
                default:
                    return new EmptyAbility(source);
            }
        }
        // *****************************************
        // ITEMS
        // *****************************************
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
            foreach (byte itemBonus in actualBonuses)
            {
                switch (itemBonus)
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
    }
}
