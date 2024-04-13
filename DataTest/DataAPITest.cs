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
            Assert.AreEqual(ball.X, 7);
            Assert.AreEqual(ball.Y, 1);
        }

        [TestMethod]
        public void SetterTest()
        {
            DataAPI ball = DataAPI.CreateBall(7, 6);
            ball.X = 1;
            ball.Y = 2;
            Assert.AreEqual(ball.X, 1);
            Assert.AreEqual(ball.Y, 2);
        }
    }
}