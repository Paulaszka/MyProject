﻿using System.ComponentModel;
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

        float X { get; }
        float Y { get; }

        void BallMove();

        void BallCreateMovementTask(int interval);

        void BallStop();
    }

    public interface IPosition : INotifyPropertyChanged
    {
        float X { get; set; }
        float Y { get; set; }

        void SetPosition(float x, float y);
    }

    public class Position : ObservableObject, IPosition
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
            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));
        }
    }

    internal class Ball : IBall
    {
        private Position position;
        private Vector2 velocity;
        private readonly int size;
        private readonly int id;
        private float x;
        private float y;
        private readonly double weight;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private Task task;
        private bool stop = false;

        public Ball(int id, int size, Position position, Vector2 velocity, double weight)
        {
            this.id = id;
            this.size = size;
            this.position = position;
            this.velocity = velocity;
            this.weight = weight;
        }

        public int BallId { get => id; }
        public int BallSize { get => size; }

        public Position BallPosition
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

        public float X
        {
            get => BallPosition.X;
        }

        public float Y
        {
            get => BallPosition.Y;
        }

        public float BallPositionX
        {
            get => BallPosition.X;
        }

        public float BallPositionY
        {
            get => BallPosition.Y;
        }

        public void BallMove()
        {
            BallPosition.SetPosition(BallPosition.X + velocity.X, BallPosition.Y + velocity.Y);
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