using Data;
using System.Collections;
using System.Numerics;

namespace Logic
{
    public abstract class LogicAbstractAPI : IObservable<LogicAbstractAPI>, IObserver<DataAbstractAPI>
    {
        public abstract int width { get; set; }
        public abstract int height { get; set; }
        public abstract int GetAmount { get; }
        private List<DataAbstractAPI> balls { get; }
        public abstract IDisposable Subscribe(IObserver<LogicAbstractAPI> observer);

        public abstract void Start();
        public abstract void Stop();
        public abstract void CollisionWithWall(DataAbstractAPI ball);
        public abstract void Bounce(DataAbstractAPI ball);
 

        public abstract IList CreateBalls(int count);
        public abstract DataAbstractAPI GetBall(int index);
        public abstract List<List<float>> GetAllBallPositions();

        public static LogicAbstractAPI CreateApi(int width, int height, DataAbstractAPI dataAbstractAPI = default(DataAbstractAPI))
        {
            return new LogicAPI(width, height, dataAbstractAPI);
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(DataAbstractAPI value);
       
    }

    internal class LogicAPI : LogicAbstractAPI
    {
        public override int width { get; set; }
        public override int height { get; set; }
        private List<DataAbstractAPI> balls { get; }
        private readonly List<IObserver<LogicAbstractAPI>>? _observers = [];

        private readonly DataAbstractAPI dataAbstractAPI;
        private readonly Mutex mutex = new Mutex();
        private readonly Random random = new Random();

        public LogicAPI(int width, int height, DataAbstractAPI dataAbstractAPI)
        {
            balls = new List<DataAbstractAPI>();
            this.dataAbstractAPI = dataAbstractAPI;
            this.width = width;
            this.height = height;
        }

        public override int GetAmount { get => balls.Count; }
        public List<DataAbstractAPI> Balls => balls;


        public override void Start()
        {
            for (int i = 0; i < balls.Count; i++)
            {
                GetBall(i).BallCreateMovementTask(30);
            }
        }

        public override void Stop()
        {
            for (int i = 0; i < balls.Count; i++)
            {
                GetBall(i).BallStop();
            }
        }

        public override void CollisionWithWall(DataAbstractAPI ball)
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

        public override void Bounce(DataAbstractAPI ball)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                DataAbstractAPI secondBall = GetBall(i);
                if (balls.IndexOf(ball) == balls.IndexOf(secondBall))
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

        internal bool Collision(DataAbstractAPI a, DataAbstractAPI b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return Distance(a, b) <= (a.BallSize / 2 + b.BallSize / 2);
        }

        internal double Distance(DataAbstractAPI a, DataAbstractAPI b)
        {
            double x1 = a.BallPosition.X + a.BallSize / 2 + a.Velocity.X;
            double y1 = a.BallPosition.Y + a.BallSize / 2 + a.Velocity.Y;
            double x2 = b.BallPosition.X + b.BallSize / 2 + b.Velocity.X;
            double y2 = b.BallPosition.Y + b.BallSize / 2 + b.Velocity.Y;
            return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }


        public override IList CreateBalls(int count)
        {
            if (count > 0)
            {
                int ballsCount = balls.Count;
                for (int i = 0; i < count; i++)
                {
                    mutex.WaitOne();
                    int r = 20;
                    double weight = 30;
                    float x = random.Next(r, width - r);
                    float y = random.Next(r, height - r);
                    Position position = new Position((float)x, (float)y);
                    Vector2 velocity = new Vector2(5, 5);
                    DataAbstractAPI ball = DataAbstractAPI.CreateApi(r, position, velocity, weight);

                    balls.Add(ball);
                    ball.Subscribe(this);
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

        public override DataAbstractAPI GetBall(int index)
        {
            return balls[index];
        }

        public override List<List<float>> GetAllBallPositions()
        {
            List<List<float>> PositionList = new List<List<float>>();

            foreach (DataAbstractAPI ball in balls)
            {
                List<float> ballPosition = new List<float>()
                {
                    ball.BallPosition.X,
                    ball.BallPosition.Y
                 };
                PositionList.Add(ballPosition);
            }
            return PositionList;
        }

        public override IDisposable Subscribe(IObserver<LogicAbstractAPI> observer)
        {
            _observers.Add(observer);
            return new SubscriptionManager(_observers, observer);
        }

        private void NotifyObservers(LogicAbstractAPI ball)
        {
            foreach (var observer in _observers) observer.OnNext(ball);
        }

        public override void OnNext(DataAbstractAPI ball)
        {
            mutex.WaitOne();
            CollisionWithWall(ball);
            Bounce(ball);
            mutex.ReleaseMutex();

            NotifyObservers(this);
        }



        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }

    internal class SubscriptionManager(ICollection<IObserver<LogicAbstractAPI>> observers, IObserver<LogicAbstractAPI> observer) : IDisposable
    {
        public void Dispose()
        {
            if (observer != null && observers.Contains(observer)) observers.Remove(observer);
        }
    }


}