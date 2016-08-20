using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Client
{
    public class PlayScreen : EmptyScreen
    {
        private const int playerSize = 50;

        private PlayedCharacter playerCharacter;
        private Game game;
        private long lastTimeStamp;
        private CenterMessages centerMessages;
        private Font centerMessagesFont;

        private QuestLog questLog;

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
            userInterface.AddComponent(new BottomPlayOverlay(playerCharacter));
            game.NewQuestAcquired += PlayerAquiredQuestMessage;

            centerMessages = new CenterMessages();
            centerMessagesFont = GameData.GetFont(12);

            lastTimeStamp = Stopwatch.GetTimestamp();
        }

        public void PlayerAquiredQuestMessage(IQuest quest)
        {
            centerMessages.AddMessage("New quest acquired: " + quest.Title);
        }

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
        public override void OnMouseLeftDown(Point location)
        {
            int x = location.X - Application.MIDDLE.X;
            int y = location.Y - Application.MIDDLE.Y;
            double length = Math.Sqrt(x*x + y*y);

            int dirX = (int)((x / length) * 100);
            int dirY = (int)((y / length) * 100);
            server.SendPlayerShoot(dirX, dirY);
            /*
            game.AddMissile(new Missile(game, playerCharacter, GameData.GetMissileImage(playerCharacter.UnitID), 
                new Point(playerCharacter.Location.X, playerCharacter.Location.Y), new Point(dirX, dirY)));*/
        }

        public override void OnKeyDown(int key)
        {
            if (key == previousDownKey)
                return;

            previousDownKey = key;

            if (key == 68)      // D
            {
                playerCharacter.StartMovingRight();
                Console.WriteLine("START RIGHT");
            }
            else if(key == 83)  // S
            {
                playerCharacter.StartMovingBottom();
                Console.WriteLine("START BOTTOM");
            }
            else if(key == 65)   // A
            {
                playerCharacter.StartMovingLeft();
                Console.WriteLine("START LEFT");
            }
            else if(key == 87)  // W
            {
                playerCharacter.StartMovingUp();
                Console.WriteLine("START UP");
            } 
            else if(key == 81) // Q
            {
                questLog.SetShown(!questLog.Shown);
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == previousDownKey)
                previousDownKey = 0;

            if (key == 68)      // D
            {
                playerCharacter.StopMovingRight();
                Console.WriteLine("STOP RIGHT");
            }
            else if (key == 83)  // S
            {
                playerCharacter.StopMovingBottom();
                Console.WriteLine("STOP BOTTOM");
            }
            else if (key == 65)   // A
            {
                playerCharacter.StopMovingLeft();
                Console.WriteLine("STOP LEFT");
            }
            else if (key == 87)  // W
            {
                playerCharacter.StopMovingUp();
                Console.WriteLine("STOP UP");
            }
        }
    }
}
