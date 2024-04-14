using System.Numerics;
using Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        [TestClass]
        public class LogicAPITest
        {
            [TestMethod]
            public void TestBallMovement()
            {
                LogicAPI logicAPI = LogicAPI.CreateObjLogic();
                logicAPI.setBallXPosition(7);
                logicAPI.setBallYPosition(8);
                Vector2 vector2 = logicAPI.getBallPosition();
                Assert.AreEqual(vector2[0], 7);
                Assert.AreEqual(vector2[1], 8);
            }
        }
    }
}