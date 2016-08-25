using System.Text;

namespace Client
{
    /// <summary>
    /// The Commands section of the ServerConnection class represents all the commands
    /// that the player can send to the server.
    /// </summary>
    public partial class ServerConnection
    {
        private static int CMD_REGISTER = 1;
        private static int CMD_LOGIN = 2;
        private static int CMD_CHARACTERS = 3;
        private static int CMD_LOADCHAR = 4;
        private static int CMD_STARTMOVING = 5;
        private static int CMD_STOPMOVING = 6;
        private static int CMD_CHANGELOCATION = 7;
        private static int CMD_SHOOT = 8;
        private static int CMD_TAKEITEM = 9;
        private static int CMD_USEABILITY = 10;
        private static int CMD_CREATE_CHARACTER = 11;
        private static int CMD_CHANGE_MAP = 12;

        /// <summary>
        /// Tries to send a message to the server with specified command identifier
        /// and arguments.
        /// </summary>
        /// <param name="commandNumber">The command number.</param>
        /// <param name="args">The command arguments.</param>
        /// <returns>The corresponding result id.</returns>
        private int trySend(int commandNumber, string[] args)
        {
            if (!Connected)
            {
                return RESULT_FALSE;
            }

            if (!SendOne(command(commandNumber, args)))
            { 
                return RESULT_CANTSEND;
            }

            return RESULT_OK;
        }

        private static string command(int cmd, string[] args)
        {
            return commandNumber(cmd) + arguments(args);
        }

        private static string commandNumber(int number)
        {
            return number.ToString() + ":";
        }

        private static string arguments(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
                sb.Append(args[i]).Append(",");
            return sb.ToString();
        }
    }
}
