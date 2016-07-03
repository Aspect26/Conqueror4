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

            if (!SendOne(ServerCommands.RegisterAccount(username, password)))
            {
                End();
                return RESULT_CANTSEND;
            }

            string msg = ReceiveOne();
            if (msg == string.Empty)
            {
                End();
                return RESULT_EMPTY;
            }

            int i = ServerResponses.GetResponse(msg);
            if (i != ServerResponses.RESPONSE_OK)
            {
                End();
                return RESULT_FALSE;
            }

            End();
            return RESULT_OK;
        }

        public int LoginAccount(string username, string password)
        {
            if (!Connect())
                return RESULT_CANTCONNECT;

            if (!SendOne(ServerCommands.LoginAccount(username, password)))
            {
                End();
                return RESULT_CANTSEND;
            }

            string msg = ReceiveOne();
            if (msg == string.Empty)
            {
                End();
                return RESULT_EMPTY;
            }

            int i = ServerResponses.GetResponse(msg);
            if (i != ServerResponses.RESPONSE_OK)
            {
                End();
                return RESULT_FALSE;
            }

            return RESULT_OK;
        }

        public int GetCharacters(Account account)
        {
            if (!Connected)
                return RESULT_FALSE;

            if (!SendOne(ServerCommands.GetCharacters()))
            {
                return RESULT_CANTSEND;
            }

            string msg = ReceiveOne();
            if (!ServerResponses.LoadCharacters(account, msg))
                return RESULT_FALSE;

            return RESULT_OK;
        }
    }
}
