using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Account
    {
        string Username;
        string Password;

        public Account(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}
