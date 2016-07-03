using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public partial class ServerConnection
    {
        public int RegisterAccount(string username, string password)
        {
            if(!Connect())
                return RESULT_CANTCONNECT;

            if(!SendOne(ServerCommands.RegisterAccount(username, password)))
                return RESULT_CANTSEND;

            string msg = ReceiveOne();
            if (msg == string.Empty)
                return RESULT_EMPTY;

            int i = ServerResponses.GetResponse(msg);
            if (i != ServerResponses.RESPONSE_OK)
                return RESULT_FALSE;

            return RESULT_OK;
        }
    }
}
