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
        private Data gameData = new Data();
        private Game game;
        private Queue<IPlayerAction> playerActions;
        private Queue<ISendAction> sendActions;

        private static int PORT = 26270;
        private IPEndPoint localEndPoint;
        private Socket serverSocket;

        private List<StateObject> clients = new List<StateObject>();

        public Server()
        {
            commandsHandler = new CommandsHandler(this);
        }

        public void Start()
        {
            localEndPoint = new IPEndPoint(IPAddress.Loopback, PORT);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Loading accounts database...");
            gameData = Data.createMockData();

            Console.WriteLine("Starting server...");

            // initialize game
            playerActions = new Queue<IPlayerAction>();
            sendActions = new Queue<ISendAction>();
            game = new Game();
            Task gameInitializationTask = Task.Factory.StartNew(() => game.Initialize(playerActions, sendActions));

            // then start game and accepting, receiving and sending data to clients
            Task gameTask = gameInitializationTask.ContinueWith((parent) => game.Start(), TaskContinuationOptions.LongRunning);
            Task receivingTask = gameInitializationTask.ContinueWith((parent) => AcceptAndReceiveClients(), TaskContinuationOptions.LongRunning);
            Task sendingTask = gameInitializationTask.ContinueWith((parent) => SendClients(), TaskContinuationOptions.LongRunning);

            Console.WriteLine("Server ended. Press any key to terminate.");
            Console.ReadKey(true);
            return;
        }

        public void AddPlayerActionToGameQueue(IPlayerAction action)
        {
            lock (playerActions)
            {
                playerActions.Enqueue(action);
            }
        }
    }
}
