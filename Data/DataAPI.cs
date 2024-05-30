using System.Collections;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI : IObservable<DataAbstractAPI>
    {
        public abstract int BallId { get; }
        public abstract int BallSize { get; }
        public abstract double BallWeight { get; }

        public abstract Position BallPosition { get; set; }
        public abstract Vector2 Velocity { get; set; }

        public abstract void BallCreateMovementTask(int interval);
        public abstract void BallStop();
        public abstract IDisposable Subscribe(IObserver<DataAbstractAPI> observer);

        public static DataAbstractAPI CreateApi(int id, int size, Position position, Vector2 velocity, double weight)
        {
            return new Ball(id, size, position, velocity, weight);
        }
    }

}