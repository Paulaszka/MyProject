using Data;
using System.Numerics;

namespace Model
{
    public abstract class BallModelAPI
    {
        public abstract int BallSize { get; }
        public abstract double BallWeight { get; }

        public abstract Position BallPosition { get; set; }
        public abstract Vector2 Velocity { get; set; }

        public static BallModelAPI CreateApi(int size, Position position, Vector2 velocity, double weight)
        {
            return new BallModel(size, position, velocity, weight);
        }
    }
}
