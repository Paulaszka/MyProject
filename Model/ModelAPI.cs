using Logic;
using System;
using System.Collections;
using System.Diagnostics;

namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<LogicAbstractAPI>
    {
        public abstract int width { get; }
        public abstract int height { get; }

        public abstract IList Start(int ballVal);
        

        public abstract void StartMoving();
        public abstract void Stop();

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

    internal class ModelAPI : ModelAbstractAPI
    {
        public override int width { get; }
        public override int height { get; }
        private readonly LogicAbstractAPI logicAbstractAPI;
        private readonly List<IObserver<ModelAbstractAPI>>? _observers = [];

        public ModelAPI(int Width, int Height, LogicAbstractAPI logicAbstractAPI)
        {
            width = Width;
            height = Height;
            this.logicAbstractAPI = logicAbstractAPI;
            logicAbstractAPI.Subscribe(this);
        }

        public override IList Start(int ballVal) => logicAbstractAPI.CreateBalls(ballVal);

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
            
        }


    }

    
}
