using System.Numerics;
using Data;
using Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        internal class TempBall : TempDataAPI
        {
            public TempBall(double _x, double _y)
            {
                x = _x;
                y = _y;
            }

            private double x;
            private double y;

            public override double posX
            {
                get
                {
                    return x;
                }
                set
                {
                    x = value;
                }
            }

            public override double posY
            {
                get
                {
                    return y;
                }
                set
                {
                    y = value;
                }
            }
        }

        internal class TempDataAPI : DataAPI 
        {
            public static DataAPI CreateBall(double x, double y)
            {
                return new TempBall(x, y);
            }

            public virtual double posX { get; set; }

            public virtual double posY { get; set; }

        }
        
        [TestMethod]
        public void TestBallMovement()
        {
            TempDataAPI dataAPI = new TempDataAPI();
            LogicAPI logicAPI = LogicAPI.CreateObjLogic(dataAPI);
            logicAPI.setBallXPosition(7);
            logicAPI.setBallYPosition(8);
            Vector2 vector2 = logicAPI.getBallPosition();
            Assert.AreEqual(vector2[0], 7);
            Assert.AreEqual(vector2[1], 8);
        }
    }
}