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
        float BallPositionX { get; set; }
        float BallPositionY { get; set; }
        Vector2 Velocity { get; set; }

        void BallMove();
        void BallCreateMovementTask(int interval);
        void BallStop();
    }

    internal class Ball : IBall
    {
        private Vector2 velocity;
        private readonly int size;
        private readonly int id;
        private float x;
        private float y;
        private readonly double weight;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;

        public Ball(int id, int size, float x, float y, Vector2 velocity,  double weight)

        {
            this.id = id;
            this.size = size;
            this.x = x;
            this.y = y;
            this.velocity = velocity;
            this.weight = weight;
        }

        public int BallId { get => id; }
        public int BallSize { get => size; }

        public float BallPositionX
        {
            get => x;
            set
            {
                if (value.Equals(x))
                {
                    return;
                }

                x = value;
                RaisePropertyChanged();
            }
        }

        public float BallPositionY
        {
            get => y;
            set
            {
                if (value.Equals(y))
                {
                    return;
                }

                y = value;
                RaisePropertyChanged();
            }
        }
        public void BallMove()
        {
            BallPositionX += velocity.X;
            BallPositionY += velocity.Y;
        }

        public double BallWeight { get => weight; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BallCreateMovementTask(int interval)
        {
            stop = false;
            task = Run(interval);
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

        public void BallStop()
        {
            stop = true;
        }

        public Vector2 Velocity
        {
            get => velocity;
            set
            {
                if (value.Equals(velocity))
                    return;

                velocity = value;
            }
        }
    }
}
