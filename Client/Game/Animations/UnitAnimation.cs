using System.Drawing;
using Shared;

namespace Client
{
    public class UnitAnimation : IAnimation
    {
        protected string baseImagePath;
        protected PlayedCharacter unit;

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

        public UnitAnimation(Game game, PlayedCharacter unit, string baseImagePath)
        {
            this.unit = unit;
            this.baseImagePath = baseImagePath;
            this.game = game;

            createImages();
        }

        public UnitAnimation(PlayedCharacter unit, string baseImagePath)
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
            Point position = game.MapPositionToScreenPosition((int)unit.Location.X, (int)unit.Location.Y);

            if (!moving)
            {
                g.DrawImageAt(noAnimationImage, position.X, position.Y);
            }
            else
            {
                g.DrawImageAt(images[currentImageSide][currentImageIndex], position.X, position.Y);
            }
        }

        public void AnimateCycle(int timeSpan)
        {
            lastChangeBefore += timeSpan;

            if(!AnimationEquals(lastDirection, unit.MovingDirection))
            {
                changeSide();
                return;
            }

            if (unit.MovingDirection == MovingDirection.None)
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

        private bool AnimationEquals(MovingDirection left, MovingDirection right)
        {
            if (left == right)
                return true;

            // UP && UP_RIGHT
            if ( (left == MovingDirection.Up && right.HasFlag(MovingDirection.Up) && right.HasFlag(MovingDirection.Right)) ||
                 (right == MovingDirection.Up && left.HasFlag(MovingDirection.Up) && left.HasFlag(MovingDirection.Right)) )
                return true;

            // RIGHT && BOTTOM_RIGHT
            if ((left == MovingDirection.Right && right.HasFlag(MovingDirection.Right) && right.HasFlag(MovingDirection.Bottom)) ||
                (right == MovingDirection.Right && left.HasFlag(MovingDirection.Right) && left.HasFlag(MovingDirection.Bottom)))
                return true;

            // BOTTOM && BOTTOM_LEFT
            if ((left == MovingDirection.Bottom && right.HasFlag(MovingDirection.Bottom) && right.HasFlag(MovingDirection.Left)) ||
                (right == MovingDirection.Bottom && left.HasFlag(MovingDirection.Bottom) && left.HasFlag(MovingDirection.Left)))
                return true;

            // LEFT && UP_LEFT
            if ((left == MovingDirection.Left && right.HasFlag(MovingDirection.Left) && right.HasFlag(MovingDirection.Up)) ||
                (right == MovingDirection.Left && left.HasFlag(MovingDirection.Left) && left.HasFlag(MovingDirection.Up)))
                return true;

            return false;
        }

        private void changeSide()
        {
            lastChangeBefore = 499;
            currentImageIndex = 0;
            moving = true;

            if (unit.MovingDirection.HasFlag(MovingDirection.Up))
            {
                if (unit.MovingDirection.HasFlag(MovingDirection.Left))
                    currentImageSide = LEFT;
                else
                    currentImageSide = BACK;
            }
            else if (unit.MovingDirection.HasFlag(MovingDirection.Right))
            {
                currentImageSide = RIGHT;
            }
            else if (unit.MovingDirection.HasFlag(MovingDirection.Bottom))
            {
                currentImageSide = FRONT;
            }
            else if (unit.MovingDirection.HasFlag(MovingDirection.Left))
            {
                currentImageSide = LEFT;
            }
            else
            {
                moving = false;
            }

            lastDirection = unit.MovingDirection;
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
