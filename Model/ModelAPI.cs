using Logic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Model
{
    public abstract class ModelAbstractAPI { 
        public abstract int width { get; }
        public abstract int height { get; }

        public abstract IList Start(int ballVal);
        
        public abstract void StartMoving();
        public abstract void Stop();

        public static ModelAbstractAPI CreateApi(int Width, int Height, LogicAbstractAPI? logicAbstractAPI = default)
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
        private readonly ObservableCollection<BallModelAPI> _balls = [];
        List<List<float>> ballPositions = [];
        private IDisposable? _subscriptionToken;

        public ModelAPI(int Width, int Height, LogicAbstractAPI logicAbstractAPI)
        {
            width = Width;
            height = Height;
            this.logicAbstractAPI = logicAbstractAPI;
            Subscribe(logicAbstractAPI);
        }

        public override IList Start(int ballVal)
        {
            _balls.Clear();
            logicAbstractAPI.CreateBalls(ballVal);
            ballPositions = logicAbstractAPI.GetAllBallPositions();

            for (var i = 0; i < ballPositions.Count; i++)
            {
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
            ballPositions = value.GetAllBallPositions();
            for (var i = 0; i < ballPositions.Count; i++)
            {
                if (_balls[i].PositionX != ballPositions[i][0])
                {
                    _balls[i].PositionX = ballPositions[i][0];
                }
                if (_balls[i].PositionY != ballPositions[i][1])
                {
                    _balls[i].PositionY = ballPositions[i][1]; 
                }
            }
        }

        public void Subscribe(IObservable<LogicAbstractAPI> provider)
        {
            if (provider != null) _subscriptionToken = provider.Subscribe(this);
        }

        public void Unsubscribe()
        {
            _subscriptionToken?.Dispose();
        }
    }
}
