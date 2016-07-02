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

        private object streaming = new object();
        private Game game;

        public bool Connected { get; set; }

        public ServerConnection(Game game)
        {
            Connected = false;
            this.game = game;
        }

        public bool Start()
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
                    new Thread(() => Receive()).Start();
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

        private void Receive()
        {
            try
            {
                while (true)
                {
                    string line;
                    line = input.ReadLine();
                    //handleServerMessage(line);
                    Console.WriteLine("RECEIVED: " + line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(string data)
        {
            lock (streaming)
            {
                if (!stream.CanWrite) Console.WriteLine("ERROR: can not write to server stream.");
                output.WriteLine(data);
                output.Flush();
            }
        }

        private void RequestCommand(int cmdid, string args)
        {
            Send(cmdid + ":" + args);
        }
    }
}
