using Data;

namespace DataTest
{
    [TestClass]
    public class DataAPITest
    {

        [TestMethod]
        public void ConstructorTest()
        {
            DataAbstractAPI dataAbstractAPI = DataAbstractAPI.CreateApi(100, 100);
            dataAbstractAPI.CreateBallsList(5);
            Assert.AreEqual(3, dataAbstractAPI.GetBall(2).BallId);
            Assert.AreEqual(5, dataAbstractAPI.GetAmount);
            dataAbstractAPI.CreateBallsList(-3);
            Assert.AreEqual(2, dataAbstractAPI.GetAmount);
            dataAbstractAPI.CreateBallsList(-3);
            Assert.AreEqual(0, dataAbstractAPI.GetAmount);
        }
    }
}