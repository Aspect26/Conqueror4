using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

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

            // then start game task, receiving commands task and sending data task

            // TASKS
            //Task gameInitializationTask = Task.Factory.StartNew(() => game.Initialize(sendActions));
            //Task gameTask = gameInitializationTask.ContinueWith((parent) => game.Start(), TaskContinuationOptions.LongRunning);
            //Task receivingTask = gameInitializationTask.ContinueWith((parent) => AcceptAndReceiveClients(), TaskContinuationOptions.LongRunning);
            //Task sendingTask = gameInitializationTask.ContinueWith((parent) => SendClients(), TaskContinuationOptions.LongRunning);

            // THREADS
            new Thread(game.Start).Start();
            new Thread(AcceptAndReceiveClients).Start();
            new Thread(SendClients).Start();

            Console.WriteLine("Server ended. Press any key to terminate.");
            Console.ReadKey(true);
            return;
        }

        public void DisconnectClient(StateObject client)
        {
            // TODO: this function
        }

        public void AddPlayerActionToGameQueue(IPlayerAction action)
        {
            game.AddPlayerAction(action);
        }
    }
}
