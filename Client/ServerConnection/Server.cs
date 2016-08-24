using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    /// <summary>
    /// Represents the server connection. This class is divided into files that
    /// are all in the ServerConnection folder. This file represents core of the server
    /// connection.
    /// </summary>
    public partial class ServerConnection
    {
        private const string IP = "127.0.0.1";
        private const int port = 26270;
        private const int bufferSize = 1024;
        private Socket serverSocket;
        private byte[] buffer = new byte[bufferSize];

        private NetworkStream stream;
        private StreamReader input;
        private StreamWriter output;

        private Application application;

        /// <summary>
        /// Gets or sets a value indicating whether the connection is established.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConnection"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public ServerConnection(Application application)
        {
            Connected = false;
            this.application = application;
        }

        /// <summary>
        /// Tries to connect to the server.
        /// </summary>
        /// <returns><c>true</c> if the operation is successful, <c>false</c> otherwise.</returns>
        public bool Connect()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IP), port);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                serverSocket.Connect(endPoint);
                stream = new NetworkStream(serverSocket);
                if (stream.CanRead)
                {
                    input = new StreamReader(stream);
                    output = new StreamWriter(stream);
                    Console.WriteLine("Connected to the server");
                    Connected = true;
                    return true;
                }
                else
                {
                    Console.WriteLine("ERROR: Can`t read from server socket.");
                    stream.Close();
                    serverSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return false;
        }

        /// <summary>
        /// Terminates the connection.
        /// </summary>
        public void End()
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        }

        /// <summary>
        /// A server result - ok
        /// </summary>
        public const int RESULT_OK = 0;

        /// <summary>
        /// A server result - can't connect
        /// </summary>
        public const int RESULT_CANTCONNECT = 1;

        /// <summary>
        /// A server result - can't send message
        /// </summary>
        public const int RESULT_CANTSEND = 2;

        /// <summary>
        /// A server result is empty
        /// </summary>
        public const int RESULT_EMPTY = 3;

        /// <summary>
        /// A server result - false
        /// </summary>
        public const int RESULT_FALSE = 4;

        /// <summary>
        /// Sends one message to the server
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if the operation is succesfull, <c>false</c> otherwise.</returns>
        private bool SendOne(string data)
        {
            if (!stream.CanWrite)
            {
                Console.WriteLine("ERROR: can not write to server stream.");
                return false;
            }

            output.WriteLine(data);
            output.Flush();
            return true;
        }

        /// <summary>
        /// Receives one message from the server (or waits for it!!)
        /// </summary>
        /// <returns>The server message</returns>
        private string ReceiveOne()
        {
            try
            {
                string line = input.ReadLine();
                Console.WriteLine("Server Message: " + line);
                return line;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// Tries to read one message from the server. Returns false if there is no
        /// message from the server.
        /// </summary>
        /// <returns>The message or null.</returns>
        public string TryReadOneMessage()
        {
            if (stream.DataAvailable)
            {
                string msg = input.ReadLine();
                Console.WriteLine("READ 1 MSG: " + msg);
                return msg;
            }

            else return null;
        }
    }
}
