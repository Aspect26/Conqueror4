using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class Server
    {
        private void SendClients()
        {
            Console.WriteLine("Task for sending messages to clients started (currently does nothing)...");
            Console.WriteLine("-------------------------");

            while (true)
            {
                while (sendActions.Count == 0) ;
                lock (sendActions)
                {
                    sendActions.Dequeue().Send();
                }
            }
        }
    }
}
