using System.Drawing;
using System.Numerics;

namespace Data
{
    public abstract class DataAPI
    {
        /*public static DataAPI CreateBall(double x, double y)
        {
            return new Ball(x, y);
        }*/

        public Vector2 BallPosition { get; set; }
        public Vector2 BallNewPosition { get; set; }
        public Vector2 Velocity { get; set; }
        public float BallPositionX { get; set; }
        public float BallPositionY { get; set; }

    }
}
