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

                string name = unitParts[0];
                int unitId = Convert.ToInt32(unitParts[1]);
                int uniqueId = Convert.ToInt32(unitParts[2]);

                if (name == Character.Name)
                {
                    continue;
                }
                else
                {
                    for (int currentIndex = 3; currentIndex < unitParts.Length; currentIndex++)
                    {
                        string[] unitPart = unitParts[currentIndex].Split('&');
                        if (unitPart[0] == "L")
                        {
                            int x = Convert.ToInt32(unitPart[1]);
                            int y = Convert.ToInt32(unitPart[2]);
                            AddOrUpdateUnit(name, unitId, uniqueId, x, y);
                        }
                        else if(unitPart[0] == "S")
                        {
                            int x = Convert.ToInt32(unitPart[1]);
                            int y = Convert.ToInt32(unitPart[2]);
                            AddMissile(uniqueId, x, y);
                        }
                    }
                }
            }
        }
    }
}
