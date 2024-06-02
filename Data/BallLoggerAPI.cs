namespace Data
{
    public abstract class BallLoggerAPI
    {
        public abstract int Id { get; set; }
        public abstract Position BallPosition { get; set; }
        public abstract DateTime DateTime { get; set; }

        public static BallLoggerAPI CreateBallLogger(int _id, Position _position, DateTime _datetime)
        {
            return new BallLogger(_id, _position, _datetime);
        }
    }

    internal class BallLogger : BallLoggerAPI
    {
        private Position position;
        private DateTime datetime;
        private int id;
        private static readonly object idLock = new();
        private readonly object positionLock = new();

        public BallLogger(int _id, Position _position, DateTime _datetime)
        {
            id = _id;
            position = _position;
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

        public override DateTime DateTime
        {
            get => datetime;
            set
            {
                datetime = value;
            }
        }

        public override int Id
        {
            get => id;
            set
            {
                id = value;
            }
        }
    }
}
