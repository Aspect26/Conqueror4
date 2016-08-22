using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Server
{
    public partial class Server
    {
        private Game game;

        private static int PORT = 26270;
        private IPEndPoint localEndPoint;
        private Socket serverSocket;

        private List<StateObject> clients = new List<StateObject>();

        public Server()
        {
        }

        public void Start()
        {
            if (!Data.Initialize())
            {
                Console.WriteLine("Could not load server data.");
                return;
            }
            game = new Game();
            commandsHandler = new CommandsHandler(this, game);

            localEndPoint = new IPEndPoint(IPAddress.Any, PORT);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Loading accounts database...");

            Console.WriteLine("Starting server...");

            // THREADS
            new Thread(game.Start).Start();
            new Thread(AcceptAndReceiveClients).Start();
            SendClients();

            Console.WriteLine("Server ended. Press any key to terminate.");
            Console.ReadKey(true);
            return;
        }

        public void DisconnectClient(StateObject client)
        {
            Data.SaveCharacter(client.PlayingCharacter);
            clients.Remove(client);
            game.RemoveClient(client);
        }

        public void AddPlayerActionToGameQueue(IPlayerAction action)
        {
            game.AddPlayerAction(action);
        }
    }
}
