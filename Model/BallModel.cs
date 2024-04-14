using Logic;
using System.Numerics;

namespace Model
{
    public class BallModel
    {
        private LogicAPI logic;
        private double x;
        private double y;

        public BallModel()
        {
            logic = LogicAPI.CreateObjLogic();
            x = logic.getBallPosition().X;
            y = logic.getBallPosition().Y;
        }

        public double ModelXPosition
        {
            get
            {
                return logic.getBallPosition().X;
            }
            set
            {
                logic.setBallXPosition(value);
            }
        }

        public double ModelYPosition
        {
            get
            {
                return logic.getBallPosition().Y;
            }
            set
            {
                logic.setBallYPosition(value);
            }
        }

        public Vector2 getModelPosition()
        {
            return new Vector2((float)ModelXPosition, (float)ModelYPosition);
        }

        public Vector2 GetBallPosition()
        {
            return logic.PutBall();
        }
    }
}
