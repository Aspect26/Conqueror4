namespace Server
{
    // TODO: remove this class?
    public class CharacterEnterLocationAction : IPlayerAction
    {
        Character character;
        Game game;
        StateObject clientState;

        public CharacterEnterLocationAction(Game game, StateObject clientState, Character character)
        {
            this.character = character;
            this.game = game;
            this.clientState = clientState;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public void Process(MapInstance instance, long timeStamp)
        {
            return;
        }
    }
}
