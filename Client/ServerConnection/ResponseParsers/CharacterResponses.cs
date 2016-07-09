using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class ServerConnection
    {
        private bool parseResponseCharacterLoad(string message, MainPlayerCharacter character)
        {
            if (message == "")
                return false;

            string[] parts = message.Split(',');
            if (parts.Length != 3)
                return false;

            int mapID, mapY, mapX;
            try {
                mapID = Convert.ToInt32(parts[0]);
                mapX = Convert.ToInt32(parts[1]);
                mapY = Convert.ToInt32(parts[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            character.Location.MapID = mapID;
            character.Location.X = mapX;
            character.Location.Y = mapY;

            return true;
        }

        private bool parseResponseCharactersList(string message, Account account)
        {
            if (message == "")
                return false;

            foreach (string charString in message.Split('|'))
            {
                string[] parts = charString.Split(',');
                if (parts.Length == 3)
                {
                    account.AddCharacter(parts[0], Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));
                }
            }

            return true;
        }
    }
}
