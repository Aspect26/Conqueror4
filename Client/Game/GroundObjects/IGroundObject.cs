using System.Drawing;

namespace Client
{
    public interface IGroundObject
    {
        void Render(Graphics g);

        Point Location { get; }

        int GetCollisionDistance();
        void Collide();
    }
}
