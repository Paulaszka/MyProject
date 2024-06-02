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
        public abstract List<DataAbstractAPI> balls { get; }

        public abstract void Start();
        public abstract void Stop();
        public abstract void CollisionWithWall(DataAbstractAPI ball);
        public abstract void Bounce(DataAbstractAPI ball);
 
        public abstract IList CreateBalls(int count);
        public abstract DataAbstractAPI GetBall(int index);
        public abstract List<List<float>> GetAllBallPositions();

        public abstract IDisposable Subscribe(IObserver<LogicAbstractAPI> observer);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(DataAbstractAPI value);

        public static LogicAbstractAPI CreateApi(int width, int height)
        {
            return new LogicAPI(width, height);
        }
    }

    internal class LogicAPI : LogicAbstractAPI
    {
        public override int width { get; set; }
        public override int height { get; set; }
        public override List<DataAbstractAPI> balls { get; }
        private readonly object _lock = new();
        private readonly Random random = new Random();
        private readonly List<IObserver<LogicAbstractAPI>>? _observers;
        private IDisposable? _subscriptionToken;
        
        public LogicAPI(int width, int height)
        {
            lock (_lock)
            {
                balls = new();
            }
            this.width = width;
            this.height = height;
            _observers = [];
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
            float diameter = 20;
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
                    double m1 = 30;
                    double m2 = 30;
                    Vector2 v1 = ball.Velocity;
                    Vector2 v2 = secondBall.Velocity;

                    double u1x = (m1 - m2) * v1.X / (m1 + m2) + (2 * m2) * v2.X / (m1 + m2);
                    double u1y = (m1 - m2) * v1.Y / (m1 + m2) + (2 * m2) * v2.Y / (m1 + m2);
                    double u2x = 2 * m1 * v1.X / (m1 + m2) + (m2 - m1) * v2.X / (m1 + m2);
                    double u2y = 2 * m1 * v1.Y / (m1 + m2) + (m2 - m1) * v2.Y / (m1 + m2);

                    ball.Velocity = new Vector2((float)u1x, (float)u1y);
                    secondBall.Velocity = new Vector2((float)u2x, (float)u2y);
                    ball.Subscribe(this);
                    secondBall.Subscribe(this);
                    return;
                }
            }
        }

        internal bool Collision(DataAbstractAPI a, DataAbstractAPI b)
        {
            int diameter = 20;
            if (a == null || b == null)
            {
                return false;
            }
            return Distance(a, b) <= diameter;
        }

        internal double Distance(DataAbstractAPI a, DataAbstractAPI b)
        {
            double diameter = 20;
            double x1 = a.BallPosition.X + diameter / 2 + a.Velocity.X;
            double y1 = a.BallPosition.Y + diameter / 2 + a.Velocity.Y;
            double x2 = b.BallPosition.X + diameter / 2 + b.Velocity.X;
            double y2 = b.BallPosition.Y + diameter / 2 + b.Velocity.Y;
            return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }

        public override IList CreateBalls(int count)
        {
            lock (_lock)
            {
                balls.Clear();
            }           
            
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    lock (_lock)
                    {
                        int diameter = 20;
                        float x = random.Next(diameter, width - diameter);
                        float y = random.Next(diameter, height - diameter);
                        Position position = new((float)x, (float)y);
                        Vector2 velocity = new(5, 5);
                        DataAbstractAPI ball = DataAbstractAPI.CreateApi(balls.Count, position, velocity);

                        balls.Add(ball);
                        balls[i].Subscribe(this);
                    }
                }
            }
            else
            {
                
            }
            return balls;
        }

        public override DataAbstractAPI GetBall(int index)
        {
            return balls[index];
        }

        public override List<List<float>> GetAllBallPositions()
        {
            List<List<float>> PositionList = [];
            lock (_lock)
            {
                foreach (DataAbstractAPI ball in balls)
                {
                    List<float> ballPosition = new()
                    {
                        ball.BallPosition.X,
                        ball.BallPosition.Y
                    };
                    PositionList.Add(ballPosition);
                }
            }
            return PositionList;
        }

        public override IDisposable Subscribe(IObserver<LogicAbstractAPI> observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            return new SubscriptionManager(_observers, observer);
        }

        private void NotifyObservers(LogicAbstractAPI ball)
        {
            if (_observers != null)
            {
                foreach (var observer in _observers) observer.OnNext(ball);
            }
        }

        public override void OnNext(DataAbstractAPI ball)
        {
            lock (_lock)
            {
                CollisionWithWall(ball);
                Bounce(ball);
            }
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

        public void Subscribe(IObservable<DataAbstractAPI> provider)
        {
            if (provider != null) _subscriptionToken = provider.Subscribe(this);
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