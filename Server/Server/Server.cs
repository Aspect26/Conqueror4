using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    public partial class Server
    {
        private Data gameData;
        private Game game;

        private static int PORT = 26270;
        private IPEndPoint localEndPoint;
        private Socket serverSocket;

        private List<StateObject> clients = new List<StateObject>();

        public Server()
        {
            game = new Game();
            gameData = new Data(game);
            commandsHandler = new CommandsHandler(this, game);
        }

        public void Start()
        {
            localEndPoint = new IPEndPoint(IPAddress.Any, PORT);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Loading accounts database...");
            gameData = Data.createMockData(game);

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
            clients.Remove(client);
            game.RemoveClient(client);
        }

        public void AddPlayerActionToGameQueue(IPlayerAction action)
        {
            game.AddPlayerAction(action);
        }
    }
}
