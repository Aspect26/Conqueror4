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
            if (args.Length != 3)
                return false;

            return gameData.RegisterAccount(args[0], args[1]);
        }

        private bool loginAccount(string[] args)
        {
            return false;
        }
    }
}
