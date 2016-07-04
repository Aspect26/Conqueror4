using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class ServerConnection
    {
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
