using System;
using System.Collections.Generic;
using System.Drawing;

namespace MinesweeperModel
{
    // Reference Game: https://www.google.com/search?q=minesweeper&rlz=1C1EJFC_enUS867US867&oq=mine&aqs=chrome.0.69i59j46i67i433j46i67j46i67i433j0i20i263i433i512j46i433i512l2j69i60.1199j0j7&sourceid=chrome&ie=UTF-8
    // There is much more to the IModel than what we completed in class
    // the first casualty of war is the plan
    public interface IModel
    {
        //ops
        /// <summary>
        /// Opens Cell
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>List of Points that changed due to this operation. If Bomb, that point will be only returned value</returns>
        List<System.Drawing.Point> OpenCell(int col, int row);
        void Setup(DifficultyLevel level);
        void FlagCell(int col, int row, bool flagOn);
        int GetTime(); // In Seconds

        Cell GetCell(int col, int row);
    }

    public class Model : IModel
    {
        //imp
        internal Cell[,] board;

        public DifficultyLevel difficultyLevel { get; private set; }

        public void FlagCell(int col, int row, bool flagOn)
        {
            // data validation
            board[col, row].IsFlagged = flagOn;
        }

        public Cell GetCell(int col, int row)
        {
            throw new NotImplementedException();
        }

        public int GetTime()
        {
            throw new NotImplementedException();
        }

        public List<System.Drawing.Point> OpenCell(int col, int row)
        {
            return null;
        }

        public void Setup(DifficultyLevel level)
        {
            this.difficultyLevel = level;
            board = new Cell[difficultyLevel.GetSize().Y, difficultyLevel.GetSize().X];
            for (int x = 0; x < board.GetLength(1); x++)
                for (int y = 0; y < board.GetLength(0); y++)
                    board[y, x] = new Cell();

        }

        internal void SetNeighborCount()
        {
            throw new NotImplementedException();
        }

        List<Point> IModel.OpenCell(int col, int row)
        {
            throw new NotImplementedException();
        }
    }


}
