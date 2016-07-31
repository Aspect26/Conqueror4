using Shared;
using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{ 
    public class CommandsHandler
    {
        private Server server;
        private Game game;

        private const int CMD_CHARACTERLOAD = 4;
        private const int CMD_STARTMOVING = 5;
        private const int CMD_STOPMOVING = 6;
        private const int CMD_CHANGELOCATION = 7;
        private const int CMD_SHOOT = 8;

        public CommandsHandler(Server server, Game game)
        {
            this.server = server;
            this.game = game;
        }

        public void HandleMessage(StateObject clientState, string message)
        {
            string[] parts = message.Split(':');
            if (parts.Length != 2)
                return;

            int cmdNumber;
            try
            {
                cmdNumber = Convert.ToInt32(parts[0]);
            }
            catch (FormatException)
            {
                return;
            }

            string[] arguments = parts[1].Split(',');

            switch (cmdNumber)
            {
                case CMD_CHARACTERLOAD:
                    handleCharacterLoad(clientState, arguments[0].ToLower()); break;
                case CMD_CHANGELOCATION:
                    int x = Convert.ToInt32(arguments[0]);
                    int y = Convert.ToInt32(arguments[1]);
                    lock (clientState.PlayingCharacter)
                    {
                        clientState.PlayingCharacter.Location.X = x;
                        clientState.PlayingCharacter.Location.Y = y;
                        clientState.PlayingCharacter.Updated = true;
                    }
                    break;
                case CMD_SHOOT:
                    handleCharacterShoot(clientState, Convert.ToInt32(arguments[0]), Convert.ToInt32(arguments[1]));
                    break;
                default:
                    return;
            }
        }

        // ************************************************
        // SPECIFIC HANDLERS
        // ************************************************
        private void handleCharacterShoot(StateObject client, int x, int y)
        {
            server.AddPlayerActionToGameQueue(new CharacterShootAction(client.PlayingCharacter, x, y));
        }

        private void handleCharacterLoad(StateObject client, string characterName)
        {
            Character character = Data.GetCharacter(characterName);
            if (character == null)
                return;

            client.PlayingCharacter = character;

            // send data back
            int uid = game.AddPlayer(client, character);
            string msg = uid + "," + character.Location.MapID + "," + character.Location.X + "," + character.Location.Y + "\n";
            byte[] byteData = Encoding.ASCII.GetBytes(msg);
            client.clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), 
                client.clientSocket);
        }

        // ************************************************
        // HELPER FUNCTIONS
        // ************************************************
        private MovingDirection intToDirection(int direction)
        {
            switch (direction)
            {
                case 1:
                    return MovingDirection.Up;
                case 2:
                    return MovingDirection.Right;
                case 3:
                    return MovingDirection.Bottom;
                case 4:
                    return MovingDirection.Left;

                default:
                    return MovingDirection.None;
            }
        }

        // ************************************************
        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            int bytes = handler.EndSend(result);
            Console.WriteLine("Sent {0} bytes to client.", bytes);
        }
    }
}
