using System.Numerics;

namespace Data
{
    public abstract class BallLoggerAPI
    {
        public abstract int Id { get; set; }
        public abstract Position BallPosition { get; set; }
        public abstract DateTime DateTime { get; set; }

        public static BallLoggerAPI CreateBallLogger(Position _position, DateTime _datetime)
        {
            return new BallLogger(_position, _datetime);
        }
    }

    internal class BallLogger : BallLoggerAPI
    {
        private Position position;
        private DateTime datetime;
        private int id;
        private List<int> IDs = new();
        private static readonly object idLock = new();
        private readonly object positionLock = new();
        private readonly object velocityLock = new();

        public BallLogger(Position _position, DateTime _datetime)
        {
            position = _position;
            datetime = _datetime;
            
            lock (idLock)
            {
                int counter = 0;
                while (true)
                {
                    if (!IDs.Contains(counter))
                    {
                        id = counter;
                        IDs.Add(counter);
                        break;
                    }
                    counter++;
                }
            }
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

        public override DateTime DateTime
        {
            get => datetime;
            set
            {
                lock (positionLock)
                {
                    datetime = value;
                }
            }
        }

        public override int Id
        {
            get => id;
            set
            {
                lock (positionLock)
                {
                    id = value;
                }
            }
        }


    }
}
