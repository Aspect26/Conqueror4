using System.Drawing;
using Shared;

namespace Client
{
    public class UnitAnimation : IAnimation
    {
        protected string baseImagePath;
        protected SimpleUnit unit;

        protected const int FRONT = 0;
        protected const int RIGHT = 1;
        protected const int BACK = 2;
        protected const int LEFT = 3;

        protected Image[][] images;
        protected Image noAnimationImage;
        protected bool moving;

        protected int currentImageSide;
        protected int currentImageIndex;

        protected const int CHANGE_SPEED_MS = 400;
        protected int lastChangeBefore = 499;
        protected MovingDirection lastDirection = MovingDirection.None;

        protected Game game;

        public UnitAnimation(Game game, SimpleUnit unit, string baseImagePath)
        {
            this.unit = unit;
            this.baseImagePath = baseImagePath;
            this.game = game;

            createImages();
        }

        public UnitAnimation(SimpleUnit unit, string baseImagePath)
        {
            this.unit = unit;
            this.baseImagePath = baseImagePath;

            createImages();
        }

        private void createImages()
        {
            noAnimationImage = Image.FromFile(baseImagePath + "_none.png");

            images = new Image[4][];

            images[FRONT] = new Image[2];
            images[FRONT][0] = Image.FromFile(baseImagePath + "_front1.png");
            images[FRONT][1] = Image.FromFile(baseImagePath + "_front2.png");

            images[LEFT] = new Image[2];
            images[LEFT][0] = Image.FromFile(baseImagePath + "_side1.png"); images[LEFT][0].RotateFlip(RotateFlipType.RotateNoneFlipX);
            images[LEFT][1] = Image.FromFile(baseImagePath + "_side2.png"); images[LEFT][1].RotateFlip(RotateFlipType.RotateNoneFlipX);

            images[RIGHT] = new Image[2];
            images[RIGHT][0] = Image.FromFile(baseImagePath + "_side1.png");
            images[RIGHT][1] = Image.FromFile(baseImagePath + "_side2.png");

            images[BACK] = new Image[2];
            images[BACK][0] = Image.FromFile(baseImagePath + "_back1.png");
            images[BACK][1] = Image.FromFile(baseImagePath + "_back2.png");

            currentImageSide = FRONT;
            currentImageIndex = 0;
            moving = false;
        }

        public virtual void Render(Graphics g)
        {
            int size = unit.UnitSize;
            Point position = game.MapPositionToScreenPosition((int)unit.Location.X, (int)unit.Location.Y);

            if (!moving)
            {
                g.DrawImageAt(noAnimationImage, position.X, position.Y, size, size);
            }
            else
            {
                g.DrawImageAt(images[currentImageSide][currentImageIndex], position.X, position.Y, size, size);
            }
        }

        public void AnimateCycle(int timeSpan)
        {
            lastChangeBefore += timeSpan;

            if(unit.Direction != lastDirection)
            {
                changeSide();
                return;
            }

            if (unit.Direction == MovingDirection.None)
            {
                moving = false;
                return;
            }

            if (lastChangeBefore > CHANGE_SPEED_MS)
            {
                // increase index
                lastChangeBefore = lastChangeBefore % CHANGE_SPEED_MS;
                currentImageIndex = (currentImageIndex + 1) % images[currentImageSide].Length;
            }
        }

        private void changeSide()
        {
            lastChangeBefore = 499;
            currentImageIndex = 0;
            moving = true;

            switch (unit.Direction)
            {
                case MovingDirection.Right:
                    currentImageSide = RIGHT; break;

                case MovingDirection.Bottom:
                    currentImageSide = FRONT; break;

                case MovingDirection.Left:
                    currentImageSide = LEFT; break;

                case MovingDirection.Up:
                    currentImageSide = BACK; break;

                default:
                    moving = false; break;
            }
            lastDirection = unit.Direction;
        }

        public Image GetCurrentImage()
        {
            if (!moving)
                return noAnimationImage;
            else
                return images[currentImageSide][currentImageIndex];
        }
    }
}
