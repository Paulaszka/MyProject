namespace Logic
{
    internal class BallCollection : BallCollectionAPI
    {
        List<LogicAPI>? ballCollection;

        public override void CreateBallCollection(int size)
        {
            ballCollection = new List<LogicAPI>();

            for (int i = 0; i < size; i++)
            {
                LogicAPI ball = LogicAPI.CreateObjLogic();
                ballCollection.Add(ball.CreateBall());
            }
        }

        public override List<LogicAPI> GetBallCollection()
        {
            return ballCollection;
        }
    }
}
