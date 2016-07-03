using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public static class ServerCommands
    {
        private static int CMD_REGISTER = 1;
        private static int CMD_LOGIN = 2;
        private static int CMD_CHARACTERS = 3;

        public static string RegisterAccount(string username, string password)
        {
            return command(CMD_REGISTER, new string[] { username, password });
        }

        public static string LoginAccount(string username, string password)
        {
            return command(CMD_LOGIN, new string[] { username, password });
        }

        public static string GetCharacters()
        {
            return command(CMD_CHARACTERS, new string[] {});
        }

        // ************************************************
        // Helper functions
        // ************************************************

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
