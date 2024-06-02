using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI : IObservable<DataAbstractAPI>
    {
        public abstract int BallId { get; set; }
        public abstract Position BallPosition { get; set; }
        public abstract Vector2 Velocity { get; set; }

        public abstract void BallCreateMovementTask(int interval);
        public abstract void BallStop();
        public abstract IDisposable Subscribe(IObserver<DataAbstractAPI> observer);

        public static DataAbstractAPI CreateApi(int _id, Position position, Vector2 velocity)
        {
            return new Ball(_id, position, velocity);
        }
    }

}