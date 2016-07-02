using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Server.Server
{
    public class ClientState
    {
        public Socket clientSocket = null;
        public static int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder dataReceived = new StringBuilder();

        public bool receiving = false; 
    }

    public partial class Server
    {
        private static int PORT = 26270;
        private IPEndPoint localEndPoint;
        private Socket serverSocket;

        private List<ClientState> clients = new List<ClientState>();

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
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            ClientState state = (ClientState)result.AsyncState;
            int bytesRead = serverSocket.EndReceive(result);

            if(bytesRead > 0)
            {
                state.dataReceived.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                string content = state.dataReceived.ToString();

                if(content.IndexOf("<EOF>") > -1)
                {
                    // data receiving finished
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                }
                else
                {
                    // data receiving not finished yet
                    serverSocket.BeginReceive(state.buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
            }
        }
    }
}
