using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class BallLoggerAPI
    {
        public abstract Position BallPosition { get; set; }
        public abstract Vector2 Velocity { get; set; }

        public static BallLoggerAPI CreateBallLogger(Position _position, Vector2 _velocity, DateTime _datetime)
        {
            return new BallLogger(_position, _velocity, _datetime);
        }
    }

    internal class BallLogger : BallLoggerAPI
    {
        private Position position;
        private Vector2 velocity;
        private DateTime datetime;
        private readonly object positionLock = new();
        private readonly object velocityLock = new();

        public BallLogger(Position _position, Vector2 _velocity, DateTime _datetime)
        {
            position = _position;
            velocity = _velocity;
            datetime = _datetime;
        }

        public override Position BallPosition
        {
            get => position;
            set
            {
                lock (positionLock)
                {
                    position = value;
                }
            }
        }

        public override Vector2 Velocity
        {
            get => velocity;
            set
            {
                lock (velocityLock)
                {
                    velocity = value;
                }
            }
        }
    }
}
