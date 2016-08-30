using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using Shared;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// This class represents a connection to a MySQL server. Currently it points to
    /// localhost because I'm too poor to afford real server :(. 
    /// It also contains functions to get game data from the database. 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class MySQLConnection : IDisposable
    {
        private const string SERVER = "localhost";
        private const string DB = "conqueror4";
        private const string UID = "root";
        private const string PWD = "root";

        private const string CONN_STRING = "SERVER=" + SERVER + ";DATABASE=" + DB + 
            ";UID=" + UID + ";PASSWORD=" + PWD + ";";

        private MySqlConnection connection;

        /// <summary>
        /// Initializes the MySql connection to the server.
        /// </summary>
        /// <returns><c>true</c> if the opertion was successful, <c>false</c> otherwise.</returns>
        public bool Initialize()
        {
            Console.WriteLine("Connecting to MySQL server...");

            connection = new MySqlConnection(CONN_STRING);
            try
            {
                connection.Open();
            }
            catch(MySqlException e)
            {
                Console.WriteLine("Error opening connection: " + e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the accounts.
        /// </summary>
        /// <param name="accountData">The account data.</param>
        public void LoadAccounts(out Dictionary<string, Account> accountData)
        {
            Console.WriteLine("Loading accounts...");
            accountData = new Dictionary<string, Account>();
            var results = Select(new string[] { "name", "password" }, "accounts");

            foreach(var row in results)
            {
                string name = row["name"];
                string pass = row["password"];
                var account = new Account(name, pass);
                accountData.Add(name, account);
            }
        }

        /// <summary>
        /// Loads the constants.
        /// </summary>
        /// <param name="constants">The constants.</param>
        public void LoadConstants(out Dictionary<string, int> constants)
        {
            Console.WriteLine("Loading game constants...");
            constants = new Dictionary<string, int>();
            var results = Select(new string[] { "name", "value" }, "integer_constants");

            foreach (var row in results)
            {
                string name = row["name"];
                int value = Convert.ToInt32(row["value"]);
                constants.Add(name, value);
            }
        }

        /// <summary>
        /// Loads the characters.
        /// </summary>
        /// <param name="charactersData">The characters data.</param>
        /// <param name="accounts">The accounts.</param>
        public void LoadCharacters(out Dictionary<string, Character> charactersData,
            Dictionary<string, Account> accounts)
        {
            Console.WriteLine("Loading characters...");
            charactersData = new Dictionary<string, Character>();
            var results = Select(new string[] { "account", "name", "spec", "xloc", "yloc", "mapid",
                "hitpoints", "manapoints", "experience", "level", "questid", "equip_weapon",
                "equip_chest", "equip_head", "equip_pants" }, "characters");

            foreach(var row in results)
            {
                string account = row["account"];
                string name = row["name"];
                int spec = Convert.ToInt32(row["spec"]);
                int xLoc = Convert.ToInt32(row["xloc"]);
                int yLoc = Convert.ToInt32(row["yloc"]);
                int mapId = Convert.ToInt32(row["mapid"]);

                int actualHp = Convert.ToInt32(row["hitpoints"]);
                int actualMp = Convert.ToInt32(row["manapoints"]);
                int xp = Convert.ToInt32(row["experience"]);
                int level = Convert.ToInt32(row["level"]);
                IQuest quest = Data.GetQuest(Convert.ToInt32(row["questid"]));
                Equip equip = new Equip();
                equip.SetItem(parseItem(row["equip_weapon"], Data.ItemWeaponSlot), Data.ItemWeaponSlot);
                equip.SetItem(parseItem(row["equip_chest"], Data.ItemChestSlot), Data.ItemChestSlot);
                equip.SetItem(parseItem(row["equip_head"], Data.ItemHeadSlot), Data.ItemHeadSlot);
                equip.SetItem(parseItem(row["equip_pants"], Data.ItemPantsSlot), Data.ItemPantsSlot);

                // TODO: wtf is this uid?
                Character character = new Character(name, spec, -1, new Location(mapId, xLoc, yLoc), null);
                character.ActualStats.HitPoints = actualHp;
                character.ActualStats.ManaPoints = actualMp;
                character.SetExperience(xp);
                character.SetLevel(level);
                character.SetQuest(quest);
                character.SetEquip(equip);

                charactersData.Add(name, character);
                accounts[account].AddCharacter(character);
            }
        }

        private IItem parseItem(string data, int slot)
        {
            if (data == "")
                return null;

            try
            {
                string[] parts = data.Split(',');
                int uid = Convert.ToInt32(parts[0]);
                int hp = 0, mp = 0, armor = 0, damage = 0, sb = 0;

                for (int i = 1; i < parts.Length; i++)
                {
                    string[] itemBonus = parts[i].Split(':');
                    int value = Convert.ToInt32(itemBonus[1]);
                    switch (itemBonus[0])
                    {
                        case "H":
                            hp = value; break;
                        case "M":
                            mp = value; break;
                        case "A":
                            armor = value; break;
                        case "D":
                            damage = value; break;
                        case "S":
                            sb = value; break;
                        default:
                            // TODO: throw some normal exception and catch it
                            throw new NotImplementedException("Wrong item bonus in sql database.");
                    }
                }

                Item item = new Item(new ItemStats(hp, mp, armor, damage, sb), slot, uid);
                return item;
            }
            catch (Exception e) when (e is FormatException || e is IndexOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Loads the units data.
        /// </summary>
        /// <param name="unitsData">The units data.</param>
        public void LoadUnitsData(out Dictionary<int, InitialData> unitsData)
        {
            Console.WriteLine("Loading units data...");
            unitsData = new Dictionary<int, InitialData>();
            var results = Select(new string[]  { "id", "name", "hitpoints", "manapoints",
                "damage", "armor", "spellbonus", "level", "fraction" }, "unit_data");

            foreach (var row in results)
            {
                int id = Convert.ToInt32(row["id"]);
                string name = row["name"];
                int hp = Convert.ToInt32(row["hitpoints"]);
                int mp = Convert.ToInt32(row["manapoints"]);
                int dmg = Convert.ToInt32(row["damage"]);
                int armor = Convert.ToInt32(row["armor"]);
                int sp = Convert.ToInt32(row["spellbonus"]);
                int level = Convert.ToInt32(row["level"]);
                int fraction = Convert.ToInt32(row["fraction"]);

                unitsData.Add(id, new InitialData(new BaseStats(hp, mp, dmg, armor, sp), level, fraction));
            }
        }

        /// <summary>
        /// Loads the characters data.
        /// </summary>
        /// <param name="charactersBaseData">The characters base data.</param>
        public void LoadCharactersData(out Dictionary<int, InitialData[]> charactersBaseData)
        {
            Console.WriteLine("Loading characters data...");
            charactersBaseData = new Dictionary<int, InitialData[]>();
            var results = Select(new string[]  { "spec", "name", "hitpoints", "manapoints",
                "damage", "armor", "spellbonus", "level", "fraction" }, "characters_data");

            foreach (var row in results)
            {
                int spec = Convert.ToInt32(row["spec"]);
                string name = row["name"];
                int hp = Convert.ToInt32(row["hitpoints"]);
                int mp = Convert.ToInt32(row["manapoints"]);
                int damage = Convert.ToInt32(row["damage"]);
                int armor = Convert.ToInt32(row["armor"]);
                int sb = Convert.ToInt32(row["spellbonus"]);
                int level = Convert.ToInt32(row["level"]);
                int fraction = Convert.ToInt32(row["fraction"]);

                if (!charactersBaseData.ContainsKey(spec))
                    charactersBaseData.Add(spec, new InitialData[Data.MaxLevel]);

                charactersBaseData[spec][level] = new InitialData(new BaseStats(hp, mp, damage, armor,
                    sb), level, fraction);
            }
        }

        /// <summary>
        /// Loads the maps.
        /// </summary>
        /// <param name="mapData">The map data.</param>
        /// <param name="mapInstances">The map instances.</param>
        public void LoadMaps(out Dictionary<int, MapData> mapData, out Dictionary<int, MapInstance> mapInstances)
        {
            Console.WriteLine("Loading maps data...");
            mapData = new Dictionary<int, MapData>();
            var results = Select(new string[] { "id", "name", "revive_loc_x", "revive_loc_y",
                "map_unit_data_table"}, "map_data");

            // map data
            foreach(var row in results)
            {
                int id = Convert.ToInt32(row["id"]);
                int reviveLocX = Convert.ToInt32(row["revive_loc_x"]);
                int reviveLocY = Convert.ToInt32(row["revive_loc_y"]);
                string name = row["name"];
                string dataTableName = row["map_unit_data_table"];
                mapData.Add(id, new MapData { Name = name, DataTableName = dataTableName,
                    ReviveLocation = new Point(reviveLocX, reviveLocY) });
            }

            // map instances
            mapInstances = new Dictionary<int, MapInstance>();
            foreach(KeyValuePair<int, MapData> map in mapData)
            {
                int id = map.Key;
                MapInstance instance = LoadMapInstance(map.Value.DataTableName, id);
                mapInstances.Add(id, instance);
            }
        }

        private MapInstance LoadMapInstance(string tableName, int id)
        {
            // load units infos
            List<UnitInfo> units = new List<UnitInfo>();
            List<ObjectInfo> objects = new List<ObjectInfo>();
            var results = Select(new string[] { "id", "xloc", "yloc", "special_arguments"}, tableName);
            foreach(var row in results)
            {
                string objectId = row["id"];
                int xLoc = Convert.ToInt32(row["xloc"]);
                int yLoc = Convert.ToInt32(row["yloc"]);

                int unitId;
                var isUnit = int.TryParse(objectId, out unitId);
                if (isUnit)
                {
                    units.Add(new UnitInfo(unitId, xLoc, yLoc));
                }
                else
                {
                    string[] specialArguments = row["special_arguments"].Split(',');
                    objects.Add(new ObjectInfo(objectId[0], xLoc, yLoc, specialArguments));
                }
            }

            // create map instance
            MapInstance instance = new MapInstance(id);
            foreach(UnitInfo unit in units)
            {
                instance.SpawnNPC(unit.ID, unit.X, unit.Y);
            }
            foreach (ObjectInfo objInfo in objects)
            {
                instance.SpawnObject(objInfo);
            }

            return instance;
        }

        /// <summary>
        /// Loads the starting locations.
        /// </summary>
        /// <param name="startingLocations">The starting locations.</param>
        public void LoadStartingLocations(out Dictionary<int, Location> startingLocations)
        {
            Console.WriteLine("Loading starting locations...");
            startingLocations = new Dictionary<int, Location>();
            var results = Select(new string[] { "spec", "loc_x", "loc_y", "mapid" }, 
                "character_starting_locations");

            foreach(var row in results)
            {
                int spec = Convert.ToInt32(row["spec"]);
                int xLoc = Convert.ToInt32(row["loc_x"]);
                int yLoc = Convert.ToInt32(row["loc_y"]);
                int mapId = Convert.ToInt32(row["mapid"]);
                startingLocations.Add(spec, new Location(mapId, xLoc, yLoc));
            }
        }

        /// <summary>
        /// Loads the initial quests.
        /// </summary>
        /// <param name="initialQuests">The initial quests.</param>
        public void LoadInitialQuests(out Dictionary<int, int> initialQuests)
        {
            Console.WriteLine("Loading initial quests...");
            initialQuests = new Dictionary<int, int>();
            var results = Select(new string[] { "spec", "quest_id" }, "character_initial_quests");

            foreach (var row in results)
            {
                int spec = Convert.ToInt32(row["spec"]);
                int qid = Convert.ToInt32(row["quest_id"]);
                initialQuests.Add(spec, qid);
            }
        }

        /// <summary>
        /// Loads the quests.
        /// </summary>
        /// <param name="questsData">The quests data.</param>
        public void LoadQuests(out Dictionary<int, IQuest> questsData)
        {
            Console.WriteLine("Loading quests...");
            questsData = new Dictionary<int, IQuest>();
            var results = Select(new string[] { "id", "title", "description", "next_quest",
                "objectives", "quest_completioner_unit_id" }, 
                "quests_data");

            foreach (var row in results)
            {
                int id = Convert.ToInt32(row["id"]);
                string title = row["title"];
                string description = row["description"];
                int nextQuestId = Convert.ToInt32(row["next_quest"]);
                string objctives = row["objectives"];
                int questCompletioner = Convert.ToInt32(row["quest_completioner_unit_id"]);

                questsData.Add(id, new Quest(id, new QuestData(parseQuestObjectives(objctives), 
                    title, description, nextQuestId, questCompletioner)));
            }
        }

        /// <summary>
        /// Loads the level xp requirements.
        /// </summary>
        /// <param name="requirements">The requirements.</param>
        public void LoadLevelXpRequirements(out List<int> requirements)
        {
            Console.WriteLine("Loading level xp requirements...");
            requirements = new List<int>();
            // TODO: generalize asc/desc in Select function
            string query = "SELECT level, xp FROM level_xp_requirements ORDER BY level ASC";
            var cmd = new MySqlCommand(query, connection);
            var resultReader = cmd.ExecuteReader();

            while(resultReader.Read())
            {
                // TODO: this conevrsions: really???
                requirements.Add(Convert.ToInt32(resultReader["xp"].ToString()));
            }

            resultReader.Close();
        }

        // DATABASE OBJECTIVES DATA GRAMMAR
        //  Objectives = Objective ["," Objective]
        //  Objective = Type [ ":" AdditionalInfo ]
        //  Type = 1 - kill objective, AdditionalInfo = uid:count:current
        //  Type = 2 - region visited, AdditionalInfo = x:y:distance
        //  Type = 3 - unit visited,   AdditionalInfo = uid
        private IQuestObjective[] parseQuestObjectives(string data)
        {
            if (data == "")
                return new IQuestObjective[0];

            string[] objectives = data.Split(',');
            IQuestObjective[] iobjectives = new IQuestObjective[objectives.Length];
            for(int i = 0; i < objectives.Length; i++)
            {
                string[] objectiveData = objectives[i].Split(':');
                int type = Convert.ToInt32(objectiveData[0]);
                switch (type)
                {
                    case 1:
                        iobjectives[i] = parseKillObjective(objectiveData); break;
                    case 2:
                        iobjectives[i] = parseRegionVisited(objectiveData); break;
                    case 3:
                        iobjectives[i] = parseUnitVisited(objectiveData); break;
                    default:
                        throw new NotImplementedException("Unknown quest objective type in database.");
                }
            }

            return iobjectives;
        }

        private UnitKillObjective parseKillObjective(string[] data)
        {
            int uid = Convert.ToInt32(data[1]);
            int count = Convert.ToInt32(data[2]);
            return new UnitKillObjective(uid, count);
        }

        private RegionVisitedObjective parseRegionVisited(string[] data)
        {
            int x = Convert.ToInt32(data[1]);
            int y = Convert.ToInt32(data[2]);
            int distance = Convert.ToInt32(data[3]);
            return new RegionVisitedObjective(new Point(x, y), distance);
        }

        private UnitVisitedObjective parseUnitVisited(string[] data)
        {
            int uid = Convert.ToInt32(data[1]);
            return new UnitVisitedObjective(uid);
        }

        /// <summary>
        /// Saves a character to the database.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <param name="create">If set to true, a new entry into the database is
        /// created.</param>
        /// <param name="accName">If the parameter create is true then accoun is needed.</param>
        public void SaveCharacter(Character c, bool create, string accName = null)
        {
            int xLoc = c.GetLocation().X;
            int yLoc = c.GetLocation().Y;
            int mapid = c.GetLocation().MapID;
            int hitpoints = c.GetActualHitPoints();
            int manapoints = c.GetActualManaPoints();
            int xp = c.Experience;
            int level = c.Level;
            int questId = c.CurrentQuest.QuestID;
            string weapon = createItemString(c.Equip.Items[Data.ItemWeaponSlot]);
            string chest = createItemString(c.Equip.Items[Data.ItemChestSlot]);
            string head = createItemString(c.Equip.Items[Data.ItemHeadSlot]);
            string pants = createItemString(c.Equip.Items[Data.ItemPantsSlot]);

            // build query
            string query = "";
            if (create)
            {
                if (accName == null)
                    throw new ArgumentException("Account cannot be null when creating new character.");
                
                // TODO insert only needed (non implicit) values
                query = "INSERT INTO characters(xloc, yloc, mapid, hitpoints, manapoints, experience, " +
                    "level, questid, equip_weapon, equip_chest, equip_head, equip_pants, name, spec, account) " +
                    "VALUES (" + xLoc + "," + yLoc + "," + mapid + "," + hitpoints + "," + 
                    manapoints + "," + xp + "," + level + "," + questId + ",\"" + weapon + "\",\"" +
                    chest + "\",\"" + head + "\",\"" + pants + "\"," + "\"" + c.Name + "\"," + c.Spec + "," +
                    "\"" + accName + "\")";
            }
            else
            {
                query = "UPDATE characters " +
                    "SET xloc=" + xLoc + ",yloc=" + yLoc + ",mapid=" + mapid + ",hitpoints=" +
                    hitpoints + ",manapoints=" + manapoints + ",experience=" + xp + ",level=" +
                    level + ",questid=" + questId + ",equip_weapon=\"" + weapon + "\",equip_chest=\"" +
                    chest + "\",equip_head=\"" + head + "\",equip_pants=\"" + pants + "\" " +
                    "WHERE name=\"" + c.Name + "\"";
            }

            // execute
            var cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Saves and account to the database.
        /// </summary>
        /// <param name="acc">The account.</param>
        public void SaveAccount(Account acc)
        {
            string name = acc.Username;
            string pass = acc.Password;

            Insert(new string[] { "name", "password" }, 
                new List<string[]> { new string[] { name, pass } }, "accounts");
        }

        private string createItemString(IItem item)
        {
            if (item == null)
                return "";

            int uid = item.UniqueID;
            StringBuilder str = new StringBuilder(uid.ToString());

            if (item.Stats.HitPoints != 0)
                str.Append(",H:").Append(item.Stats.HitPoints);

            if (item.Stats.ManaPoints != 0)
                str.Append(",M:").Append(item.Stats.ManaPoints);

            if (item.Stats.Armor != 0)
                str.Append(",A:").Append(item.Stats.Armor);

            if (item.Stats.Damage != 0)
                str.Append(",D:").Append(item.Stats.Damage);

            if (item.Stats.SpellBonus != 0)
                str.Append("S:").Append(item.Stats.SpellBonus);

            return str.ToString();
        }

        // *******************************************************
        // QUERIES
        // *******************************************************

        /// <summary>
        /// Represents the INSERT query.
        /// </summary>
        /// <param name="insertingColumns">Name of the inserting columns.</param>
        /// <param name="rows">The rows to be inserted.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <exception cref="Exception">Wrong number of values specified in insert statement.</exception>
        private void Insert(string[] insertingColumns, List<string[]> rows, string tableName)
        {
            if (insertingColumns.Length == 0 || rows.Count == 0)
                return;

            int columnnsCount = insertingColumns.Length;

            // build query
            var queryBuilder = new StringBuilder("INSERT INTO ").Append(tableName);
            queryBuilder.Append("(").Append(insertingColumns[0]);
            for (int i = 1; i < insertingColumns.Length; i++)
                queryBuilder.Append(",").Append(insertingColumns[i]);
            queryBuilder.Append(") VALUES ");

            bool first = true;
            foreach (var row in rows)
            {
                if (row.Length != columnnsCount)
                    // TODO: too lazy to create custom exception
                    throw new Exception("Wrong number of values specified in insert statement.");

                if (!first)
                    queryBuilder.Append(",");
                first = false;

                queryBuilder.Append("(");
                queryBuilder.Append("\"" + row[0] + "\"" );
                for (int i = 1; i < row.Length; i++)
                    queryBuilder.Append(",").Append("\"").Append(row[i].ToString()).Append("\"");
                queryBuilder.Append(")");
            }

            // execute
            var cmd = new MySqlCommand(queryBuilder.ToString(), connection);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Represents the SELECT query.
        /// </summary>
        /// <param name="columns">The columns to be selected.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>List containing hashmaps -> the hashmap's key is a column
        /// and the value is the value that was selected from a row in that column.</returns>
        private List<Dictionary<string, string>> Select(string[] columns, string tableName)
        {
            if (columns.Length == 0)
                return null;

            // create query
            var queryBuilder = new StringBuilder("SELECT " + columns[0]);
            for (int i = 1; i < columns.Length; i++)
                queryBuilder.Append("," + columns[i]);
            queryBuilder.Append(" FROM " + tableName);
            var query = queryBuilder.ToString();

            // execute command
            var cmd = new MySqlCommand(query, connection);
            var resultReader = cmd.ExecuteReader();

            // read results
            var results = new List<Dictionary<string, string>>();
            while (resultReader.Read())
            {
                var row = new Dictionary<string, string>();
                foreach (string column in columns)
                {
                    row.Add(column, resultReader[column].ToString());
                }
                results.Add(row);
            }

            resultReader.Close();

            return results;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, 
        /// or resetting unmanaged resources (connection closure).
        /// </summary>
        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
