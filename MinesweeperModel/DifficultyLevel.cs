using System.Drawing;

namespace MinesweeperModel
{
    public enum DifficultyLevel
    {
        Easy = 0, // 10 x 8
        Medium = 1,// 
        Hard = 2
    }

    public static class Extensions
    {
        public static Point GetSize(this DifficultyLevel dl )
        {
            switch (dl)
            {
                case DifficultyLevel.Easy:
                    return new Point { X = 10, Y = 8 };
                case DifficultyLevel.Medium:
                    return new Point { X = 15, Y = 12 };
                case DifficultyLevel.Hard:
                default:
                    return new Point { X = 20, Y = 20 };
            }
        }
    }
}