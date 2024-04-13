﻿

namespace Logic
{
    internal class BallCollection : BallCollectionAPI
    {
        List<LogicAPI> ballCollection;

        public override void CreateBallCollection(int size)
        {
            ballCollection = new List<LogicAPI>();

            for (int i = 0; i < size; i++)
            {
                Logic ball = new Logic();
                ballCollection.Add(ball.CreateBall());
            }
        }

        public override List<LogicAPI> GetBallCollection()
        {
            return ballCollection;
        }
    }
}
