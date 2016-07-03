using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerResponses
    {
        public static readonly int RESPONSE_UNKNOWN = -1;
        public static readonly int RESPONSE_FALSE = 0;
        public static readonly int RESPONSE_OK = 1;

        public static int GetResponse(string message)
        {
            if (message == "1")
                return RESPONSE_OK;
            else if (message == "0")
                return RESPONSE_FALSE;

            return RESPONSE_UNKNOWN;
        }

        public static bool LoadCharacters(Account account, string message)
        {
            if (message == "")
                return false;

            foreach(string charString in message.Split('|'))
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
