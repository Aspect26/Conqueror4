using System.Drawing;

namespace Client
{
    public sealed class CreateCharacterButton : BorderedRectangleComponent
    {
        private int spec;

        public CreateCharacterButton(Point parentPosition, Rectangle position, int spec)
            : base(parentPosition, position, GameData.GetUnitImage(spec))
        {
            this.spec = spec;
        }

        public int GetSpec()
        {
            return spec;
        }
    }
}
