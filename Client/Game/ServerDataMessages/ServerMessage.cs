using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (unitParts.Length != 4)
                    continue;

                string name = unitParts[0];

                int spec = Convert.ToInt32(unitParts[1]);
                int x = Convert.ToInt32(unitParts[2]);
                int y = Convert.ToInt32(unitParts[3]);

                if (name == Character.Name)
                {
                    //Character.Location.X = x;
                    //Character.Location.Y = y;
                    continue;
                }
                else
                {
                    AddOrUpdateUnit(name, spec, x, y);
                }
            }
        }
    }
}
