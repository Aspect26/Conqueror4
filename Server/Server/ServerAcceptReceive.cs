using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public partial class Server
    {
        private ManualResetEvent newConnectionEstablishedEvent = new ManualResetEvent(false);
        private CommandsHandler commandsHandler;

        //**************************************************
        // ACCEPTING CYCLE
        //**************************************************
        private void AcceptAndReceiveClients()
        {
            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                Console.WriteLine("Task for accepting and receiving clients started...");
                while (true)
                {
                    newConnectionEstablishedEvent.Reset();
                    serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);
                    newConnectionEstablishedEvent.WaitOne(); // because the cycle wouldn't stop
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //**************************************************
        // ACCEPTED CLIENT CALLBACK
        //**************************************************
        private void AcceptCallback(IAsyncResult result)
        {
            newConnectionEstablishedEvent.Set();
            Socket clientSocket = serverSocket.EndAccept(result);
            StateObject clientState = new StateObject();
            clientState.clientSocket = clientSocket;

            lock(clients)
                clients.Add(clientState);

            Console.WriteLine("New connection from: " + clientSocket.LocalEndPoint);
            clientSocket.BeginReceive(clientState.buffer, 0, StateObject.BufferSize, 
                SocketFlags.None, new AsyncCallback(ReceivingCallback), clientState);
        }

        //**************************************************
        // GENERIC COMMANDS RECEIVING
        //**************************************************
        private void ReceivingCallback(IAsyncResult result)
        {
            StateObject state = (StateObject)result.AsyncState;
            Socket clientSocket = state.clientSocket;
            int bytesRead = 0;
            try {
                bytesRead = clientSocket.EndReceive(result);
            } catch(SocketException e)
            {
                Console.WriteLine("Lost connection with: " + state.Account.Username);
                state.Account.LoggedIn = false;
                DisconnectClient(state);
            }

            if (bytesRead > 0)
            {
                state.dataReceived.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                string content = state.dataReceived.ToString();

                if (content.IndexOf("\n") > -1)
                {
                    // Whole message received
                    state.dataReceived = new StringBuilder();
                    //Console.WriteLine("A generic message from: {0}\n {1}", clientSocket.LocalEndPoint, content);

                    // Handle message
                    commandsHandler.HandleMessage(state, content);

                    // Continue receiving
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceivingCallback), state);
                }
                else
                {
                    // Not all data received. Get more.
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceivingCallback), state);
                }
            }
        }
    }
}
