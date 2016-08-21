using System.Drawing;

namespace Client
{
    public interface ISpecialEffect
    {
        void Render(Graphics g);
        void PlayCycle(int timeSpan);
        bool IsDead { get; }
    }
}
