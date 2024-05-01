

using System.Numerics;

namespace Data
{
    internal class Ball : DataAPI
    {
        private Vector2 position;
        private Vector2 newPosition;
        private Vector2 velocity;
        private readonly int size;
        private readonly double weight;


        public Ball(int size, Vector2 position, Vector2 newPosition, Vector2 velocity, double weight)
        {
            this.size = size;
            this.position = position;
            this.newPosition = newPosition;
            this.velocity = velocity;
            this.weight = weight;
        }

        public Vector2 BallPosition
        {
            get => position;
            set
            {
                if (value.Equals(position))
                    return;

                position = value;
            }
        }

        public Vector2 BallNewPosition
        {
            get => newPosition;
            set
            {
                if (value.Equals(newPosition))
                    return;

                newPosition = value;
            }
        }

        public Vector2 Velocity
        {
            get => velocity;
            set
            {
                if (value.Equals(velocity))
                    return;

                velocity = value;
            }
        }

        public int BallSize => size;
        public double BallWeight => weight;

        public float BallPositionX
        {
            get => BallPosition.X;

        }

        public float BallPositionY
        {
            get => BallPosition.Y;

        }


    }
}
