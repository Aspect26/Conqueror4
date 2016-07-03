using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Data
    {
        Dictionary<string, Account> accountData = new Dictionary<string, Account>();

        public bool RegisterAccount(string username, string password)
        {
            if (accountData.ContainsKey(username))
                return false;

            accountData.Add(username, new Account(username, password));
            return true;
        }
    }
}
