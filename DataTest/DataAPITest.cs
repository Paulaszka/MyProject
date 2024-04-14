using Data;

namespace DataTest
{
    [TestClass]
    public class DataAPITest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            DataAPI ball = DataAPI.CreateBall(7,1);
            Assert.AreEqual(ball.posX, 7);
            Assert.AreEqual(ball.posY, 1);
        }

        [TestMethod]
        public void SetterTest()
        {
            DataAPI ball = DataAPI.CreateBall(7, 6);
            ball.posX = 1;
            ball.posY = 2;
            Assert.AreEqual(ball.posX, 1);
            Assert.AreEqual(ball.posY, 2);
        }
    }
}