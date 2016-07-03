using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Server
{
    public partial class Server
    {
        private Data gameData = new Data();

        private static int PORT = 26270;
        private IPEndPoint localEndPoint;
        private Socket serverSocket;

        private List<StateObject> clients = new List<StateObject>();

        public void Start()
        {
            localEndPoint = new IPEndPoint(IPAddress.Loopback, PORT);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Starting server...");

            Thread receivingThread = new Thread(AcceptAndReceiveClients);
            Thread sendingThread = new Thread(SendClients);

            receivingThread.Start();
            sendingThread.Start();

            receivingThread.Join();

            Console.WriteLine("Server ended. Press any key to terminate.");
            Console.ReadKey(true);
            return;
        }

        private void SendClients()
        {
            Console.WriteLine("Thread for sending messages to clients started...");
            Console.WriteLine("-------------------------");
        }
    }
}
