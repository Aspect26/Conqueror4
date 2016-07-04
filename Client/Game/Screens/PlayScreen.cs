using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PlayScreen : EmptyScreen
    {
        private Image playerImage;
        private const int playerSize = 50;

        private Character character;
        private Map map;

        public PlayScreen(Game game): base(game)
        {
            character = game.Account.PlayCharacter;
            game.server.LoadCharacter(character);

            playerImage = Game.GetCharacterImage(character.Spec);
            map = new Map();
            map.Create(Game.GetMapFilePath(character.Location.MapID));
        }

        public override void Render(Graphics g)
        {
            base.Render(g);
            RenderMap(g);
            RenderPlayer(g);
        }

        private void RenderPlayer(Graphics g)
        {
            g.DrawImage(playerImage, Game.WIDTH / 2 - playerSize / 2, Game.HEIGHT / 2 - playerSize / 2,
                playerSize, playerSize);
        }

        private void RenderMap(Graphics g)
        {
            map.Render(g, character.Location);
        }
    }
}
