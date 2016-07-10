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
                case CMD_STARTMOVING:
                    handleCharacterStartMoving(clientState, Convert.ToInt32(arguments[0])); break;
                case CMD_STOPMOVING:
                    handleCharacterStopMoving(clientState, Convert.ToInt32(arguments[0])); break;
                default:
                    return;
            }
        }

        // ************************************************
        // SPECIFIC HANDLERS
        // ************************************************

        private void handleCharacterStartMoving(StateObject client, int direction)
        {
            server.AddPlayerActionToGameQueue(new CharacterStartMovingAction(game, client.PlayingCharacter,
                intToDirection(direction)));
        }

        private void handleCharacterStopMoving(StateObject client, int direction)
        {
            server.AddPlayerActionToGameQueue(new CharacterStopMovingAction(game, client.PlayingCharacter,
                intToDirection(direction)));
        }

        private void handleCharacterLoad(StateObject client, string characterName)
        {
            Character character = Data.GetCharacter(characterName);
            if (character == null)
                return;

            client.PlayingCharacter = character;

            // send data back
            string msg = character.Location.MapID + "," + character.Location.X + "," + character.Location.Y + "\n";
            byte[] byteData = Encoding.ASCII.GetBytes(msg);
            client.clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), 
                client.clientSocket);

            // add character to game
            server.AddPlayerActionToGameQueue(new CharacterEnterLocationAction(game, client, character));
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
