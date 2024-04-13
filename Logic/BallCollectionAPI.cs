using Data;

namespace Logic
{
    public abstract class BallCollectionAPI
    {
        public abstract void CreateBallCollection(int quantity);
        public abstract List<LogicAPI> GetBallCollection();
        public static BallCollectionAPI CreateObjCollectionLogic(DataAPI data = default(DataAPI))
        {
            return new BallCollection();
        }
    }
}
