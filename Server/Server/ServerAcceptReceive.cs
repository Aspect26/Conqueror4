using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public partial class Server
    {
        private ManualResetEvent newConnectionEstablishedEvent = new ManualResetEvent(false);

        //**************************************************
        // ACCEPTING AND RECEIVING CYCLE
        //**************************************************
        private void AcceptAndReceiveClients()
        {
            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                Console.WriteLine("Thread for accepting and receiving clients started...");
                Console.WriteLine("-------------------------");
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
        // ACCEPTED CALLBACK
        //**************************************************
        private void AcceptCallback(IAsyncResult result)
        {
            newConnectionEstablishedEvent.Set();
            Socket clientSocket = serverSocket.EndAccept(result);
            StateObject clientState = new StateObject();
            clientState.clientSocket = clientSocket;

            clients.Add(clientState);
            Console.WriteLine("New connection from: " + clientSocket.LocalEndPoint);
            clientSocket.BeginReceive(clientState.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(FirstClientMessage), clientState);
        }

        //**************************************************
        // FIRST MESSAGE CALLBACK
        //**************************************************
        private void FirstClientMessage(IAsyncResult result)
        {
            StateObject state = (StateObject)result.AsyncState;
            Socket clientSocket = state.clientSocket;
            int bytesRead = clientSocket.EndReceive(result);

            if (bytesRead > 0)
            {
                state.dataReceived.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                string content = state.dataReceived.ToString();

                if (content.IndexOf("\n") > -1)
                {
                    // Whole message received
                    Console.WriteLine("First message from user: {0}", content);
                    string[] msgParts = content.Split(':');
                    bool actionResult;

                    if (msgParts.Length != 2)
                        actionResult = false;
                    else if (msgParts[0] == "1")
                        actionResult = registerAccount(msgParts[1].Split(','));
                    else if (msgParts[0] == "2")
                        actionResult = loginAccount(msgParts[1].Split(','));
                    else
                        actionResult = false;

                    byte[] byteData;
                    if (actionResult)
                        byteData = Encoding.ASCII.GetBytes("1\n");
                    else
                        byteData = Encoding.ASCII.GetBytes("0\n");

                    clientSocket.Send(byteData, 0, byteData.Length, 0);

                    // Continue receiving
                    // clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(FirstClientMessage), state);
                }
                else
                {
                    // Not all data received. Get more.
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(FirstClientMessage), state);
                }
            }
        }
    }
}
