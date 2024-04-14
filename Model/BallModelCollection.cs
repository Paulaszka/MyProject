using Logic;

namespace Model
{
    public class BallModelCollection
    {
        List<BallModel> ballModelCollection;
        private LogicAPI logic;
        private BallCollectionAPI logicCollection;

        public void CreateBallModelCollection(int size)
        {
            logic = LogicAPI.CreateObjLogic();
            logicCollection = BallCollectionAPI.CreateObjCollectionLogic();
            logicCollection.CreateBallCollection(size);

            List<LogicAPI> ballCollection = logicCollection.GetBallCollection();
            ballModelCollection = new List<BallModel>();

            foreach (LogicAPI x in ballCollection)
            {
                BallModel ballModel = new BallModel();
                this.ballModelCollection.Add(ballModel);
                ballModel.ModelXPosition = logic.getBallPosition().X;
                ballModel.ModelYPosition = logic.getBallPosition().Y;
            }
        }

        public List<BallModel> GetBallModelCollection()
        {
            return ballModelCollection;
        }
    }
}
