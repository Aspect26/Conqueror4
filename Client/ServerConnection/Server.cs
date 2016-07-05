using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
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

        //private object streaming = new object();
        private Application game;

        public bool Connected { get; set; }

        public ServerConnection(Application game)
        {
            Connected = false;
            this.game = game;
        }

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

        public void End()
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        }

        public const int RESULT_OK = 0;
        public const int RESULT_CANTCONNECT = 1;
        public const int RESULT_CANTSEND = 2;
        public const int RESULT_EMPTY = 3;
        public const int RESULT_FALSE = 4;

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
    }
}
