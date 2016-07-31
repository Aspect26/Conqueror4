using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class ServerConnection
    {
        private bool parseResponseCharacterLoad(string message, out Game game, PlayedCharacter character)
        {
            game = null;
            if (message == "")
                return false;

            string[] parts = message.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int myUid = -1;
            try {
                int uid = Convert.ToInt32(parts[0]);
                int mapID = Convert.ToInt32(parts[1]);
                int mapX = Convert.ToInt32(parts[2]);
                int mapY = Convert.ToInt32(parts[3]);

                myUid = uid;
                character.SetUniqueID(uid);
                character.Location.MapID = mapID;
                character.Location.X = mapX;
                character.Location.Y = mapY;

                game = new Game(character);
            }
            catch (FormatException)
            {
                return false;
            }

            for(int i = 4; i<parts.Length; i++)
            {
                string[] unitParts = parts[i].Split('|');

                int uid = Convert.ToInt32(unitParts[0]);
                if (uid == myUid)
                    continue;

                int id = Convert.ToInt32(unitParts[1]);
                string name = unitParts[2];
                int xLoc = Convert.ToInt32(unitParts[3]);
                int yLoc = Convert.ToInt32(unitParts[4]);

                game.AddOrUpdateUnit(name, id, uid, xLoc, yLoc);
            }

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
