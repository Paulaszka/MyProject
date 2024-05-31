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
        private readonly int size;
        private readonly double weight;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;
        Mutex mutex = new Mutex();
        private readonly List<IObserver<DataAbstractAPI>> _observers = [];

        public Ball(int size, Position position, Vector2 velocity, double weight)
        {
            this.size = size;
            BallPosition = position;
            Velocity = velocity;
            this.weight = weight;
        }

        public override int BallSize { get => size; }
        public override double BallWeight { get => weight; }

        public override Position BallPosition { get; set; }
        public override Vector2 Velocity { get; set; }

        private void BallMove()
        {
            //mutex.WaitOne();
            BallPosition.SetPosition(BallPosition.X + Velocity.X, BallPosition.Y + Velocity.Y);
            NotifyObservers(this);
            //mutex.ReleaseMutex();
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