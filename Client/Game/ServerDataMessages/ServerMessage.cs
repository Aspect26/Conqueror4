using Shared;
using System;
using System.Drawing;

namespace Client
{
    public partial class Game
    {
        private const int MSG_UNITS_DATA = 5;

        public void ProcessServerMessage(string message)
        {
            string[] parts = message.Split(':');

            int cmdNumber = Convert.ToInt32(parts[0]);
            switch (cmdNumber)
            {
                case MSG_UNITS_DATA:
                    handleUnitsData(parts[1]); break;
            }
        }

        private void handleUnitsData(string arguments)
        {
            // TODO: separate this into multiple functions
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
                    else if (unitPart[0] == "X")
                    {
                        int xp = Convert.ToInt32(unitPart[1]);
                        ChangePlayerXp(xp);
                    }
                    else if (unitPart[0] == "LV")
                    {
                        int level = Convert.ToInt32(unitPart[1]);
                        ChangePlayerLevel(level);
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
                        int actualHp = Convert.ToInt32(unitPart[6]);
                        int fraction = Convert.ToInt32(unitPart[7]);

                        AddUnit(name, unitId, uniqueId, xLoc, yLoc, new BaseStats(maxHp), new BaseStats(actualHp),
                            fraction);
                    }
                    else if(unitPart[0] == "Q")
                    {
                        UpdateQuest(Quest.CreateQuest(unitParts[currentIndex]));
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
                        UpdateActualStats(uniqueId, new BaseStats(hp));
                    }
                    else if(unitPart[0] == "M")
                    {
                        int hp = Convert.ToInt32(unitPart[1]);
                        UpdateMaxStats(uniqueId, new BaseStats(hp));
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
