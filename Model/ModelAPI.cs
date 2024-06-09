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

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(LogicAbstractAPI value);

        public static ModelAbstractAPI CreateApi(int Width, int Height, LogicAbstractAPI? logicAbstractAPI = default)
        {
            if (logicAbstractAPI == null)
            {
                logicAbstractAPI = LogicAbstractAPI.CreateApi(Width, Height);
            }
            return new ModelAPI(Width, Height, logicAbstractAPI);
        }
    }

    internal class ModelAPI : ModelAbstractAPI, IObserver<LogicAbstractAPI>
    {
        public override int width { get; }
        public override int height { get; }
        private readonly object _lock = new();
        private readonly LogicAbstractAPI logicAbstractAPI;
        private readonly ObservableCollection<BallModelAPI> modelBallsCollection = [];
        List<List<float>> ballPositions = [];
        private IDisposable? subscriptionKey;

        public ModelAPI(int Width, int Height, LogicAbstractAPI logicAbstractAPI)
        {
            width = Width;
            height = Height;
            this.logicAbstractAPI = logicAbstractAPI;
            Subscribe(logicAbstractAPI);
        }

        public override IList Start(int ballVal)
        {
            lock (_lock)
            {
                modelBallsCollection.Clear();
            }
            logicAbstractAPI.CreateBalls(ballVal);
            ballPositions = logicAbstractAPI.GetAllBallPositions();

            for (int i = 0; i < ballPositions.Count; i++)
            {
                lock (_lock)
                {
                    Vector2 vector = new(5,5);
                    BallModelAPI ball = BallModelAPI.CreateApi(ballPositions[i][0], ballPositions[i][1], vector);
                    modelBallsCollection.Add(ball);
                }
            }
            return modelBallsCollection;
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
            lock (_lock)
            {
                ballPositions = value.GetAllBallPositions();
                for (int i = 0; i < ballPositions.Count; i++)
                {
                    if (modelBallsCollection[i].PositionX != ballPositions[i][0])
                    {
                        modelBallsCollection[i].PositionX = ballPositions[i][0];
                    }
                    if (modelBallsCollection[i].PositionY != ballPositions[i][1])
                    {
                        modelBallsCollection[i].PositionY = ballPositions[i][1]; 
                    }
                }
            }
        }

        public void Subscribe(IObservable<LogicAbstractAPI> provider)
        {
            if (provider != null) subscriptionKey = provider.Subscribe(this);
        }        
    }
}
