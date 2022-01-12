using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MinesweeperModel.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Model m = new Model();

            m.board[0, 0] = new Cell() { IsBomb = true };
            m.SetNeighborCount();

            m.board[0, 1].Should().Be(1);
            m.board[1, 0].Should().Be(1);
            m.board[1, 1].Should().Be(1);
        }
    }
}
