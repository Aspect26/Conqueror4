using System.Drawing;

namespace Client
{
    public interface IGroundObject
    {
        void Render(Graphics g);

        int GetCollisionDistance();
        void Collide();
    }
}
