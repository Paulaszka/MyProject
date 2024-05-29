using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        int BallId { get; }
        int BallSize { get; }
        double BallWeight { get; }

        Position BallPosition { get; set; }
        Vector2 Velocity { get; set; }

        void BallCreateMovementTask(int interval);
        void BallStop();
    }

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

    internal class Ball : IBall
    {
        private readonly int size;
        private readonly int id;
        private readonly double weight;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;
        public event PropertyChangedEventHandler PropertyChanged;
        Mutex mutex = new Mutex();

        public Ball(int id, int size, Position position, Vector2 velocity, double weight)
        {
            this.id = id;
            this.size = size;
            BallPosition = position;
            Velocity = velocity;
            this.weight = weight;
        }

        public int BallId { get => id; }
        public int BallSize { get => size; }
        public double BallWeight { get => weight; }

        public Position BallPosition { get; set; }
        public Vector2 Velocity { get; set; }

        private void BallMove()
        {
            //mutex.WaitOne();
            BallPosition.SetPosition(BallPosition.X + Velocity.X, BallPosition.Y + Velocity.Y);
            RaisePropertyChanged(nameof(BallPosition));
            //mutex.ReleaseMutex();
        }

        public void BallCreateMovementTask(int interval)
        {
            stop = false;
            task = Run(interval);
        }

        public void BallStop()
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
                
        internal void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}