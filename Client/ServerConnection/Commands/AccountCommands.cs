namespace Client
{
    /// <summary>
    /// This part of the ServerConnection class handles client's commands connected
    /// with its account (e.g.: login, register, ...)
    /// </summary>
    public partial class ServerConnection
    {
        /// <summary>
        /// Command to the server to create a character for the specified account with 
        /// specified nickname and specialization
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="nickname">The nickname.</param>
        /// <param name="spec">The specialization.</param>
        /// <returns>The result message id.</returns>
        public int CreateCharacter(string account, string nickname, int spec)
        {
            int res = trySend(CMD_CREATE_CHARACTER, new string[] { account, nickname, spec.ToString() });
            if (res != RESULT_OK)
                return res;

            string msg = ReceiveOne();
            int response = parseBooleanResponse(msg);

            if (response != RESPONSE_OK)
                return RESULT_FALSE;
            else
                return RESULT_OK;
        }

        /// <summary>
        /// Command to the server to register a new account with specified username 
        /// and password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The result message id.</returns>
        public int RegisterAccount(string username, string password)
        {
            Connect();
            int res = trySend(CMD_REGISTER, new string[] { username, password });
            if (res != RESULT_OK)
                return res;

            string msg = ReceiveOne();
            if (msg == string.Empty)
            {
                End();
                return RESULT_EMPTY;
            }

            int i = parseBooleanResponse(msg);
            if (i != RESPONSE_OK)
            {
                End();
                return RESULT_FALSE;
            }

            End();
            return RESULT_OK;
        }

        /// <summary>
        /// Commands the server to login accounts with specified username and
        /// password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The result message id.</returns>
        public int LoginAccount(string username, string password)
        {
            Connect();
            int res = trySend(CMD_LOGIN, new string[] { username, password });
            if (res != RESULT_OK)
                return res;

            string msg = ReceiveOne();
            if (msg == string.Empty)
            {
                End();
                return RESULT_EMPTY;
            }

            int i = parseBooleanResponse(msg);
            if (i != RESPONSE_OK)
            {
                End();
                return RESULT_FALSE;
            }

            return RESULT_OK;
        }
    }
}
