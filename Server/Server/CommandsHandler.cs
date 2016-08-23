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

        private const int CMD_REGISTER_ACCOUNT = 1;
        private const int CMD_LOGIN_ACCOUNT = 2;
        private const int CMD_CHARACTERS_LIST = 3;
        private const int CMD_CHARACTERLOAD = 4;
        private const int CMD_STARTMOVING = 5;
        private const int CMD_STOPMOVING = 6;
        private const int CMD_CHANGELOCATION = 7;
        private const int CMD_SHOOT = 8;
        private const int CMD_TAKEITEM = 9;
        private const int CMD_USEABILITY = 10;
        private const int CMD_CREATECHARACTER = 11;

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
                case CMD_CREATECHARACTER:
                    handleCreateCharacter(clientState, arguments[0], arguments[1], Convert.ToInt32(arguments[2])); break;
                case CMD_REGISTER_ACCOUNT:
                    handleRegisterAccounts(clientState, arguments[0], arguments[1]); break;
                case CMD_LOGIN_ACCOUNT:
                    handleLoginAccount(clientState, arguments[0], arguments[1]); break;
                case CMD_CHARACTERS_LIST:
                    handleGetCharactersList(clientState); break;
                case CMD_CHARACTERLOAD:
                    handleCharacterLoad(clientState, arguments[0].ToLower()); break;
                case CMD_CHANGELOCATION:
                    handleCharacterMove(clientState, Convert.ToInt32(arguments[0]), Convert.ToInt32(arguments[1]));
                    break;
                case CMD_SHOOT:
                    handleCharacterShoot(clientState, Convert.ToInt32(arguments[0]), Convert.ToInt32(arguments[1]));
                    break;
                case CMD_TAKEITEM:
                    handleTakeItem(clientState, Convert.ToInt32(arguments[0])); break;
                case CMD_USEABILITY:
                    handleUseAbility(clientState); break;
                default:
                    return;
            }
        }

        // ************************************************
        // SPECIFIC HANDLERS
        // ************************************************
        private void handleCreateCharacter(StateObject client, string username, string name, int spec)
        {
            Character c = Data.CreateCharacter(username.ToLower(), name.ToLower(), spec);

            byte[] byteData;
            if (c != null)
                byteData = Encoding.ASCII.GetBytes("1\n");
            else
                byteData = Encoding.ASCII.GetBytes("0\n");

            client.clientSocket.Send(byteData, 0, byteData.Length, 0);
        }

        private void handleLoginAccount(StateObject client, string name, string password)
        {
            bool result = server.LoginAccount(client, name, password);
            byte[] byteData;
            if (result)
                byteData = Encoding.ASCII.GetBytes("1\n");
            else
                byteData = Encoding.ASCII.GetBytes("0\n");

            client.clientSocket.Send(byteData, 0, byteData.Length, 0);
        }

        private void handleRegisterAccounts(StateObject client, string name, string password)
        {
            bool result = server.RegisterAccount(name, password);
            byte[] byteData;
            if (result)
                byteData = Encoding.ASCII.GetBytes("1\n");
            else
                byteData = Encoding.ASCII.GetBytes("0\n");

            client.clientSocket.Send(byteData, 0, byteData.Length, 0);
            server.DisconnectClient(client);
        }

        private void handleGetCharactersList(StateObject client)
        {
            string charactersMessage = "";
            foreach (Character character in client.Account.GetCharacters())
            {
                charactersMessage += character.Name + "," + character.Level + "," + character.Spec + "|";
            }

            byte[] byteData = Encoding.ASCII.GetBytes(charactersMessage + "\r\n");
            client.clientSocket.Send(byteData, 0, byteData.Length, SocketFlags.None);
        }

        private void handleUseAbility(StateObject client)
        {
            server.AddPlayerActionToGameQueue(new CharacterUseAbilityAction(client.PlayingCharacter));
        }

        private void handleTakeItem(StateObject client, int itemUid)
        {
            server.AddPlayerActionToGameQueue(new CharacterTakesItemAction(client.PlayingCharacter, itemUid));
        }

        private void handleCharacterMove(StateObject client, int x, int y)
        {
            server.AddPlayerActionToGameQueue(new CharacterMovedAction(client.PlayingCharacter, x, y));
        }

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
            MapInstance map = game.AddPlayer(client, character);
            string msg = character.UniqueID + "," + character.Location.MapID + "," + 
                character.Experience + "," + Data.GetNextLevelXPRequired(character.Level) + "," + 
                character.Location.X + "," + character.Location.Y + "," + character.GetMaxHitPoints() + "," + 
                character.GetMaxManaPoints() + "," +  character.GetActualHitPoints() + "," + 
                character.GetActualManaPoints() + "," + character.Fraction + "," + 
                character.CurrentQuest.GetCodedData() + "," + character.Equip.GetCodedData() + ",";
            msg += map.GetMessageCodedData() + "\n";
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
