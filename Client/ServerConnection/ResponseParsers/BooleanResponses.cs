using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class ServerConnection
    {
        public static readonly int RESPONSE_NOTBOOLEAN = -1;
        public static readonly int RESPONSE_FALSE = 0;
        public static readonly int RESPONSE_OK = 1;

        public static int parseBooleanResponse(string message)
        {
            if (message == "1")
                return RESPONSE_OK;
            else if (message == "0")
                return RESPONSE_FALSE;

            return RESPONSE_NOTBOOLEAN;
        }
    }

    // TODO: MOVE THIS SOMEWHERE ELSE
    public static class Extension
    {
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length == 1)
                return input.ToUpper();

            return input.Remove(1).ToUpper() + input.Substring(1);
        }
    }
}
