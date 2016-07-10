using System.Drawing;
using Shared;

namespace Client
{
    public class PlayedCharacter : SimpleUnit
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Spec { get; set; }
        private ServerConnection server;

        public PlayedCharacter(ServerConnection server, string name, int level, int spec) 
            : base(new Location())
        {
            this.Name = name;
            this.Level = level;
            this.Spec = spec;
            this.server = server;
            this.animation = new CentreUnitAnimation(this, GameData.GetCharacterBasePath(spec));
        }

        // RENDERING
        private int playerSize = 50;
        public override void DrawUnit(Graphics g)
        {
            base.DrawUnit(g);

            g.DrawString(Name, GameData.GetFont(8), Brushes.Black,
                Application.WIDTH / 2 - playerSize / 2, Application.HEIGHT / 2 - playerSize / 2 - 20);
        }

        // MOVING COMMANDS
        // Start
        public override void StartMovingBottom()
        {
            server.StartMovingCharacter(MovingDirection.Bottom);
            base.StartMovingBottom();
        }

        public override void StartMovingRight()
        {
            server.StartMovingCharacter(MovingDirection.Right);
            base.StartMovingRight();
        }

        public override void StartMovingUp()
        {
            server.StartMovingCharacter(MovingDirection.Up);
            base.StartMovingUp();
        }

        public override void StartMovingLeft()
        {
            server.StartMovingCharacter(MovingDirection.Left);
            base.StartMovingLeft();
        }

        // Stop
        public override void StopMovingBottom()
        {
            server.StopMovingCharacter(MovingDirection.Bottom);
            base.StopMovingBottom();
        }

        public override void StopMovingLeft()
        {
            server.StopMovingCharacter(MovingDirection.Left);
            base.StopMovingLeft();
        }

        public override void StopMovingUp()
        {
            server.StopMovingCharacter(MovingDirection.Up);
            base.StopMovingUp();
        }

        public override void StopMovingRight()
        {
            server.StopMovingCharacter(MovingDirection.Right);
            base.StopMovingRight();
        }
    }
}
