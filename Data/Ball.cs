using System.Diagnostics;
using System.Numerics;

namespace Data
{
    public interface IPosition
    {
        float X { get; set; }
        float Y { get; set; }

        void SetPosition(float x, float y);
    }

    public class Position : IPosition
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
        private Position position;
        private Vector2 velocity;

        public Ball(Position _position, Vector2 _velocity)
        {
            position = _position;
            velocity = _velocity;
        }

        public override Position BallPosition { 
            get => position;
            set
             {
                lock (positionLock)
                {
                    position = value;
                }
            }
        }

        public override Vector2 Velocity { 
            get => velocity;
            set
            {
                lock (velocityLock)
                {
                    velocity = value;
                }
            }
        }

        private void BallMove()
        {
            BallPosition.SetPosition(BallPosition.X + Velocity.X, BallPosition.Y + Velocity.Y);
            NotifyObservers(this);
        }

        public override void BallCreateMovementTask(int interval)
        {
            stop = false;
            task = Run(interval);
        }

        public override void BallStop()
        {
            stop = true;
        }

        private async Task Run(int interval)
        {
            while (!stop)
            {
                stopwatch.Reset();
                stopwatch.Start();
                if (!stop)
                {
                    BallMove();
                }
                stopwatch.Stop();
                await Task.Delay((int)(interval - stopwatch.ElapsedMilliseconds));
            }
        }

        public override IDisposable Subscribe(IObserver<DataAbstractAPI> observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            return new SubscriptionManager(_observers, observer);
        }

        private void NotifyObservers(DataAbstractAPI ball)
        {
            foreach (var observer in _observers) observer.OnNext(ball);
        }
    }

    internal class SubscriptionManager(ICollection<IObserver<DataAbstractAPI>> observers, IObserver<DataAbstractAPI> observer) : IDisposable
    {
        public void Dispose()
        {
            if (observer != null && observers.Contains(observer)) observers.Remove(observer);
        }
    }
}