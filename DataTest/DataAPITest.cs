using Data;
using System.Numerics;

namespace DataTest
{
    [TestClass]
    public class DataAPITest
    {

        [TestMethod]
        public void ConstructorTest()
        {
            IPosition position1 = IPosition.CreatePosition(100, 200);
            Vector2 vector = new(5, 10);
            DataAbstractAPI ball = DataAbstractAPI.CreateApi(1,position1, vector);
            Assert.AreEqual(100, ball.BallPosition.X);
            Assert.AreEqual(200, ball.BallPosition.Y);
            Assert.AreEqual(5, ball.Velocity.X);
            Assert.AreEqual(10, ball.Velocity.Y);
        }
    }
}