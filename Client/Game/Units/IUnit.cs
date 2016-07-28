using System.Drawing;

namespace Client
{
    public interface IUnit
    {
        void PlayCycle(int timeSpan);
        void DrawUnit(Graphics g);
        Image GetCurrentImage();
        void SetLocation(int x, int y);

        void StartMovingUp();
        void StartMovingRight();
        void StartMovingBottom();
        void StartMovingLeft();

        void StopMovingUp();
        void StopMovingRight();
        void StopMovingBottom();
        void StopMovingLeft();
    }
}
