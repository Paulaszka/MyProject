using System.Diagnostics;
using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI : IObservable<DataAbstractAPI>
    {
        public abstract int BallId { get; set; }
        public abstract IPosition BallPosition { get; set; }
        public abstract Vector2 Velocity { get; set; }

        public abstract void BallCreateMovementTask();
        public abstract void BallStop();
        public abstract IDisposable Subscribe(IObserver<DataAbstractAPI> observer);

        public static DataAbstractAPI CreateApi(int id, IPosition position, Vector2 velocity)
        {
            return new Ball(id, position, velocity);
        }
    }

    public interface IPosition
    {
        float X { get; set; }
        float Y { get; set; }

        public static IPosition CreatePosition(float x, float y)
        {
            return new Position(x, y);
        }

        void SetPosition(float x, float y);
    }

    internal class Position : IPosition
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    internal class Ball : DataAbstractAPI, IObservable<DataAbstractAPI>
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;
        private readonly List<IObserver<DataAbstractAPI>> _observers = [];
        private readonly object positionLock = new();
        private readonly object velocityLock = new();
        private readonly object loggerLock = new();
        private int id;
        private IPosition position;
        private Vector2 velocity;
        private Logger loggerAPI;

        public Ball(int _id, IPosition _position, Vector2 _velocity)
        {
            id = _id;
            position = _position;
            velocity = _velocity;
            loggerAPI = Logger.GetInstance();
        }

        public override IPosition BallPosition
        {
            get
            {
                lock (positionLock)
                {
                    return position;
                }
            }
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
            get
            {
                lock (velocityLock)
                {
                    return velocity;
                }
            }
            set
            {
                lock (velocityLock)
                {
                    velocity = value;
                }
            }
        }

        public override int BallId
        {
            get => id;
            set
            {
                id = value;
            }
        }

        public override void BallCreateMovementTask()
        {
            stop = false;
            task = Run();
        }

        public override void BallStop()
        {
            stop = true;
        }

        private async Task Run()
        {
            float lastUpdateTime = 0f;
            stopwatch.Start();
            while (true)
            {
                float currentTime = (float)stopwatch.Elapsed.TotalSeconds;
                float delta = currentTime - lastUpdateTime;
                const float timeOfTravel = 1f / 60f;

                if (delta >= timeOfTravel)
                {
                    lastUpdateTime = currentTime;
                    BallPosition.SetPosition(BallPosition.X + Velocity.X, BallPosition.Y + Velocity.Y);
                    NotifyObservers(this);
                    loggerAPI.AddBallToQueue(this, DateTime.Now);
                }

                if (stop)
                {
                    stopwatch.Stop();
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(timeOfTravel));
            } 
        }

        public override IDisposable Subscribe(IObserver<DataAbstractAPI> observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            return new SubscriptionKey(_observers, observer);
        }

        private void NotifyObservers(DataAbstractAPI ball)
        {
            foreach (var observer in _observers) observer.OnNext(ball);
        }
    }

    internal class SubscriptionKey(ICollection<IObserver<DataAbstractAPI>> observers, IObserver<DataAbstractAPI> observer) : IDisposable
    {
        public void Dispose()
        {
            if (observer != null && observers.Contains(observer)) observers.Remove(observer);
        }
    }

}