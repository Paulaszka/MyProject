using Data;

namespace Logic
{
    public abstract class BallCollectionAPI
    {
        public abstract void CreateBallCollection(int size);

        public abstract List<LogicAPI> GetBallCollection();

        public static BallCollectionAPI CreateObjCollectionLogic(DataAPI? data = default)
        {
            return new BallCollection();
        }
    }
}
