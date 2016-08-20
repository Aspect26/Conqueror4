using System.Drawing;

namespace Client
{
    public interface IGroundObject
    {
        void Render(Graphics g);
        int UniqueID { get; }

        Point Location { get; }

        int GetCollisionDistance();
        void Collide();
        void Leave();
    }
}
