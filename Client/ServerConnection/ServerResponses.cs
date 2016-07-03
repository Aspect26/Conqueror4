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
    }
}
