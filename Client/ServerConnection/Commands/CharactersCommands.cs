namespace Client
{
    /// <summary>
    /// This part of the ServerConnection class handles client's commands connected
    /// with its character
    /// </summary>
    public partial class ServerConnection
    {
        /// <summary>
        /// Commands the server to get the information about current state in the map
        /// that the player is trying to load into.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="character">The character.</param>
        /// <returns>The result message id.</returns>
        public int LoadGame(Game game, PlayedCharacter character)
        {
            int res = trySend(CMD_LOADCHAR, new string[] { character.Name });
            if (res != RESULT_OK)
                return res;

            string msg = ReceiveOne();
            while (msg.Split(':')[0] != "6")
            {
                msg = ReceiveOne();
            }

            if (!ParseResponseCharacterLoad(msg.Split(':')[1], game, character))
                return RESULT_FALSE;

            return RESULT_OK;
        }

        /// <summary>
        /// Commands the server to get the list of characters in the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>The result message id.</returns>
        public int GetCharacters(Account account)
        {
            int res = trySend(CMD_CHARACTERS, new string[] { });
            if (res != RESULT_OK)
                return res;

            if (!parseResponseCharactersList(ReceiveOne(), account))
                return RESULT_FALSE;

            return RESULT_OK;
        }
    }
}
