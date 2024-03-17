using MyProject;

namespace MyTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(5, App.funkcja(2, 3));
               
        }
    }
}