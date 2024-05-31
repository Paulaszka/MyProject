using System.Numerics;

namespace Model
{
    public abstract class BallModelAPI
    {
        public abstract Vector2 Velocity { get; set; }
        public abstract float PositionX { get; set; }
        public abstract float PositionY { get; set; }

        public static BallModelAPI CreateApi(float positionX, float positionY, Vector2 velocity)
        {
            return new BallModel(positionX, positionY, velocity);
        }
    }
}
