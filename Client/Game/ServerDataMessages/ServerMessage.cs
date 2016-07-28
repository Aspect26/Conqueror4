using System;

namespace Client
{
    public partial class Game
    {
        private const int MSG_UNITS_DATA = 5;

        public void ProcessServerMessage(string message)
        {
            string[] parts = message.Split(':');
            if (parts.Length != 2) //TODO: Disconnect from server?
                return;

            int cmdNumber = Convert.ToInt32(parts[0]);
            switch (cmdNumber)
            {
                case MSG_UNITS_DATA:
                    handleUnitsData(parts[1]); break;
            }
        }

        private void handleUnitsData(string arguments)
        {
            string[] unitStrings = arguments.Split(',');

            foreach (string unitString in unitStrings)
            {
                string[] unitParts = unitString.Split('|');

                string name = unitParts[0];
                int spec = Convert.ToInt32(unitParts[1]);

                if (name == Character.Name)
                {
                    continue;
                }
                else
                {
                    for (int currentIndex = 2; currentIndex < unitParts.Length; currentIndex++)
                    {
                        string[] unitPart = unitParts[currentIndex].Split(':');
                        if (unitPart[0] == "L")
                        {
                            int x = Convert.ToInt32(unitPart[1]);
                            int y = Convert.ToInt32(unitPart[2]);
                            AddOrUpdateUnit(name, spec, x, y);
                        }
                        else if(unitPart[0] == "S")
                        {
                            int x = Convert.ToInt32(unitPart[1]);
                            int y = Convert.ToInt32(unitPart[2]);
                        }
                    }
                }
            }
        }
    }
}
