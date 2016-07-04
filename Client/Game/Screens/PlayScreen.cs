using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PlayScreen : EmptyScreen
    {
        public PlayScreen(Game game): base(game)
        {
            game.server.LoadCharacter(game.Account.PlayCharacter);
        }
    }
}
