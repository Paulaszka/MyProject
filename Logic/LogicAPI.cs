using Data;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public abstract int GetAmount { get; }

        public abstract IList CreateBalls(int count);

        public abstract void Start();

        public abstract void Stop();

        public abstract int width { get; set; }
        public abstract int height { get; set; }

        public abstract IBall GetBall(int index);

        public abstract void CollisionWithWall(IBall ball);

        public abstract void Bounce(IBall ball);

        public abstract void ChangeBallPosition(object sender, PropertyChangedEventArgs args);

        public static LogicAbstractAPI CreateApi(int width, int height, DataAbstractAPI dataAbstractAPI = default(DataAbstractAPI))
        {
            if (dataAbstractAPI == null)
            {
                dataAbstractAPI = DataAbstractAPI.CreateApi(width, height);
            }
            return new LogicAPI(width, height, dataAbstractAPI);
        }
    }

    internal class LogicAPI : LogicAbstractAPI
    {
        private readonly DataAbstractAPI dataAbstractAPI;
        private readonly Mutex mutex = new Mutex();

        public LogicAPI(int width, int height, DataAbstractAPI dataAbstractAPI)
        {
            this.dataAbstractAPI = dataAbstractAPI;
            this.width = width;
            this.height = height;
        }

        public override int width { get; set; }
        public override int height { get; set; }

        public override void Start()
        {
            for (int i = 0; i < dataAbstractAPI.GetAmount; i++)
            {
                dataAbstractAPI.GetBall(i).BallCreateMovementTask(30);
            }
        }

        public override void Stop()
        {
            for (int i = 0; i < dataAbstractAPI.GetAmount; i++)
            {
                dataAbstractAPI.GetBall(i).BallStop();
            }
        }

        public override void CollisionWithWall(IBall ball)
        {
            float diameter = ball.BallSize;
            float right = width - diameter;
            float down = height - diameter;

            if (ball.BallPosition.X <= 0)
            {
                ball.BallPosition.SetPosition(-ball.BallPosition.X, ball.BallPosition.Y);
                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
            }
            else if (ball.BallPosition.X >= right)
            {
                ball.BallPosition.SetPosition(right - (ball.BallPosition.X - right), ball.BallPosition.Y);
                ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
            }
            if (ball.BallPosition.Y <= 0)
            {
                ball.BallPosition.SetPosition(ball.BallPosition.X, -ball.BallPosition.Y);
                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
            }
            else if (ball.BallPosition.Y >= down)
            {
                ball.BallPosition.SetPosition(ball.BallPosition.X, down - (ball.BallPosition.Y - down));
                ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
            }
        }

        public override void Bounce(IBall ball)
        {
            for (int i = 0; i < dataAbstractAPI.GetAmount; i++)
            {
                IBall secondBall = dataAbstractAPI.GetBall(i);
                if (ball.BallId == secondBall.BallId)
                {
                    continue;
                }
                if (Collision(ball, secondBall))
                {
                    double m1 = ball.BallWeight;
                    double m2 = secondBall.BallWeight;
                    Vector2 v1 = ball.Velocity;
                    Vector2 v2 = secondBall.Velocity;

                    double u1x = (m1 - m2) * v1.X / (m1 + m2) + (2 * m2) * v2.X / (m1 + m2);
                    double u1y = (m1 - m2) * v1.Y / (m1 + m2) + (2 * m2) * v2.Y / (m1 + m2);
                    double u2x = 2 * m1 * v1.X / (m1 + m2) + (m2 - m1) * v2.X / (m1 + m2);
                    double u2y = 2 * m1 * v1.Y / (m1 + m2) + (m2 - m1) * v2.Y / (m1 + m2);

                    ball.Velocity = new Vector2((float)u1x, (float)u1y);
                    secondBall.Velocity = new Vector2((float)u2x, (float)u2y);
                    return;
                }
            }
        }

        internal bool Collision(IBall a, IBall b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return Distance(a, b) <= (a.BallSize / 2 + b.BallSize / 2);
        }

        internal double Distance(IBall a, IBall b)
        {
            double x1 = a.BallPosition.X + a.BallSize / 2 + a.Velocity.X;
            double y1 = a.BallPosition.Y + a.BallSize / 2 + a.Velocity.Y;
            double x2 = b.BallPosition.X + b.BallSize / 2 + b.Velocity.X;
            double y2 = b.BallPosition.Y + b.BallSize / 2 + b.Velocity.Y;
            return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }

        public override IList CreateBalls(int count)
        {
            int previousCount = dataAbstractAPI.GetAmount;
            IList temp = dataAbstractAPI.CreateBallsList(count);
            for (int i = 0; i < dataAbstractAPI.GetAmount - previousCount; i++)
            {
                dataAbstractAPI.GetBall(previousCount + i).PropertyChanged += ChangeBallPosition;
            }
            return temp;
        }

        public override void ChangeBallPosition(object sender, PropertyChangedEventArgs args)
        {
            IBall ball = (IBall)sender;
            mutex.WaitOne();
            CollisionWithWall(ball);
            Bounce(ball);
            mutex.ReleaseMutex();
        }

        public override IBall GetBall(int index)
        {
            return dataAbstractAPI.GetBall(index);
        }

        public override int GetAmount { get => dataAbstractAPI.GetAmount; }
    }
}