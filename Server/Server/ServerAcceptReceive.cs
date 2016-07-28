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
        // ACCEPTING AND RECEIVING CYCLE
        //**************************************************
        private void AcceptAndReceiveClients()
        {
            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                Console.WriteLine("Task for accepting and receiving clients started...");
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
        // FIRST MESSAGE CALLBACK (LOGIN OR REGISTER)
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
                    Console.WriteLine("First message from user: {0}", clientSocket.LocalEndPoint);
                    state.dataReceived = new StringBuilder();
                    string[] msgParts = content.Split(':');
                    bool actionResult;
                    bool loggingIn = false;

                    if (msgParts.Length != 2)
                    {
                        actionResult = false;
                    }
                    else if (msgParts[0] == "1")
                    {
                        actionResult = registerAccount(msgParts[1].Split(new char[] { ',' },
                            StringSplitOptions.RemoveEmptyEntries));
                    }
                    else if (msgParts[0] == "2")
                    {
                        actionResult = loginAccount(state, msgParts[1].Split(new char[] { ',' },
                            StringSplitOptions.RemoveEmptyEntries));
                        loggingIn = true;
                    }
                    else
                    {
                        actionResult = false;
                    }

                    byte[] byteData;
                    if (actionResult)
                        byteData = Encoding.ASCII.GetBytes("1\n");
                    else
                        byteData = Encoding.ASCII.GetBytes("0\n");

                    clientSocket.Send(byteData, 0, byteData.Length, 0);

                    // continue sending characters list if logging in
                    if (!loggingIn)
                        return;

                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(CharactersRequest), state);
                }
                else
                {
                    // Not all data received. Get more.
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(FirstClientMessage), state);
                }
            }
        }

        //**************************************************
        // CHARACTERS LIST REQUEST
        //**************************************************
        private void CharactersRequest(IAsyncResult result)
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
                    Console.WriteLine("Characters list request from: {0}", state.Account.Username);
                    state.dataReceived = new StringBuilder();
                    content = content.Replace("\r", "").Replace("\n", "");
                    if (content == "3:")
                    {
                        string charactersMessage = "";
                        foreach(Character character in state.Account.GetCharacters())
                        {
                            charactersMessage += character.Name + "," + character.Level + "," + character.Spec + "|";
                        }

                        byte[] byteData = Encoding.ASCII.GetBytes(charactersMessage + "\r\n");
                        clientSocket.Send(byteData, 0, byteData.Length, SocketFlags.None);

                        clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceivingCallback), state);
                    }
                }
                else
                {
                    // Not all data received. Get more.
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(CharactersRequest), state);
                }
            }
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
                clients.Remove(state);
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
