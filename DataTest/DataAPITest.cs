using Data;

namespace DataTest
{
    [TestClass]
    public class DataAPITest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            DataAPI ball = DataAPI.CreateBall(7, 1);
            Assert.AreEqual(ball.getX(), 7);
            Assert.AreEqual(ball.getY(), 1);
        }

        [TestMethod]
        public void SetterTest()
        {
            DataAPI ball = DataAPI.CreateBall(7, 6);
            ball.setX(1);
            ball.setY(2);
            Assert.AreEqual(ball.getX(), 1);
            Assert.AreEqual(ball.getY(), 2);
        }
    }
}