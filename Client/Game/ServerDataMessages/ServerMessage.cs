using Shared;
using System;

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
            string[] unitStrings = arguments.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string unitString in unitStrings)
            {
                string[] unitParts = unitString.Split('|');

                int uniqueId = Convert.ToInt32(unitParts[0]);

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
                }
            }
        }
    }
}
