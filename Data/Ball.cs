using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        int BallId { get; }
        int BallSize { get; }
        double BallWeight { get; }

        Vector2 BallPosition { get; set; }
        Vector2 BallNewPosition { get; set; }
        Vector2 Velocity { get; set; }

        void ballMove();
        void ballCreateMovementTask(int interval);
        void ballStop();
    }

    internal class Ball : IBall
    {
        private Vector2 position;
        private Vector2 newPosition;
        private Vector2 velocity;
        private readonly int id;
        private readonly int size;
        private readonly double weight;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;

        public Ball(int id, int size, Vector2 position, Vector2 newPosition, Vector2 velocity, double weight)
        {
            this.id = id;
            this.size = size;
            this.position = position;
            this.newPosition = newPosition;
            this.velocity = velocity;
            this.weight = weight;
        }

        public int BallId => id;
        public int BallSize => size;
        public double BallWeight => weight;

        public Vector2 BallPosition
        {
            get => position;
            set
            {
                if (value.Equals(position))
                    return;

                position = value;
                RaisePropertyChanged();
            }
        }

        public Vector2 BallNewPosition
        {
            get => newPosition;
            set
            {
                if (value.Equals(newPosition))
                    return;

                newPosition = value;
            }
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

        public void ballMove()
        {
            BallPosition += Velocity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ballCreateMovementTask(int interval)
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
                    ballMove();

                stopwatch.Stop();
                await Task.Delay((int)(interval - stopwatch.ElapsedMilliseconds));
            }
        }

        public void ballStop()
        {
            stop = true;
        }

        public float BallPositionX
        {
            get => BallPosition.X;

        }

        public float BallPositionY
        {
            get => BallPosition.Y;

        }
    }
}