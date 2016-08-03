using Shared;
using System;

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
                int experience = Convert.ToInt32(parts[2]);
                int mapX = Convert.ToInt32(parts[3]);
                int mapY = Convert.ToInt32(parts[4]);
                int maxHp = Convert.ToInt32(parts[5]);
                int actualHp = Convert.ToInt32(parts[6]);
                int fraction = Convert.ToInt32(parts[7]);
                string questData = parts[8];

                myUid = uid;
                character.SetUniqueID(uid);
                character.Location.MapID = mapID;
                character.Experience = experience;
                character.Location.X = mapX;
                character.Location.Y = mapY;
                character.MaxStats.HitPoints = maxHp;
                character.ActualStats.HitPoints = actualHp;
                character.SetFraction(fraction);

                game = new Game(character);
            }
            catch (FormatException e)
            {
                return false;
            }

            for(int i = 9; i<parts.Length; i++)
            {
                string[] unitParts = parts[i].Split('|');

                int uid = Convert.ToInt32(unitParts[0]);

                int id = Convert.ToInt32(unitParts[1]);
                string name = unitParts[2];
                int xLoc = Convert.ToInt32(unitParts[3]);
                int yLoc = Convert.ToInt32(unitParts[4]);

                int maxHp = Convert.ToInt32(unitParts[5]);
                int actualHp = Convert.ToInt32(unitParts[6]);

                int fraction = Convert.ToInt32(unitParts[7]);

                BaseStats maxStats = new BaseStats(maxHp);
                BaseStats actualStats = new BaseStats(actualHp);

                game.AddUnit(name, id, uid, xLoc, yLoc, maxStats, actualStats, fraction);
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
