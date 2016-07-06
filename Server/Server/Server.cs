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
        private Queue<ISendAction> sendActions;

        private static int PORT = 26270;
        private IPEndPoint localEndPoint;
        private Socket serverSocket;

        private List<StateObject> clients = new List<StateObject>();

        public Server()
        {
            game = new Game();
            commandsHandler = new CommandsHandler(this, game);
        }

        public void Start()
        {
            localEndPoint = new IPEndPoint(IPAddress.Loopback, PORT);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Loading accounts database...");
            gameData = Data.createMockData();

            Console.WriteLine("Starting server...");

            // initialize game
            sendActions = new Queue<ISendAction>();
            Task gameInitializationTask = Task.Factory.StartNew(() => game.Initialize(sendActions));

            // then start game task, receiving commands task and sending data task
            Task gameTask = gameInitializationTask.ContinueWith((parent) => game.Start(), TaskContinuationOptions.LongRunning);
            Task receivingTask = gameInitializationTask.ContinueWith((parent) => AcceptAndReceiveClients(), TaskContinuationOptions.LongRunning);
            Task sendingTask = gameInitializationTask.ContinueWith((parent) => SendClients(), TaskContinuationOptions.LongRunning);

            Console.WriteLine("Server ended. Press any key to terminate.");
            Console.ReadKey(true);
            return;
        }

        public void AddPlayerActionToGameQueue(IPlayerAction action)
        {
            game.AddPlayerAction(action);
        }
    }
}
