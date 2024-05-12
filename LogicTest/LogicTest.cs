using Logic;

namespace LogicTest
{
    [TestClass]
    public class CreationTest
    {
        [TestMethod]
        public void ConstuctorTest()
        {
            LogicAbstractAPI logicAbstractAPI = LogicAbstractAPI.CreateApi(100, 200);
            Assert.IsNotNull(logicAbstractAPI);
            Assert.AreEqual(100, logicAbstractAPI.width);
            Assert.AreEqual(200, logicAbstractAPI.height);
        }

        [TestMethod]
        public void BallNumberTest()
        {
            LogicAbstractAPI logicAbstractAPI = LogicAbstractAPI.CreateApi(100, 200);
            logicAbstractAPI.CreateBalls(5);
            Assert.AreEqual(5, logicAbstractAPI.GetAmount);
            logicAbstractAPI.CreateBalls(-3);
            Assert.AreEqual(2, logicAbstractAPI.GetAmount);
            logicAbstractAPI.CreateBalls(-3);
            Assert.AreEqual(0, logicAbstractAPI.GetAmount);
        }
    }
}