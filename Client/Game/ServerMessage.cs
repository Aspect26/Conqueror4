using Shared;
using System;
using System.Drawing;

namespace Client
{
    public partial class Game
    {
        private const int MSG_UNITS_DATA = 5;
        private const int MSG_RELOAD_DATA = 6;

        /// <summary>
        /// Processes the server message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessServerMessage(string message)
        {
            string[] parts = message.Split(':');

            int cmdNumber = Convert.ToInt32(parts[0]);
            switch (cmdNumber)
            {
                case MSG_UNITS_DATA:
                    handleUnitsData(parts[1]); break;
                case MSG_RELOAD_DATA:
                    handleLoadData(parts[1]); break;
            }
        }

        private void handleLoadData(string arguments)
        {
            // remove all the current data
            this.units.Clear();
            this.missiles.Clear();
            this.objects.Clear();
            this.collidingObjects.Clear();
            this.specialEffects.Clear();
            this.droppedItem = null;

            server.ParseResponseCharacterLoad(arguments, this, this.Character);
            this.Map.Create(GameData.GetMapFilePath(this.Character.Location.MapID));
        }

        private void handleUnitsData(string arguments)
        {
            // TODO: totally separate this function into multiple smaller functions OMG WTF SRSLY
            string[] unitStrings = arguments.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string unitString in unitStrings)
            {
                string[] unitParts = unitString.Split('|');

                int uniqueId = Convert.ToInt32(unitParts[0]);

                // ITEM DIFERENCE
                if(uniqueId == SharedData.DIFFERENCE_TYPE_ITEM)
                {
                    string[] itemPart = unitParts[1].Split('&');
                    if (itemPart[0] == "R")
                    {
                        RemoveGameObject(Convert.ToInt32(itemPart[1]));
                    }
                    continue;
                }

                // UNIT DIFFERENCE
                for (int currentIndex = 1; currentIndex < unitParts.Length; currentIndex++)
                {
                    string[] unitPart = unitParts[currentIndex].Split('&');
                    if (unitPart[0] == "L" && uniqueId != Character.UniqueID)
                    {
                        int x = Convert.ToInt32(unitPart[1]);
                        int y = Convert.ToInt32(unitPart[2]);
                        UpdateUnitLocation(uniqueId, x, y);
                    }
                    else if (unitPart[0] == "S")
                    {
                        int x = Convert.ToInt32(unitPart[1]);
                        int y = Convert.ToInt32(unitPart[2]);
                        AddMissile(uniqueId, x, y);
                    }
                    else if (unitPart[0] == "H")
                    {
                        int hp = Convert.ToInt32(unitPart[1]);
                        UpdateUnitActualHitPoints(uniqueId, hp);
                    }
                    else if (unitPart[0] == "MN" && uniqueId == Character.UniqueID)
                    {
                        int mp = Convert.ToInt32(unitPart[1]);
                        Character.ActualStats.ManaPoints = mp;
                    }
                    else if (unitPart[0] == "X")
                    {
                        int xp = Convert.ToInt32(unitPart[1]);
                        ChangePlayerXp(xp);
                    }
                    else if (unitPart[0] == "LV")
                    {
                        int level = Convert.ToInt32(unitPart[1]);
                        int xpRequired = Convert.ToInt32(unitPart[2]);
                        ChangePlayerLevel(level, xpRequired);
                    }
                    else if(unitPart[0] == "D")
                    {
                        KillUnit(uniqueId);
                    }
                    else if(unitPart[0] == "R")
                    {
                        string name = unitPart[1];
                        int unitId = Convert.ToInt32(unitPart[2]);
                        int xLoc = Convert.ToInt32(unitPart[3]);
                        int yLoc = Convert.ToInt32(unitPart[4]);
                        int maxHp = Convert.ToInt32(unitPart[5]);
                        int maxMp = Convert.ToInt32(unitPart[6]);
                        int actualHp = Convert.ToInt32(unitPart[7]);
                        int actualMp = Convert.ToInt32(unitPart[8]);
                        int fraction = Convert.ToInt32(unitPart[9]);

                        AddUnit(name, unitId, uniqueId, xLoc, yLoc, new BaseStats(maxHp, maxMp, 0, 0), 
                            new BaseStats(actualHp, actualMp, 0, 0), fraction);
                    }
                    else if(unitPart[0] == "Q")
                    {
                        UpdateQuest(Quest.CreateQuest(unitParts[currentIndex], Character.Name));
                    }
                    else if(unitPart[0] == "QO")
                    {
                        UpdateQuestObjectives(Quest.CreateObjectives(unitPart, 1));
                    }
                    else if(unitPart[0] == "FM")
                    {
                        int mapId = Convert.ToInt32(unitPart[1]);
                        int x = Convert.ToInt32(unitPart[2]);
                        int y = Convert.ToInt32(unitPart[3]);
                        UpdateUnitLocation(uniqueId, new Location(mapId, x, y));
                    }
                    else if(unitPart[0] == "A")
                    {
                        int hp = Convert.ToInt32(unitPart[1]);
                        int mp = Convert.ToInt32(unitPart[2]);
                        UpdateActualStats(uniqueId, new BaseStats(hp, mp, 0, 0, 0));
                    }
                    else if(unitPart[0] == "M")
                    {
                        int hp = Convert.ToInt32(unitPart[1]);
                        int mp = Convert.ToInt32(unitPart[2]);
                        UpdateMaxStats(uniqueId, new BaseStats(hp, mp, 0, 0, 0));
                    }
                    else if(unitPart[0] == "I")
                    {
                        IItem item = parseItem(unitPart, 3);
                        Point location = new Point(Convert.ToInt32(unitPart[1]), 
                            Convert.ToInt32(unitPart[2]));
                        CreateItem(item, location);
                    }
                    else if(unitPart[0] == "IE" && uniqueId == Character.UniqueID)
                    {
                        IItem item = parseItem(unitPart, 1);
                        EquipItem(item);
                    } 
                    else if(unitPart[0] == "AB")
                    {
                        ISpecialEffect effect = SpecialEffect.ParseAbilityEffect(this, unitParts[1]);
                        if(effect != null)
                            AddSpecialEffect(effect);
                    }
                }
            }
        }

        private IItem parseItem(string[] parts, int offset)
        {
            // type
            int slot = Convert.ToInt32(parts[offset]);

            // uid
            int uid = Convert.ToInt32(parts[offset+1]);

            // stats
            ItemStats stats = new ItemStats();
            for(int i = offset+2; i<parts.Length; i++)
            {
                string[] details = parts[i].Split('^');
                int amount = Convert.ToInt32(details[1]);
                switch (details[0])
                {
                    case "H":
                        stats.HitPoints = amount; break;
                    case "M":
                        stats.ManaPoints = amount; break;
                    case "A":
                        stats.Armor = amount; break;
                    case "D":
                        stats.Damage = amount; break;
                    case "S":
                        stats.SpellBonus = amount; break;
                    default:
                        throw new NotImplementedException("Server send unknown item bonus value");
                }
            }

            // actual item
            return new Item(stats, slot, uid);
        }
    }
}
