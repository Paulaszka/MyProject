using Logic;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace Model
{
    public abstract class ModelAbstractAPI { 
        public abstract int width { get; }
        public abstract int height { get; }

        public abstract IList Start(int ballVal);
        
        public abstract void StartMoving();
        public abstract void Stop();
        public abstract IDisposable Subscribe(IObserver<ModelAbstractAPI> observer);

        public static ModelAbstractAPI CreateApi(int Width, int Height, LogicAbstractAPI logicAbstractAPI = default(LogicAbstractAPI))
        {
            if (logicAbstractAPI == null)
            {
                logicAbstractAPI = LogicAbstractAPI.CreateApi(Width, Height);
            }
            return new ModelAPI(Width, Height, logicAbstractAPI);
        }

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(LogicAbstractAPI value);
    }

    internal class ModelAPI : ModelAbstractAPI, IObserver<LogicAbstractAPI>
    {
        public override int width { get; }
        public override int height { get; }
        private readonly LogicAbstractAPI logicAbstractAPI;
        private readonly List<IObserver<ModelAbstractAPI>>? _observers = [];
        private readonly ObservableCollection<BallModelAPI> _balls = [];
        List<List<float>> ballPositions;

        public ModelAPI(int Width, int Height, LogicAbstractAPI logicAbstractAPI)
        {
            width = Width;
            height = Height;
            this.logicAbstractAPI = logicAbstractAPI;
            logicAbstractAPI.Subscribe(this);
        }

        public override IList Start(int ballVal)
        {
            logicAbstractAPI.CreateBalls(ballVal);
            ballPositions = logicAbstractAPI.GetAllBallPositions();

            for (var i = 0; i < ballPositions.Count; i++)
            {
                //Vector2 position = new(ballPositions[i][0], ballPositions[i][1]);
                Vector2 vector = new (5, 5);
                BallModelAPI ball = BallModelAPI.CreateApi(20, ballPositions[i][0], ballPositions[i][1], vector, 30);
                _balls.Add(ball);
            }
            return _balls;
        }

        public override void StartMoving()
        {
            logicAbstractAPI.Start();
        }

        public override void Stop()
        {
            logicAbstractAPI.Stop();
        }

        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnNext(LogicAbstractAPI value)
        {
            Debug.WriteLine("dupa");
            for (var i = 0; i < ballPositions.Count; i++)
            {
                if (_balls[i].PositionX != ballPositions[i][0])
                {
                    _balls[i].PositionX = ballPositions[i][0];
                }
                if (_balls[i].PositionY!= ballPositions[i][1])
                {
                    _balls[i].PositionY = ballPositions[i][1];
                    
                }
            }
        }
        
        public override IDisposable Subscribe(IObserver<ModelAbstractAPI> observer)
        {
            _observers.Add(observer);
            return new SubscriptionManager(_observers, observer);
        }

    }

    internal class SubscriptionManager(ICollection<IObserver<ModelAbstractAPI>> observers, IObserver<ModelAbstractAPI> observer) : IDisposable
    {
        public void Dispose()
        {
            if (observer != null && observers.Contains(observer)) observers.Remove(observer);
        }
    }
}
