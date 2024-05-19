using System.Collections;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract int GetAmount { get; }

        public abstract IList CreateBallsList(int count);

        public abstract int Width { get; }
        public abstract int Height { get; }

        public abstract IBall GetBall(int index);

        public static DataAbstractAPI CreateApi(int width, int height)
        {
            return new DataAPI(width, height);
        }
    }

    internal class DataAPI : DataAbstractAPI
    {
        private ObservableCollection<IBall> balls { get; }
        private readonly Mutex mutex = new Mutex();
        private readonly Random random = new Random();

        public override int Width { get; }
        public override int Height { get; }

        public DataAPI(int width, int height)
        {
            balls = new ObservableCollection<IBall>();
            this.Width = width;
            this.Height = height;
        }

        public override IList CreateBallsList(int count)
        {
            if (count > 0)
            {
                int ballsCount = balls.Count;
                for (int i = 0; i < count; i++)
                {
                    mutex.WaitOne();
                    int r = 20;
                    int pom = random.Next(20, 40);
                    double weight = pom;
                    float x = random.Next(r, Width - r);
                    float y = random.Next(r, Height - r);
                    Position position = new Position((float)x, (float)y);
                    Vector2 velocity = new Vector2(5, 5);
                    Ball ball = new Ball(i + 1 + ballsCount, r, position, velocity, weight);

                    balls.Add(ball);
                    mutex.ReleaseMutex();
                }
            }
            if (count < 0)
            {
                for (int i = count; i < 0; i++)
                {
                    if (balls.Count > 0)
                    {
                        mutex.WaitOne();
                        balls.Remove(balls[balls.Count - 1]);
                        mutex.ReleaseMutex();
                    };
                }
            }
            return balls;
        }

        public ObservableCollection<IBall> Balls => balls;
        public override int GetAmount { get => balls.Count; }

        public override IBall GetBall(int index)
        {
            return balls[index];
        }
    }
}