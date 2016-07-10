using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class ServerConnection
    {
        public int LoadCharacter(PlayedCharacter character)
        {
            int res = trySend(CMD_LOADCHAR, new string[] { character.Name });
            if (res != RESULT_OK)
                return res;

            if (!parseResponseCharacterLoad(ReceiveOne(), character))
                return RESULT_FALSE;

            return RESULT_OK;
        }

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
