namespace MinesweeperModel
{
    public class Cell
    {
        public bool IsBomb { get; set; }
        public int BombCount { get; set; }
        public bool IsFlagged { get; set; }
    }
}