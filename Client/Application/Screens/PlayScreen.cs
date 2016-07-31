using System;
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

        public PlayScreen(Application application, ServerConnection server) : base(application, server)
        {
            this.server = server;

            playerCharacter = application.Account.PlayCharacter;
            application.server.LoadCharacter(playerCharacter);

            game = new Game(application.Account.PlayCharacter);

            lastTimeStamp = Stopwatch.GetTimestamp();
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            long now = Stopwatch.GetTimestamp();
            int elapsedMilis = (int)((1000 * (now - lastTimeStamp)) / Stopwatch.Frequency);
            lastTimeStamp = now;

            string msg;
            while ((msg = server.ReadOneMessage()) != null)
            {
                game.ProcessServerMessage(msg);
            }

            game.RunCycle(g, elapsedMilis);
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
            game.AddMissile(new Missile(game, GameData.GetMissileImage(playerCharacter.ID), 
                new Point(playerCharacter.Location.X, playerCharacter.Location.Y), new Point(dirX, dirY)));
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
