using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Server
{
    /// <summary>
    /// Represents the game data loaded from a MySQL database. 
    /// This class is basicly just a bunch of getters (I give you ID and you will return
    /// the right image or string. The documentation here is mostly only generted without
    /// much further editation.
    /// It contains a buch of hashmaps (dictionaries) that take integer identifier
    /// or a string and return needed value. This data are loaded from an SQL server through
    /// MySQLConnection instance.
    /// </summary>
    /// <seealso cref="MySQLConnection"/>
    public class Data
    {
        private static MySQLConnection sqlConnection;

        /// <summary>
        /// The lock for the sql database is needed becaus it is accessed from the
        /// game thread (for data) and from the sending thread (to save the data).
        /// </summary>
        public static object SQLLock = new object();

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
        private static List<int> levelXpRequirements = new List<int>();

        private static Random random;

        static Data()
        {
            sqlConnection = new MySQLConnection();
            random = new Random();
        }

        /// <summary>
        /// Loads the data from the database.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        public static bool Initialize()
        {
            lock (SQLLock)
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
                sqlConnection.LoadLevelXpRequirements(out levelXpRequirements);
            }

            return true;
        }

        // *****************************************
        // MISCELANEOUS
        // *****************************************
        /// <summary>
        /// Gets the xp reward.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>System.Int32.</returns>
        public static int GetXPReward(int level)
        {
            return (int)(level * 2.5f + 7.5f);
        }

        /// <summary>
        /// Gets the next level xp required.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>System.Int32.</returns>
        public static int GetNextLevelXPRequired(int level)
        {
            return levelXpRequirements[--level];
        }

        // *****************************************
        // MAPS
        // *****************************************
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <returns>Dictionary&lt;System.Int32, MapData&gt;.</returns>
        public static Dictionary<int, MapData> GetMaps()
        {
            return mapData;
        }

        /// <summary>
        /// Gets the revive location.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>Point.</returns>
        public static Point GetReviveLocation(int mapId)
        {
            return mapData[mapId].ReviveLocation;
        }

        /// <summary>
        /// Gets the map instances.
        /// </summary>
        /// <returns>Dictionary&lt;System.Int32, MapInstance&gt;.</returns>
        public static Dictionary<int, MapInstance> GetMapInstances()
        {
            return mapInstances;
        }

        // *****************************************
        // ACCOUNTS & CHARACTERS
        // *****************************************
        /// <summary>
        /// Gets the character.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Character.</returns>
        public static Character GetCharacter(string name)
        {
            Character rtrn;
            if (characterData.TryGetValue(name, out rtrn))
                return rtrn;
            else return null;
        }

        /// <summary>
        /// Logins the account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Account.</returns>
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

        /// <summary>
        /// Registers the account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool RegisterAccount(string username, string password)
        {
            username = username.ToLower();
            password = password.ToLower();

            if (accountData.ContainsKey(username))
                return false;

            accountData.Add(username, new Account(username, password));
            lock(SQLLock)
                sqlConnection.SaveAccount(accountData[username]);
            return true;
        }

        /// <summary>
        /// Creates a character.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <param name="spec">The spec.</param>
        /// <returns>Character.</returns>
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

            lock(SQLLock)
                sqlConnection.SaveCharacter(newChar, true, username);

            return newChar;
        }

        // *****************************************
        // CONSTANTS FROM DATABASE
        // *****************************************
        private static int ITEM_BONUS_BASE { get { return integerConstants["item_bonus_base"]; } }
        /// <summary>
        /// Gets the item weapon slot.
        /// </summary>
        /// <value>The item weapon slot.</value>
        public static int ItemWeaponSlot { get { return integerConstants["equip_weapon_slot"]; } }
        /// <summary>
        /// Gets the item chest slot.
        /// </summary>
        /// <value>The item chest slot.</value>
        public static int ItemChestSlot { get { return integerConstants["equip_chest_slot"]; } }
        /// <summary>
        /// Gets the item head slot.
        /// </summary>
        /// <value>The item head slot.</value>
        public static int ItemHeadSlot { get { return integerConstants["equip_head_slot"]; } }
        /// <summary>
        /// Gets the item pants slot.
        /// </summary>
        /// <value>The item pants slot.</value>
        public static int ItemPantsSlot { get { return integerConstants["equip_pants_slot"]; } }
        /// <summary>
        /// Gets the combat range.
        /// </summary>
        /// <value>The combat range.</value>
        public static int CombatRange { get { return integerConstants["combat_range"]; } }
        /// <summary>
        /// Gets the enter combat distance.
        /// </summary>
        /// <value>The enter combat distance.</value>
        public static int EnterCombatDistance { get { return integerConstants["enter_combat_distance"]; } }
        /// <summary>
        /// Gets the leave combat distance.
        /// </summary>
        /// <value>The leave combat distance.</value>
        public static int LeaveCombatDistance { get { return integerConstants["leave_combat_distance"]; } }
        /// <summary>
        /// Gets the visit distance.
        /// </summary>
        /// <value>The visit distance.</value>
        public static int VisitDistance { get { return integerConstants["visit_distance"]; } }
        /// <summary>
        /// Gets the maximum level.
        /// </summary>
        /// <value>The maximum level.</value>
        public static int MaxLevel { get { return integerConstants["max_level"]; } }
        /// <summary>
        /// Gets the hp regen interval.
        /// </summary>
        /// <value>The hp regen interval.</value>
        public static int HPRegenInterval { get { return integerConstants["hp_regen_interval"]; } }
        /// <summary>
        /// Gets the mp regen interval.
        /// </summary>
        /// <value>The mp regen interval.</value>
        public static int MPRegenInterval { get { return integerConstants["mp_regen_interval"]; } }
        /// <summary>
        /// Gets the SQL save interval.
        /// </summary>
        /// <value>The SQL save interval.</value>
        public static int SQLSaveInterval { get { return integerConstants["sql_save_interval"]; } }
        
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
        /// <summary>
        /// Gets the initial data.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns>InitialData.</returns>
        public static InitialData GetInitialData(int unitId)
        {
            return unitsData[unitId];
        }

        /// <summary>
        /// Gets the character base stats.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="level">The level.</param>
        /// <returns>InitialData.</returns>
        public static InitialData GetCharacterBaseStats(int id, int level)
        {
            return characterBaseStats[id][level];
        }

        // *****************************************
        // QUESTS
        // *****************************************
        /// <summary>
        /// Gets the initial quest.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns>IQuest.</returns>
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

        /// <summary>
        /// Gets the quest.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IQuest.</returns>
        public static IQuest GetQuest(int id)
        {
            return questsData[id].Copy();
        }

        // *****************************************
        // ABILITIES
        // *****************************************
        /// <summary>
        /// Gets the character ability.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <returns>IAbility.</returns>
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

        /// <summary>
        /// Creates the ability.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>IAbility.</returns>
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
        /// <summary>
        /// Generates the item dropped.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>IItem.</returns>
        public static IItem GenerateItemDropped(IUnit unit)
        {
            if (unit.IsPlayer())
                return null;

            // NOTE: drop chance -> from db maybe?
            if (!(random.Next(1000) < 170))
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
                        itemStats.SpellBonus = bonusAmount; break;
                    default:
                        throw new NotImplementedException("Not specified generating value of item stat!");
                }
            }

            int slot = random.Next(SharedData.ITEM_SLOTS);
            IItem item = new Item(itemStats, slot, lastUID++);
            return item;
        }

        // ***************************************************
        // SAVE
        // ***************************************************
        /// <summary>
        /// Saves the data to the SQL database. This function is called every X
        /// minutes.
        /// </summary>
        public static void Save()
        {
            Console.WriteLine("SAVING DATA TO DB...");
            lock (SQLLock)
            {
                foreach(var pair in characterData)
                {
                    string name = pair.Key;
                    Character c = pair.Value;
                    if (c.SQLDifference)
                    {
                        sqlConnection.SaveCharacter(c, false);
                        c.SQLDifference = false;
                    }
                }
            }
        }

        /// <summary>
        /// Saves a character to the SQL database.
        /// </summary>
        /// <param name="c">The c.</param>
        public static void SaveCharacter(Character c)
        {
            lock (SQLLock)
            {
                sqlConnection.SaveCharacter(c, false);
            }
        }
    }
}
