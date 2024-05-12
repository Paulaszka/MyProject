using Data;
using Logic;
using System.Collections;


namespace Model
{
    public abstract class ModelAbstractAPI
    {
        public abstract int width { get; }
        public abstract int height { get; }
        public abstract void StartMoving();
        public abstract IList Start(int ballVal);
        public abstract void Stop();

        public static ModelAbstractAPI CreateApi(int Width, int Height, LogicAbstractAPI logicAbstractAPI = default(LogicAbstractAPI))
        {
            if (logicAbstractAPI == null)
            {
                logicAbstractAPI = LogicAbstractAPI.CreateApi(Width, Height);
            }
            return new ModelAPI(Width, Height, logicAbstractAPI);
        }
    }

    internal class ModelAPI : ModelAbstractAPI
    {
        public override int width { get; }
        public override int height { get; }
        private readonly LogicAbstractAPI logicAbstractAPI;

        public ModelAPI(int Width, int Height, LogicAbstractAPI logicAbstractAPI)
        {
            width = Width;
            height = Height;
            this.logicAbstractAPI = logicAbstractAPI;
        }

        public override void StartMoving()
        {
            logicAbstractAPI.Start();
        }

        public override void Stop()
        {
            logicAbstractAPI.Stop();
        }

        public override IList Start(int ballVal) => logicAbstractAPI.CreateBalls(ballVal);
    }
}
