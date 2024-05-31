using Data;
using System.Numerics;

namespace Model
{
    public abstract class BallModelAPI
    {
        public abstract int BallSize { get; }
        public abstract double BallWeight { get; }

        //public abstract Vector2 BallPosition { get; set; }
        public abstract Vector2 Velocity { get; set; }
        public abstract float PositionX { get; set; }
        public abstract float PositionY { get; set; }

        public static BallModelAPI CreateApi(int size, float positionX, float positionY, Vector2 velocity, double weight)
        {
            return new BallModel(size, positionX, positionY, velocity, weight);
        }
    }
}
