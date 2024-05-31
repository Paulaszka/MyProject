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
            Position position1 = new(100, 200);
            Vector2 vector = new(5, 10);
            DataAbstractAPI ball = DataAbstractAPI.CreateApi(20, position1, vector, 30);
            Assert.AreEqual(20, ball.BallSize);
            Assert.AreEqual(100, ball.BallPosition.X);
            Assert.AreEqual(200, ball.BallPosition.Y);
            Assert.AreEqual(5, ball.Velocity.X);
            Assert.AreEqual(10, ball.Velocity.Y);
            Assert.AreEqual(30, ball.BallWeight);
        }
    }
}