using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server.Server
{
    public partial class Server
    {
        private ManualResetEvent newConnectionEstablishedEvent = new ManualResetEvent(false);

        private void AcceptAndReceiveClients()
        {
            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                Console.WriteLine("Started accepting and receiving clients...");
                while (true)
                {
                    newConnectionEstablishedEvent.Reset();
                    serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);
                    newConnectionEstablishedEvent.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void AcceptCallback(IAsyncResult result)
        {
            newConnectionEstablishedEvent.Set();
            Socket clientSocket = serverSocket.EndAccept(result);
            ClientState clientState = new ClientState();
            clientState.clientSocket = clientSocket;

            clientSocket.BeginReceive(clientState.buffer, 0, ClientState.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveClient), clientState);
        }

        private void ReceiveClient(IAsyncResult result)
        {
            ClientState state = (ClientState)result.AsyncState;
            Socket socket = state.clientSocket;
            int bytesRead = socket.EndReceive(result);

            if (bytesRead > 0)
            {
                state.dataReceived.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                string content = state.dataReceived.ToString();

                if (content.IndexOf("<EOF>") > -1)
                {
                    // Whole message received
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);

                    // Continue receiving
                    socket.BeginReceive(state.buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveClient), state);
                }
                else
                {
                    // Not all data received. Get more.
                    socket.BeginReceive(state.buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveClient), state);
                }
            }
        }
    }
}
