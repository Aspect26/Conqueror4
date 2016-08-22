using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class Server
    {
        private bool registerAccount(string[] args)
        {
            if (args.Length < 2)
                return false;

            return Data.RegisterAccount(args[0], args[1]);
        }

        private bool loginAccount(StateObject state, string[] args)
        {
            if (args.Length < 2)
                return false;

            state.Account = Data.LoginAccount(args[0], args[1]);
            return state.Account != null;
        }
    }
}
