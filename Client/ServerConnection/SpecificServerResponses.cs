using Shared;
using System;

namespace Client
{
    /// <summary>
    /// This source file of ServerConnection class contains server responses's parser.
    /// </summary>
    public partial class ServerConnection
    {
        public static readonly int RESPONSE_NOTBOOLEAN = -1;
        public static readonly int RESPONSE_FALSE = 0;
        public static readonly int RESPONSE_OK = 1;

        /// <summary>
        /// Parses a boolean response.
        /// </summary>
        /// <param name="message">The server message.</param>
        /// <returns>The corresponding constant.</returns>
        public static int parseBooleanResponse(string message)
        {
            if (message == "1")
                return RESPONSE_OK;
            else if (message == "0")
                return RESPONSE_FALSE;

            return RESPONSE_NOTBOOLEAN;
        }

        // TODO: the function is horribly long
        /// <summary>
        /// Parses a server message that is received immediatelly after the player enters
        /// the world of Conqueror 4!
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="game">The game.</param>
        /// <param name="character">The character.</param>
        /// <returns><c>true</c> if the operation is successful, <c>false</c> otherwise.</returns>
        public bool ParseResponseCharacterLoad(string message, Game game, PlayedCharacter character)
        {
            if (message == "")
                return false;

            string[] parts = message.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int myUid = -1;

            // MYSELF
            try
            {
                int uid = Convert.ToInt32(parts[0]);
                int mapID = Convert.ToInt32(parts[1]);
                int experience = Convert.ToInt32(parts[2]);
                int experienceRequired = Convert.ToInt32(parts[3]);
                int mapX = Convert.ToInt32(parts[4]);
                int mapY = Convert.ToInt32(parts[5]);
                int maxHp = Convert.ToInt32(parts[6]);
                int maxMp = Convert.ToInt32(parts[7]);
                int actualHp = Convert.ToInt32(parts[8]);
                int actualMp = Convert.ToInt32(parts[9]);
                int fraction = Convert.ToInt32(parts[10]);
                IQuest quest = Quest.CreateQuest(parts[11], character.Name);
                Equip equip = Equip.ParseEquip(parts[12]);

                myUid = uid;
                character.SetUniqueID(uid);
                character.Location.MapID = mapID;
                character.Experience = experience;
                character.ExperienceRequired = experienceRequired;
                character.Location.X = mapX;
                character.Location.Y = mapY;
                character.MaxStats.HitPoints = maxHp;
                character.MaxStats.ManaPoints = maxMp;
                character.ActualStats.HitPoints = actualHp;
                character.ActualStats.ManaPoints = actualMp;
                character.SetFraction(fraction);
                character.SetCurrentQuest(quest);
                character.SetEquip(equip);
            }
            catch (FormatException e)
            {
                Console.WriteLine("COULD NOT LOAD GAME :(");
                return false;
            }

            // OTHER THINGS
            for (int i = 13; i < parts.Length; i++)
            {
                string[] dataParts = parts[i].Split('|');

                // it is a unit if it starts with a number otherwise it is an object
                int uid;
                var isUnit = int.TryParse(dataParts[0], out uid);
                if (isUnit)
                {
                    // UNIT
                    int id = Convert.ToInt32(dataParts[1]);
                    string name = dataParts[2];
                    int xLoc = Convert.ToInt32(dataParts[3]);
                    int yLoc = Convert.ToInt32(dataParts[4]);

                    int maxHp = Convert.ToInt32(dataParts[5]);
                    int actualHp = Convert.ToInt32(dataParts[6]);

                    int fraction = Convert.ToInt32(dataParts[7]);

                    BaseStats maxStats = new BaseStats(maxHp, 0, 0, 0, 0);
                    BaseStats actualStats = new BaseStats(actualHp, 0, 0, 0, 0);

                    game.AddUnit(name, id, uid, xLoc, yLoc, maxStats, actualStats, fraction);
                }
                else
                {
                    // OBJECT
                    game.AddObject(dataParts);
                }
            }

            return true;
        }

        /// <summary>
        /// Parser the server message with account's characters list.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if the operation is successful, <c>false</c> otherwise.</returns>
        private bool parseResponseCharactersList(string message, Account account)
        {
            if (message == "")
                return true;

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
