using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BallModel : IBall
    {
        private readonly int size;
        private readonly int id;
        private readonly double weight;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;
        public event PropertyChangedEventHandler PropertyChanged;
        Mutex mutex = new Mutex();

        public BallModel(int id, int size, Position position, Vector2 velocity, double weight)
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
