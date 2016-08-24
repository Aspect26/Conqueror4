using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents the screen that is shown when the user actually plays the game with selected character.
    /// </summary>
    /// <seealso cref="Client.EmptyScreen" />
    public class PlayScreen : EmptyScreen
    {
        private const int playerSize = 50;

        private PlayedCharacter playerCharacter;
        private Game game;
        private long lastTimeStamp;
        private CenterMessages centerMessages;
        private Font centerMessagesFont;

        private QuestLog questLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayScreen"/> class.
        /// Initializs the interface components and loads the character, other players, NPCs and game objects
        /// from the server. 
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="server">The server connection.</param>
        public PlayScreen(Application application, ServerConnection server) : base(application, server)
        {
            this.server = server;

            playerCharacter = application.Account.PlayCharacter;
            application.server.LoadGame(out game, playerCharacter);
            questLog = new QuestLog(new Rectangle(10, 80, 200, 250), playerCharacter);

            userInterface.AddComponent(new CharacterStatus(application.Account.PlayCharacter));
            userInterface.AddComponent(questLog);

            // TODO: actively wait for game to load :( <- loading screen or something
            while (game == null) ;
            userInterface.AddComponent(new RightPlayOverlay(game, playerCharacter));
            game.NewQuestAcquired += OnPlayerAquiredNewQuest;

            centerMessages = new CenterMessages();
            centerMessagesFont = GameData.GetFont(12);

            lastTimeStamp = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// Called when player aquired new quest. Shows a message with the new quest's title in the middle of the screen.
        /// </summary>
        /// <param name="quest">The new quest.</param>
        public void OnPlayerAquiredNewQuest(IQuest quest)
        {
            centerMessages.AddMessage("New quest acquired: " + quest.Title);
        }

        /// <summary>
        /// Firstly read all the messages sent from the server and handles them. Then runs one game cycle (moves all
        /// the objects to their new location, e.g.: player, NPCs, missiles, ...) and then renders all the objects. It 
        /// also render a center message if there is any.
        /// </summary>
        /// <param name="g">Graphics object.</param>
        /// <seealso cref="Client.CenterMessages"/>
        public override void Render(Graphics g)
        {
            // TODO: remove this when ready
            g.Clear(Color.Black);

            long now = Stopwatch.GetTimestamp();
            int elapsedMilis = (int)((1000 * (now - lastTimeStamp)) / Stopwatch.Frequency);
            lastTimeStamp = now;

            string msg;
            while ((msg = server.ReadOneMessage()) != null)
            {
                game.ProcessServerMessage(msg);
            }

            game.RunCycle(g, elapsedMilis);

            userInterface.Render(g);

            // center messages
            IList<string> messages = centerMessages.GetMessages();
            int y = 65;
            lock (messages)
            {
                foreach (string message in messages)
                {
                    float width = g.MeasureString(message, centerMessagesFont).Width;
                    g.DrawString(message, centerMessagesFont, Brushes.Yellow,
                        Application.WIDTH / 2 - width / 2, y);

                    y += 20;
                }
            }
        }

        // *************************************************
        // EVENTS
        // *************************************************
        int previousDownKey = 0;

        // TODO: in this function -> player.Shoot, nevytvarat strelu v screene -_-
        /// <summary>
        /// Called when [left mouse button down] -> user tries to shoot a missile.
        /// </summary>
        /// <param name="location">The location.</param>
        public override void OnMouseLeftDown(Point location)
        {
            playerCharacter.TryShoot(location);
        }

        /// <summary>
        /// Called when [key down]. Checks which key is down and does a appropriate action.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyDown(int key)
        {
            if (key == previousDownKey)
                return;

            previousDownKey = key;

            switch (key)
            {
                case 68:        // D
                    playerCharacter.StartMovingRight();
                    Console.WriteLine("START RIGHT");
                    break;

                case 83:        // S
                    playerCharacter.StartMovingBottom();
                    Console.WriteLine("START BOTTOM");
                    break;

                case 65:        // A
                    playerCharacter.StartMovingLeft();
                    Console.WriteLine("START LEFT");
                    break;

                case 87:        // W
                    playerCharacter.StartMovingUp();
                    Console.WriteLine("START UP");
                    break;

                case 81:        // Q
                    questLog.SetShown(true);
                    break;
            }
        }

        /// <summary>
        /// Called when [key up]. Checks which key is up and does a appropriate action.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyUp(int key)
        {
            if (key == previousDownKey)
                previousDownKey = 0;

            switch (key)
            {
                case 68:        // D
                    playerCharacter.StopMovingRight();
                    Console.WriteLine("START RIGHT");
                    break;

                case 83:        // S
                    playerCharacter.StopMovingBottom();
                    Console.WriteLine("START BOTTOM");
                    break;

                case 65:        // A
                    playerCharacter.StopMovingLeft();
                    Console.WriteLine("START LEFT");
                    break;

                case 87:        // W
                    playerCharacter.StopMovingUp();
                    Console.WriteLine("START UP");
                    break;

                case 81:        // Q
                    questLog.SetShown(false);
                    break;

                case 84:        // T
                    game.TryTakeDroppedItem();
                    break;

                case 32:        // SPACE
                    game.TryUseAbility();
                    break;
            }
        }
    }
}
