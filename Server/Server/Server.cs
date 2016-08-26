using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// This is a server core file. It includes server's initialization and some
    /// account oriented actions requested by a player.
    /// </summary>
    public partial class Server
    {
        private Game game;

        /// <summary>
        /// The server runs on port 26270.
        /// </summary>
        private static int PORT = 26270;
        private IPEndPoint localEndPoint;
        private Socket serverSocket;

        /// <summary>
        /// List of states of all connected clients.
        /// </summary>
        private List<StateObject> clients = new List<StateObject>();

        /// <summary>
        /// Starts the server. The server runs in 3 explicit threads (and possibly more
        /// because of usage of threadpool). Firstly the
        /// data from SQL database has to be loaded. If the action is successful
        /// the server divides into 3 threads. One thread is  for main game loop,
        /// one for handling clients (accepting them and receiving messages) and the third
        /// one is used to send messages to clients about the game state and saving
        /// data to the SQL database every X minutes.
        /// </summary>
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

        /// <summary>
        /// Registers a new account and immediatelly saves it to the database.
        /// </summary>
        /// <param name="name">The nickname.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if the operation is successful, <c>false</c> otherwise
        /// (e.g.: if the nickname is already taken).</returns>
        public bool RegisterAccount(string name, string password)
        {
            return Data.RegisterAccount(name, password);
        }

        /// <summary>
        /// Logins a account.
        /// For the sake of simplicity this function returns only true or false so
        /// if the account didn't log into the game, the client doesn't receive
        /// any message informing him why.
        /// </summary>
        /// <param name="state">The client's state object.</param>
        /// <param name="name">The nickname.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if the operation is successful, <c>false</c> otherwise
        /// (e.g.: wrong password, user is already logged in or the account doesn't
        /// exist).</returns>
        public bool LoginAccount(StateObject state, string name, string password)
        {
            state.Account = Data.LoginAccount(name, password);
            return state.Account != null;
        }

        /// <summary>
        /// Disconnects a client from the server.
        /// </summary>
        /// <param name="client">The client's state object.</param>
        public void DisconnectClient(StateObject client)
        {
            clients.Remove(client);
            if (client.PlayingCharacter != null)
            {
                Data.SaveCharacter(client.PlayingCharacter);
                game.RemoveClient(client);
            }
        }

        /// <summary>
        /// Adds a player action to the game ( -> map instance) queue.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddPlayerActionToGameQueue(IPlayerAction action)
        {
            game.AddPlayerAction(action);
        }
    }
}
